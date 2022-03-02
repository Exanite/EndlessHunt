using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LeaveMenu : MonoBehaviour
{
    public string nextScene;
    public float duration = 0.5f;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(Fade());
        }
    }

    private IEnumerator Fade()
    {
        yield return FadeTransition.Instance.FadeToBlack(duration);

        SceneManager.LoadScene(nextScene);
    }
}