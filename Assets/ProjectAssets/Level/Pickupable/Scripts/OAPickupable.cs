using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OAPickupable : MonoBehaviour
{
    [Tooltip("What the player gains from picking this up")]
    [SerializeField]
    private OAResource value;
    public OAResource Value { get => value; }

    private OAPickupSpawnPoint master;
    public OAPickupSpawnPoint Master { set => Master = value; }

    void OnTriggerEnter2D(Collider2D col)
    {
        master.OnPickup(this);
    }
}
