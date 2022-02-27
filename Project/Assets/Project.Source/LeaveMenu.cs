
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class LeaveMenu : MonoBehaviour, PlayerInput.IMenuActions
{
    public FadeTransition fadeTransition;
    public float duration = 3;
    void Start()
    {
        fadeTransition = FindObjectOfType<Canvas>().GetComponentInChildren<FadeTransition>();
    }

    public void OnMenuEscape(InputAction.CallbackContext context)
    {
        Debug.Log("space pressed");
        fadeTransition.FadeToBlack(duration);
    }
}
