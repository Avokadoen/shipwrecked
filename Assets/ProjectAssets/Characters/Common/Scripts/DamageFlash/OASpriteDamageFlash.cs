using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Used to make a sprite renderer flash when the host object recieves damage
/// </summary>
public class OASpriteDamageFlash : MonoBehaviour
{
    [Tooltip("How long the sprite will flash in seconds")]
    [SerializeField]
    float flashTime = 0.3f;

    float actualFlashTime; 

    [Tooltip("The color to flash to")]
    [SerializeField]
    Color targetColor = Color.white;

    float flashDir = 1f;
    float flashPos = 0f;

    [SerializeField]
    OAKillable killable;

    SpriteRenderer spriteRenderer;

    void Start()
    {
        // Attempt to find killable on the object
        if (!killable)
            killable = GetComponent<OAKillable>();

        spriteRenderer = GetComponent<SpriteRenderer>();


        killable.OnHurt.AddListener(() => {
            flashDir = 1;
            flashPos = 0;
        });

        flashPos = -1;
        actualFlashTime = flashTime * 0.5f;

        spriteRenderer.material.SetFloat("_FlashTime", flashTime);
        spriteRenderer.material.SetColor("_TargetColor", targetColor);
    }

    void Update()
    {
        if (flashPos < 0)
        {
            spriteRenderer.material.SetFloat("_FlashPos", 0);
            return;
        }

        if (flashPos > actualFlashTime)
            flashDir = -1f;

        spriteRenderer.material.SetFloat("_FlashPos", flashPos);

        flashPos += Time.deltaTime * flashDir;

    }
}
