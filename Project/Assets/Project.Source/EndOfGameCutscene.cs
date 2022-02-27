using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndOfGameCutscene : MonoBehaviour
{
    public PlayerMovement player;

    public List<Behaviour> componentsToDisable;
    public List<GameObject> objectsToDisable;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.attachedRigidbody && other.attachedRigidbody.TryGetComponent(out PlayerMovement player))
        {
            StartCoroutine(StartCutscene());
        }
    }

    private IEnumerator StartCutscene()
    {
        Debug.Log("Starting cutscene");
        
        foreach (var component in componentsToDisable)
        {
            component.enabled = false;
        }

        foreach (var gameObject in objectsToDisable)
        {
            gameObject.SetActive(false);
        }

        yield return null;
    }
}