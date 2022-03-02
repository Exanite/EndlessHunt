using System.Collections;
using System.Collections.Generic;
using Project.Source;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class EndOfGameCutscene : MonoBehaviour
{
    public string nextScene = "Credits";

    public PlayerMovement player;
    public SpriteRenderer playerSprite;
    public SpriteRenderer playerSpriteWhite;
    public ShadowCaster2D playerShadow;

    public List<Behaviour> componentsToDisable;
    public List<GameObject> objectsToDisable;

    public Transform marker1;
    public Transform marker2;
    public Transform marker3;
    public Transform marker4;
    public Transform marker4hardmode;

    public AnimationCurve jumpUpCurve;
    public float jumpUpDuration = 1;

    public AnimationCurve jumpDownCurve;
    public float jumpDownDuration = 1;

    public AnimationCurve ascendCurve;
    public AnimationCurve ascendColorCurve;
    public float ascendDuration = 1;

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
            var targetPosition = Vector3.Lerp(marker2.position, marker3.position, jumpUpCurve.Evaluate(sharedTimer / jumpUpDuration));
            player.transform.position = targetPosition;

            yield return null;
        }

        sharedTimer = 0;
        playerShadow.enabled = false;
        playerSprite.sortingLayerName = "Ground";
        playerSprite.sortingOrder = -50;

        if (GameSettings.Instance.isHardmode)
        {
            player.playerCamera.transform.parent = null;
            playerSpriteWhite.gameObject.SetActive(true);
            playerSprite.sprite = playerSpriteWhite.sprite;
            player.playerAnimator.enabled = false;

            while (sharedTimer < ascendDuration)
            {
                sharedTimer += Time.deltaTime;
                var targetPosition = Vector3.Lerp(marker3.position, marker4hardmode.position, ascendCurve.Evaluate(sharedTimer / ascendDuration));
                player.transform.position = targetPosition;

                playerSpriteWhite.color = Color.Lerp(new Color(1, 1, 1, 0), Color.white, ascendColorCurve.Evaluate(sharedTimer / ascendDuration));

                yield return null;
            }
        }
        else
        {
            while (sharedTimer < jumpDownDuration)
            {
                sharedTimer += Time.deltaTime;
                var targetPosition = Vector3.Lerp(marker3.position, marker4.position, jumpDownCurve.Evaluate(sharedTimer / jumpDownDuration));
                player.transform.position = targetPosition;

                if (sharedTimer / jumpDownDuration > 0.3f)
                {
                    playerSprite.enabled = false;
                }

                yield return null;
            }
        }

        yield return new WaitForSeconds(1f);

        yield return FadeTransition.Instance.FadeToBlack(2f);

        SceneManager.LoadScene(nextScene);
    }
}