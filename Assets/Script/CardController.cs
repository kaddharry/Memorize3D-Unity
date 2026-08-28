using System.Collections;
using UnityEngine;

public class CardController : MonoBehaviour
{
    public float RotateSpeed = 50f;
    public Vector3 RotateStep = new Vector3(0, 0, 180);
    [Tooltip("Set to the same value for cards that are a matching pair")]
    public string PairId;

    private PlayerController playerController;
    private AudioSource flipAudio;
    private Quaternion _targetRot = Quaternion.identity;
    private bool _isFaceUp = false;

    private string MatchKey => string.IsNullOrEmpty(PairId) ? gameObject.name : PairId;

    private void Start()
    {
        playerController = FindFirstObjectByType<PlayerController>();
        flipAudio = GetComponents<AudioSource>()[0];
    }

    void Update()
    {
        transform.rotation = Quaternion.Lerp(transform.rotation, _targetRot, RotateSpeed * Time.deltaTime);
    }

    public void FlipFaceUp()
    {
        _isFaceUp = true;
        _targetRot = Quaternion.Euler(RotateStep);
    }

    public void FlipFaceDown()
    {
        _isFaceUp = false;
        _targetRot = Quaternion.identity;
    }

    public IEnumerator ShakeAndFlipDown()
    {
        Vector3 originalPos = transform.position;
        float duration = 0.35f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float dampen = 1f - (elapsed / duration);
            float xOffset = Mathf.Sin(elapsed * Mathf.PI * 18f) * 0.06f * dampen;
            transform.position = new Vector3(originalPos.x + xOffset, originalPos.y, originalPos.z);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = originalPos;
        FlipFaceDown();
    }

    public void ResetCard()
    {
        _isFaceUp = false;
        _targetRot = Quaternion.identity;
        transform.rotation = Quaternion.identity;
    }

    private void OnMouseDown()
    {
        StartCoroutine(HandleClick());
    }

    private IEnumerator HandleClick()
    {
        if (playerController.IsProcessing) yield break;
        if (_isFaceUp) yield break;

        Debug.Log($"[Card] {gameObject.name} — PairId: '{PairId}'");
        SoundManager.Instance?.PlaySFX(flipAudio);
        FlipFaceUp();

        if (playerController.FirstSelected == null)
        {
            playerController.FirstSelected = this.gameObject;
            yield break;
        }

        playerController.IsProcessing = true;
        playerController.LastSelected = this.gameObject;

        yield return new WaitForSeconds(0.5f);

        CardController firstCard = playerController.FirstSelected.GetComponent<CardController>();
        bool isMatch = firstCard != null && firstCard.MatchKey == MatchKey;

        if (isMatch)
        {
            // SONIDO FALTANTE: acierto al emparejar dos cartas
            // Sugerencia: tono positivo corto ~0.3s (ej. "match_success.wav" o "coin.wav")
            // Agregar campo: public AudioClip matchClip;  — asignar en el Inspector del prefab Card
            // SoundManager.Instance?.PlaySFX(flipAudio, matchClip);

            playerController.FirstSelected.SetActive(false);
            gameObject.SetActive(false);
            playerController.CardsRemaining -= 2;

            if (playerController.CardsRemaining == 0)
                playerController.EndMatch();
        }
        else
        {
            // SONIDO FALTANTE: error al no emparejar
            // Sugerencia: buzz o thud suave ~0.2s, distinto al sonido de volteo (ej. "wrong.wav")
            // Agregar campo: public AudioClip failClip;  — asignar en el Inspector del prefab Card
            // SoundManager.Instance?.PlaySFX(flipAudio, failClip);

            playerController.FailedAttempts++;
            if (firstCard != null) StartCoroutine(firstCard.ShakeAndFlipDown());
            yield return StartCoroutine(ShakeAndFlipDown());
        }

        playerController.FirstSelected = null;
        playerController.LastSelected = null;
        playerController.IsProcessing = false;
    }
}
