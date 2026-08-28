using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

public class PlayerController : MonoBehaviour
{
    public Text Remaining;
    public Text YouWin;
    public Text Attempts;
    public Text Timer;
    public Text BestScore;
    public Text WinStats;

    public int CardsRemaining { get; set; }
    public int FailedAttempts { get; set; }
    public bool IsProcessing { get; set; }
    public GameObject LastSelected { get; set; }
    public GameObject FirstSelected { get; set; }
    public Button buttonResetGame;

    public float MinYPosition = -0.22f;

    private const string PrefBestTime     = "best_time";
    private const string PrefBestAttempts = "best_attempts";

    private GameObject[] RealCards;
    private GameObject[] YouWinObjects;
    private Music musicManager;
    private float _elapsedTime;
    private bool _timerRunning;

    void Start()
    {
        musicManager = FindFirstObjectByType<Music>();
        // SONIDO FALTANTE: BGM de gameplay — actualmente la música se detiene al entrar al juego
        // Sugerencia: loop ambiental o de tensión distinto al menú (ej. "gameplay_bgm.mp3")
        // Para agregar: quitar StopMusic(), cambiar el clip del AudioSource de Music por la pista de juego
        if (musicManager != null) musicManager.StopMusic();

        RealCards = GameObject.FindGameObjectsWithTag("Cards");
        YouWinObjects = GameObject.FindGameObjectsWithTag("YouWin");

        foreach (var win in YouWinObjects)
            win.SetActive(false);

        SetPositionOfCards();
        UpdateBestScoreHUD();

        buttonResetGame.onClick.AddListener(RestartMatch);
    }

    private void Update()
    {
        if (_timerRunning)
            _elapsedTime += Time.deltaTime;

        Remaining.text = LocalizationManager.Get("ui.remaining", CardsRemaining);
        if (Attempts != null)
            Attempts.text = LocalizationManager.Get("ui.attempts", FailedAttempts);
        if (Timer != null)
            Timer.text = string.Format("{0:00}:{1:00}", (int)_elapsedTime / 60, (int)_elapsedTime % 60);
    }

    void SetPositionOfCards()
    {
        IsProcessing = true;
        LastSelected = null;
        FirstSelected = null;
        CardsRemaining = RealCards.Length;
        FailedAttempts = 0;
        _elapsedTime = 0f;
        _timerRunning = false;

        for (int i = 0; i < RealCards.Length; i++)
        {
            var card = RealCards[i].GetComponent<CardController>();
            if (card != null)
            {
                var mat = RealCards[i].GetComponent<MeshRenderer>()?.sharedMaterial;
                card.PairId = mat != null ? mat.name : RealCards[i].name;
            }
        }

        var rnd = new Random();
        var randomized = RealCards.Select(c => c.transform.position)
                                  .OrderBy(_ => rnd.Next())
                                  .ToList();

        for (int i = 0; i < RealCards.Length; i++)
        {
            RealCards[i].SetActive(true);
            RealCards[i].transform.position = new Vector3(randomized[i].x, MinYPosition, randomized[i].z);
            RealCards[i].GetComponent<CardController>()?.ResetCard();
        }

        StartCoroutine(AnimateCardsIn());
    }

    private IEnumerator AnimateCardsIn()
    {
        const float startY      = 8f;
        const float dropDuration = 0.45f;
        const float stagger     = 0.06f;

        foreach (var card in RealCards)
        {
            var pos = card.transform.position;
            card.transform.position = new Vector3(pos.x, startY, pos.z);
        }

        var order = RealCards.OrderBy(_ => UnityEngine.Random.value).ToArray();
        int landed = 0;

        for (int i = 0; i < order.Length; i++)
        {
            float duration = dropDuration + UnityEngine.Random.Range(-0.05f, 0.05f);
            StartCoroutine(DropCard(order[i].transform, startY, MinYPosition, duration, () => landed++));
            yield return new WaitForSeconds(stagger);
        }

        yield return new WaitUntil(() => landed >= order.Length);

        IsProcessing = false;
        _timerRunning = true;
    }

    private IEnumerator DropCard(Transform card, float fromY, float toY, float duration, System.Action onComplete)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            float eased = 1f - Mathf.Pow(1f - t, 3f); // cubic ease-out
            var pos = card.position;
            pos.y = Mathf.Lerp(fromY, toY, eased);
            card.position = pos;
            yield return null;
        }

        var final = card.position;
        final.y = toY;
        card.position = final;

        // SONIDO FALTANTE: golpe suave al aterrizar cada carta durante la animación de entrada
        // Sugerencia: tap o "thud" corto ~0.1s, volumen bajo para no saturar con 20 cartas (ej. "card_land.wav")
        // Agregar campo: public AudioClip cardLandClip;  — asignar en el Inspector del PlayerController
        // SoundManager.Instance?.PlaySFX(card.GetComponent<AudioSource>(), cardLandClip);

        onComplete();
    }

    public void EndMatch()
    {
        _timerRunning = false;

        bool newBestTime     = TrySaveBest(PrefBestTime,     _elapsedTime,   (a, b) => a < b);
        bool newBestAttempts = TrySaveBest(PrefBestAttempts, FailedAttempts, (a, b) => a < b);

        // SONIDO FALTANTE: jingle de victoria al completar todos los pares
        // Sugerencia: fanfare corta ~1-2s (ej. "win_jingle.wav")
        // Agregar campo: public AudioClip winClip;  — asignar en el Inspector del PlayerController
        // GetComponent<AudioSource>()?.PlayOneShot(winClip, SoundManager.Instance?.SFXVolume ?? 1f);

        // SONIDO FALTANTE: sonido especial al batir el récord personal
        // Sugerencia: efecto adicional al jingle, más festivo (ej. "new_record.wav")
        // if (newBestTime || newBestAttempts)
        //     GetComponent<AudioSource>()?.PlayOneShot(newRecordClip, SoundManager.Instance?.SFXVolume ?? 1f);

        if (WinStats != null)
        {
            string run  = string.Format("{0}  |  {1}", FormatTime(_elapsedTime), LocalizationManager.Get("ui.attempts", FailedAttempts));
            string best = string.Format("{0}  |  {1}", FormatTime(PlayerPrefs.GetFloat(PrefBestTime)), LocalizationManager.Get("ui.attempts", PlayerPrefs.GetInt(PrefBestAttempts)));
            WinStats.text = run + "\n" + LocalizationManager.Get("ui.best") + ": " + best;
            if (newBestTime || newBestAttempts)
                WinStats.text += "\n" + LocalizationManager.Get("ui.new_record");
        }

        UpdateBestScoreHUD();

        foreach (var win in YouWinObjects)
            win.SetActive(true);
    }

    public void RestartMatch()
    {
        foreach (var win in YouWinObjects)
            win.SetActive(false);

        SetPositionOfCards();
        UpdateBestScoreHUD();
    }

    private bool TrySaveBest<T>(string key, T current, System.Func<T, T, bool> isBetter) where T : struct
    {
        T saved = key == PrefBestTime
            ? (T)(object)PlayerPrefs.GetFloat(key, float.MaxValue)
            : (T)(object)PlayerPrefs.GetInt(key, int.MaxValue);

        if (!isBetter(current, saved)) return false;

        if (key == PrefBestTime) PlayerPrefs.SetFloat(key, (float)(object)current);
        else                     PlayerPrefs.SetInt(key, (int)(object)current);
        PlayerPrefs.Save();
        return true;
    }

    private void UpdateBestScoreHUD()
    {
        if (BestScore == null) return;
        float savedTime = PlayerPrefs.GetFloat(PrefBestTime, -1f);
        BestScore.text = savedTime < 0
            ? LocalizationManager.Get("ui.best") + ": --:--"
            : LocalizationManager.Get("ui.best") + ": " + FormatTime(savedTime);
    }

    private string FormatTime(float seconds) =>
        string.Format("{0:00}:{1:00}", (int)seconds / 60, (int)seconds % 60);
}
