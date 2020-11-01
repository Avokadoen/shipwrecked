using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Used to place
public class OAPickupableManualPoint: MonoBehaviour, IOAResourceMaster
{
    [SerializeField]
    private OAPlayerInventory inventory;

    [Tooltip("All manually placed pickupables")]
    [SerializeField]
    private List<OAPickupable> children;

    // Start is called before the first frame update
    void Start()
    {
        if (!inventory)
        {
            inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<OAPlayerInventory>();
        }

        foreach (var child in children)
        {
            child.Master = this;
        }
    }

    public void OnPickup(OAPickupable pickup)
    {
        inventory.OnPickup(pickup.Value);

        Destroy(pickup.gameObject);

        if (children.Count == 0)
            Destroy(gameObject);
    }
}
