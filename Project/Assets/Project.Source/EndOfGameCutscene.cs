using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class EndOfGameCutscene : MonoBehaviour
{
    public string nextScene = "Credits";
    
    public PlayerMovement player;
    public SpriteRenderer playerSprite;
    public ShadowCaster2D playerShadow;

    public List<Behaviour> componentsToDisable;
    public List<GameObject> objectsToDisable;

    public Transform marker1;
    public Transform marker2;
    public Transform marker3;
    public Transform marker4;

    public AnimationCurve jumpUpCurve;
    public float jumpUpDuration;
    
    public AnimationCurve jumpDownCurve;
    public float jumpDownDuration;

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

        player.transform.position = marker1.position;

        var sharedTimer = 0f;
        var walkToEdgeDuration = 3f;

        while (sharedTimer < walkToEdgeDuration)
        {
            sharedTimer += Time.deltaTime;
            var targetPosition = Vector3.Lerp(marker1.position, marker2.position, sharedTimer / walkToEdgeDuration);
            player.transform.position = targetPosition;
            player.playerAnimator.SetBool(PlayerMovement.IsWalking, true);

            yield return null;
        }

        player.playerAnimator.SetBool(PlayerMovement.IsWalking, false);
        sharedTimer = 0;

        yield return new WaitForSeconds(1f);
        
        while (sharedTimer < jumpUpDuration)
        {
            sharedTimer += Time.deltaTime;
            var targetPosition = Vector3.Lerp(marker2.position, marker3.position, sharedTimer / jumpUpDuration);
            player.transform.position = targetPosition;

            yield return null;
        }

        sharedTimer = 0;
        playerShadow.enabled = false;
        
        while (sharedTimer < jumpDownDuration)
        {
            sharedTimer += Time.deltaTime;
            var targetPosition = Vector3.Lerp(marker3.position, marker4.position, sharedTimer / jumpDownDuration);
            player.transform.position = targetPosition;
            playerSprite.sortingLayerName = "Ground";
            playerSprite.sortingOrder = -50;

            if (sharedTimer / jumpDownDuration > 0.3f)
            {
                playerSprite.enabled = false;
            }
            
            yield return null;
        }

        yield return new WaitForSeconds(2f);
        
        SceneManager.LoadScene(nextScene);
    }
}