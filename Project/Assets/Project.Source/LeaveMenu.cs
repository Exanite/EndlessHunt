using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaveMenu : MonoBehaviour
{
    public FadeTransition fadeTransition;
    public float duration = 3;
    void Start()
    {
        
    }
    void FixedUpdate()
    {
        if(Input.GetKeyDown("space"))
            fadeTransition.FadeToBlack(duration);
    }
}
