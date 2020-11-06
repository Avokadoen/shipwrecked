using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO: component to fade HUD when not in use
public class OAHUDEquipmentAssigner : MonoBehaviour
{
    [Tooltip("The position of the first item in the hud")]
    [SerializeField]
    RectTransform startSlot;

    [Tooltip("How much space there should be inbetween each entry in pixels")]
    [SerializeField]
    int slotStridePx = 10;

    List<RectTransform> equipmentHud = new List<RectTransform>();

    // TODO: start function

    public void RegisterEquipmentHudElements(in List<OAEquipment> equipments)
    {
        for(var i = 0; i < equipments.Count; i++)
        {
            var hudObject = Instantiate(equipments[i].HudPrefab, transform);

            // Copy rect transform values, didn't find a simpler way of doing it :(
            var objectRectTransform = hudObject.transform.GetComponent<RectTransform>();
            objectRectTransform.anchoredPosition = startSlot.anchoredPosition;
            objectRectTransform.position = startSlot.position;
            objectRectTransform.pivot = startSlot.pivot;
            objectRectTransform.localRotation = startSlot.localRotation;
            objectRectTransform.localPosition = startSlot.localPosition;
            objectRectTransform.anchorMax = startSlot.anchorMax;
            objectRectTransform.anchorMin = startSlot.anchorMin;
            objectRectTransform.offsetMax = startSlot.offsetMax;
            objectRectTransform.offsetMin = startSlot.offsetMin;
            Vector3 newPos = objectRectTransform.localPosition;
            newPos.x += slotStridePx * i;
            objectRectTransform.localPosition = newPos;

            equipmentHud.Add(objectRectTransform);
        }
    }

    public void SetActiveEquipmentHud(bool active)
    {
        foreach(var eqHud in equipmentHud)
        {
            eqHud.gameObject.SetActive(active);
        }
    }
}
