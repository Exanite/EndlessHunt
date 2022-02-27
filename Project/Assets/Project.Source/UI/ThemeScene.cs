using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ThemeScene : MonoBehaviour
{
    public string nextScene;

    private IEnumerator Start()
    {
        yield return FadeTransition.Instance.FadeToClear(2f);
        yield return new WaitForSeconds(1);
        yield return FadeTransition.Instance.FadeToBlack(2f);

        SceneManager.LoadScene(nextScene);
    }
}