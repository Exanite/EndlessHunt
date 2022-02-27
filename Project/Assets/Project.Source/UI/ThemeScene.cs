using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ThemeScene : MonoBehaviour
{
    public float fadeToClearDuration = 2;
    public float waitDuration = 1;
    public float fadeToBlackDuration = 2;
    
    public string nextScene;

    private IEnumerator Start()
    {
        yield return FadeTransition.Instance.FadeToClear(fadeToClearDuration);
        yield return new WaitForSeconds(waitDuration);
        yield return FadeTransition.Instance.FadeToBlack(fadeToBlackDuration);

        SceneManager.LoadScene(nextScene);
    }
}