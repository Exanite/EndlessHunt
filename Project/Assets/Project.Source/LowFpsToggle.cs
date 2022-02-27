using System;
using System.Collections.Generic;
using UnityEngine;

public class LowFpsToggle : MonoBehaviour
{
    public bool isLowFps;
    
    [Header("Disable on low fps")]
    public List<GameObject> disableGameObjects;
    public List<Behaviour> disableComponents;
    
    [Header("Enable on low fps")]
    public List<GameObject> enableGameObjects;
    public List<Behaviour> enableComponents;

    private void Start()
    {
        if (Application.platform == RuntimePlatform.WebGLPlayer)
        {
            isLowFps = true;
        }

        if (isLowFps)
        {
            foreach (var disableGameObject in disableGameObjects)
            {
                disableGameObject.SetActive(false);
            }

            foreach (var disableComponent in disableComponents)
            {
                disableComponent.enabled = false;
            }
            
            foreach (var enableGameObject in enableGameObjects)
            {
                enableGameObject.SetActive(true);
            }
            
            foreach (var enableComponent in enableComponents)
            {
                enableComponent.enabled = true;
            }
        }
    }
}