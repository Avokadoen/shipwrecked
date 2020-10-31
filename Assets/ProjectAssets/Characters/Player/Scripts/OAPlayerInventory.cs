using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// Not to be confused with OAPlayerEquipment
public class OAPlayerInventory : MonoBehaviour
{
    [SerializeField]
    private UnityEvent onPickup;

    Dictionary<OAResource.Type, uint> resources = new Dictionary<OAResource.Type, uint>(){
        {OAResource.Type.ShinyScale, 0},
        {OAResource.Type.BlueScale, 0},
        {OAResource.Type.Spike, 0},
    };

    void Awake()
    {
#if UNITY_EDITOR
        var targetCount = System.Enum.GetValues(typeof(OAResource.Type)).Length - 1;
        if (resources.Count != targetCount)
            Debug.LogError("OAPlayerInventory initialized resources with wrong count");
#endif
    }

    public bool CanWithdraw(OAResource resource)
    {
        return resources[resource.InstanceType] >= resource.Amount;
    }


    public void OnPickup(OAResource resource)
    {
        onPickup.Invoke();
        resources[resource.InstanceType] += resource.Amount;
    }

    /// <summary>
    /// Attempts to withdraw resources from the player
    /// </summary>
    /// <param name="resource">type and amount to withdraw</param>
    /// <returns>True on success, false otherwise</returns>
    public bool OnWithdrawResource(OAResource resource) 
    {
        if (!CanWithdraw(resource))
            return false;

        resources[resource.InstanceType] -= resource.Amount;
        return true;
    }
}
