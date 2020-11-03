using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OADespawner : MonoBehaviour
{
    private UnityEvent<OADespawner> onDespawnSelf = new UnityEvent<OADespawner>();
    public UnityEvent<OADespawner> OnDespawnSelf { get => onDespawnSelf; }


    [SerializeField]
    private List<SpriteRenderer> srList = new List<SpriteRenderer>();

    public float fadeStepDelay = .1f;
    public float fadeStride = .02f;


    public void Start()
    {
        if (srList.Count < 0)
            Debug.LogError("OADespawner srList is 0 in length");
    }

    /// <summary>
    /// Resets material alpha to 1
    /// </summary>
    public void Respawn()
    {
        foreach (var spriteRenderer in srList)
        {
            Color c = spriteRenderer.material.color;
            c.a = 1f;
            spriteRenderer.material.color = c;
        }
    }

    /// <summary>
    /// Used to handle the despawning of a object
    /// </summary>
    /// <param name="fadeStepDelay">How often we update the color alpha</param>
    /// <param name="fadeStride">How much we subtract from color alpha on each step</param>
    public void Despawn()
    {
        StartCoroutine(Fade());
    }

    IEnumerator Fade()
    {
        for (float ft = 1f; ft >= 0; ft -= fadeStride)
        {
            foreach (var spriteRenderer in srList)
            {
                Color c = spriteRenderer.material.color;
                c.a = ft;
                spriteRenderer.material.color = c;
            }
            yield return new WaitForSeconds(fadeStepDelay);
        }

        // so that pooling can retake object
        onDespawnSelf.Invoke(this);
    }
}
