using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OADespawner : MonoBehaviour
{
    [SerializeField]
    private List<SpriteRenderer> srList = new List<SpriteRenderer>();

    [SerializeField]
    private GameObject parentObject = null;

    public float fadeStepDelay = .1f;
    public float fadeStride = .02f;

    public void Start()
    {
#if UNITY_EDITOR
        if (srList.Count < 0)
            Debug.LogError("OADespawner srList is 0 in lenght");

        OAExtentions.AssertObjectNotNull(parentObject, "OADespawner is missing parentObject");
#endif
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
        
        // TODO: supply to a pool at this point instead!
        parentObject.SetActive(false);
    }
}
