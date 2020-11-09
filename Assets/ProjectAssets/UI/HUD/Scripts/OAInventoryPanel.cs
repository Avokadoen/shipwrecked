using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OAInventoryPanel : MonoBehaviour
{
    // TODO: we hardcode this menu for now
    [SerializeField]
    Text scaleAmount;

    [SerializeField]
    Text blueShellAmount;

    [SerializeField]
    Text spikeAmount;

    Dictionary<OAResource.Type, Text> resources;

    void Start()
    {
        GameObject.FindGameObjectWithTag("Player")
            .GetComponent<OAPlayerInventory>()
            .OnResourceChange
            .AddListener(UpdateDisplay);

        resources = new Dictionary<OAResource.Type, Text>(){
            {OAResource.Type.ShinyScale, scaleAmount},
            {OAResource.Type.BlueScale, blueShellAmount},
            {OAResource.Type.Spike, spikeAmount},
        };
    }

    private void UpdateDisplay(OAResource.Type rType, uint amount)
    {
        resources[rType].text = $"{amount}";
    }
}
