using UnityEngine;
using UnityEngine.UI;

public class ButtonsManager : MonoBehaviour
{
    private GameManager gameManager;

    public Button buttonStart, buttonGame, buttonOptions, buttonCredits, buttonExit;

    // SONIDO FALTANTE: clic de botón para navegación de menú
    // Sugerencia: clic UI sutil ~0.1s (ej. "button_click.wav")
    // Pasos para agregar:
    //   1. Agregar un AudioSource a este GameObject en el Inspector
    //   2. Declarar: public AudioClip buttonClickClip;
    //   3. En cada onClick reemplazar la lambda por un método que primero llame:
    //      SoundManager.Instance?.PlaySFX(GetComponent<AudioSource>(), buttonClickClip);

    void Start()
    {
        gameManager = FindFirstObjectByType<GameManager>();

        if (buttonStart)
            buttonStart.onClick.AddListener(() => gameManager.ChangeScene("Start"));

        if (buttonGame)
            buttonGame.onClick.AddListener(() => gameManager.ChangeScene("Game"));

        if (buttonOptions)
            buttonOptions.onClick.AddListener(() => gameManager.ChangeScene("Options"));

        if (buttonCredits)
            buttonCredits.onClick.AddListener(() => gameManager.ChangeScene("Credits"));

        if (buttonExit)
            buttonExit.onClick.AddListener(Application.Quit);
    }
}
