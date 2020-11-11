using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OAEqupmentHUDClick : MonoBehaviour
{
    public int equipmentId = -1;

    private OAPlayerEquipment equipment;

    private void Start()
    {
        var button = GetComponent<Button>();
        button.onClick.AddListener(OnClick);
    }

    public void OnClick()
    {
        equipment.SetEquipmentIndex(equipmentId);
    }
}
