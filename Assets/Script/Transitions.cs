using System.Collections;
using UnityEngine;

public class Transitions : MonoBehaviour
{
    private Animator _transitionAnim;
    private GameManager gameManager;

    void Start()
    {
        _transitionAnim = GetComponent<Animator>();
        gameManager = FindFirstObjectByType<GameManager>();
    }

    public void LoadScene(string scene)
    {
        StartCoroutine(Transition(scene));
    }

    IEnumerator Transition(string scene)
    {
        _transitionAnim.SetTrigger("ExitTrigger");
        yield return new WaitForSeconds(1);
        gameManager.ChangeScene(scene);
    }
}
