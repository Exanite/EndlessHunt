using System.Collections;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    public bool canTeleport;

    private void Awake()
    {
        canTeleport = true;
    }

    public Transform target;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!canTeleport)
        {
            return;
        }

        if (other.attachedRigidbody && other.attachedRigidbody.TryGetComponent(out PlayerMovement player))
        {
            StartCoroutine(Teleport(player));
        }
    }

    private IEnumerator Teleport(PlayerMovement player)
    {
        canTeleport = false;
        
        yield return StartCoroutine(FadeTransition.Instance.FadeIn(0.15f));

        player.transform.position = target.position;
        
        yield return StartCoroutine(FadeTransition.Instance.FadeOut(0.3f));

        canTeleport = true;
    }
}