using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Main_Menu : MonoBehaviour
{
    [SerializeField] private Animator animator;

    public void OpenGame()
    {
        StartCoroutine(CloseMenu());
    }

    IEnumerator CloseMenu()
    {
        animator.SetTrigger("isClosing");
        yield return new WaitForSeconds(1.25f);
        SceneManager.LoadScene(1);
    }
}
