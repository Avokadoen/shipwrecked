using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OAEquipmentHUDClick : MonoBehaviour
{
    [SerializeField]
    TMPro.TextMeshProUGUI text;
    public string KeyAsString { set => text.text = value; }

    private int equipmentId = -1;
    public int EquipmentId { get => equipmentId; set => equipmentId = value; }

    private OAPlayerEquipment equipment;
    public OAPlayerEquipment Equipment { get => equipment; set => equipment = value; }

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
