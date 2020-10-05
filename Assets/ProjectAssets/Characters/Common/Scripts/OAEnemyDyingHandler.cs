using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO: integrate with pool mechanism
/// <summary>
/// Used to replace animated alive sprite with ragdoll sprite on death
/// </summary>
public class OAEnemyDyingHandler : MonoBehaviour
{
    [Tooltip("Object that will replace the killable object on death")]
    [SerializeField]
    private GameObject deadObject = null;

    [Tooltip("Despawner for dead object")]
    [SerializeField]
    private OADespawner deadDespawner = null;

    [SerializeField]
    private SpriteRenderer killableSprite = null; 

    [SerializeField]
    private OAKillable killable = null;

    public void OnDead()
    {
        killableSprite.gameObject.SetActive(false);
  
        deadObject.SetActive(true);
        
        if (killableSprite.flipX == true)
        {
            Vector3 scale = deadObject.transform.localScale;
            scale.x = scale.x * -1;
            deadObject.transform.localScale = scale;
        }

        deadObject.transform.position = killableSprite.transform.position;

        deadDespawner.Despawn();
    }

    // Start is called before the first frame update
    void Start()
    {
        OAExtentions.AssertObjectNotNull(deadObject, "deadObject not set in OADeathHandler");
        OAExtentions.AssertObjectNotNull(deadDespawner, "deadDespawner not set in OADeathHandler");
        OAExtentions.AssertObjectNotNull(killableSprite, "deadDespawner not set in OADeathHandler");
        OAExtentions.AssertObjectNotNull(killable, "killable not set in OADeathHandler");

        killable.AddDeathListener(OnDead);
    }
}
