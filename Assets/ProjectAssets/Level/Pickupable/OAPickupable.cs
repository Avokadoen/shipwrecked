using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OAPickupable : MonoBehaviour
{
    [Tooltip("What the player gains from picking this up")]
    [SerializeField]
    private OAResource value;

    [SerializeField]
    private UnityEvent onPickup;

    // TODO: if we spawn these, then the spawner can supply items with this reference
    private OAPlayerInventory inventory;

    void Start()
    {
        inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<OAPlayerInventory>();
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        onPickup.Invoke();
        inventory.OnPickup(value);

        // TODO: supply back to resource spawner instead
        Destroy(gameObject);
    }
}
