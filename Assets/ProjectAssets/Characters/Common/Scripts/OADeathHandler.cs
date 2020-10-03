using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OADeathHandler : MonoBehaviour
{
    [SerializeField]
    private GameObject deadObject = null;

    public void OnDead(SpriteRenderer diedObject)
    {
        diedObject.gameObject.SetActive(false);
        deadObject.SetActive(true);
        
        if (diedObject.flipX == true)
        {
            Vector3 scale = deadObject.transform.localScale;
            scale.x = scale.x * -1;
            deadObject.transform.localScale = scale;
        }

        deadObject.transform.position = diedObject.transform.position;
    }

    // Start is called before the first frame update
    void Start()
    {
        OAExtentions.AssertObjectNotNull(deadObject, "deadObject not set in OADeathHandler");
    }
}
