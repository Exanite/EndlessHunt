using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaveMenu : MonoBehaviour
{
    public string nextScene = "";
    public FadeTransition fadeTransition;
    public float duration = .5f;
    void Start()
    {
        fadeTransition = FindObjectOfType<Canvas>().GetComponentInChildren<FadeTransition>();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            //Debug.Log("space pressed");
            StartCoroutine(Fade());
        }
    }

    private IEnumerator Fade()
    {
        yield return FadeTransition.Instance.FadeToBlack(duration);
        yield return new WaitForSeconds(0.2f);
        SceneManager.LoadScene(nextScene);
    }
}
