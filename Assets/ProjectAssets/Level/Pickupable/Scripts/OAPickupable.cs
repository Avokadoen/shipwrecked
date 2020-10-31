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

    private IOAResourceMaster master;
    public IOAResourceMaster Master { set => master = value; }

    void OnTriggerEnter2D(Collider2D col)
    {
        master.OnPickup(this);
    }
}
