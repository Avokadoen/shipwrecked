using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

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
    List<Button> EquipmentButtons = new List<Button>();

    public void RegisterEquipmentHudElements(in List<OAEquipment> equipments)
    {
        var playerEquipment = GameObject.FindGameObjectWithTag("Player").GetComponent<OAPlayerEquipment>();

        for(var i = 0; i < equipments.Count; i++)
        {
            var hudObject = Instantiate(equipments[i].HudPrefab, transform);
            hudObject.EquipmentId = i;
            hudObject.Equipment = playerEquipment;
            hudObject.KeyAsString = (i + 1).ToString();

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

            EquipmentButtons.Add(objectRectTransform.GetComponent<Button>());
        }
    }

    private void Update()
    {
        // TODO: we migth not need this many options
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            OnKeyboardButtonShortcut(0);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            OnKeyboardButtonShortcut(1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            OnKeyboardButtonShortcut(2);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            OnKeyboardButtonShortcut(3);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            OnKeyboardButtonShortcut(4);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            OnKeyboardButtonShortcut(5);
        }
    }

    public void SetActiveEquipmentHud(bool active)
    {
        foreach(var eqHud in equipmentHud)
        {
            eqHud.gameObject.SetActive(active);
        }
    }

    private void OnKeyboardButtonShortcut(int index)
    {
        if (index >= EquipmentButtons.Count || index < 0)
        {
            return;
        }

        EquipmentButtons[index].onClick.Invoke();
        EventSystem.current.SetSelectedGameObject(EquipmentButtons[index].gameObject, null);
    }
}
