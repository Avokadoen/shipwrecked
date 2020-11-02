using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(OAKillable), typeof(OADespawner))]
public class OADeathActivater : MonoBehaviour
{
    [Tooltip("Components that should be disabled on death")]
    [SerializeField]
    List<Behaviour> disableComponents;

    [Tooltip("All limbs that should be simulated on death")]
    [SerializeField]
    List<OARagdollLimb> limbs;

    [SerializeField]
    OAKillable killable;

    [SerializeField]
    OADespawner despawner;

    public float fadeStepDelay = .1f;
    public float fadeStride = .02f;

    // Start is called before the first frame update
    void Start()
    {
        if (!killable)
            killable = GetComponent<OAKillable>();

        if (!despawner)
            despawner = GetComponent<OADespawner>();

        killable.OnDeath.AddListener(OnDeath);
    }

    void OnDeath()
    {
        foreach (var component in disableComponents)
        {
            component.enabled = false;
        }

        foreach (var limb in limbs)
        {
            limb.OnRagdoll();
        }

        despawner.Despawn();
    }

    [ContextMenu("Debug: necromancer?")]
    public void OnRespawn()
    {
        foreach (var component in disableComponents)
        {
            component.enabled = true;
        }

        foreach (var limb in limbs)
        {
            limb.OnRigid();
        }
    }
}
