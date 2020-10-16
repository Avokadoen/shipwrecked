using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class OAPlayerEquipment : MonoBehaviour
{
    // TODO: hold a special equipment component instead
    [Tooltip("All equipment that the player can use currently")]
    [SerializeField]
    List<OAEquipment> equippables = new List<OAEquipment>();

    [Tooltip("Point we rotate equipt item around")]
    [SerializeField]
    Transform pivotPoint;

    [Tooltip("Particles that follow equipment")]
    [SerializeField]
    GameObject equipmentParticles;

    [Tooltip("Particles that follow head")]
    [SerializeField]
    GameObject headParticles;

    [Tooltip("HUD prefab")]
    [SerializeField]
    GameObject hudPrefab;

    GameObject hudInstance;

    [Tooltip("Set the start equipment, -1 means no equipment")]
    [SerializeField] 
    private int equiptIndex = -1;
    private bool IsValidEquipt
    {
        get => equiptIndex >= 0 && equiptIndex < equippables.Count;
    }

    public void SetEquipmentIndex(int index)
    {
        if (IsValidEquipt)
            equippables[equiptIndex].gameObject.SetActive(false);

        equiptIndex = index;

        if (IsValidEquipt)
        {
            equippables[equiptIndex].gameObject.SetActive(true);
            equipmentParticles.SetActive(true);
            headParticles.SetActive(true);
        } else
        {
            equipmentParticles.SetActive(false);
            headParticles.SetActive(false);
        }
    }

    void Awake()
    {
        OAExtentions.AssertObjectNotNull(hudPrefab, "Player missing HUD object");

        hudInstance = Instantiate(hudPrefab);
    }

    void Start()
    {
        // TODO: validate that variables are set
        foreach (var equipment in equippables)
        {
            equipment.gameObject.SetActive(false);
            Instantiate(equipment.HudPrefab, hudInstance.transform);
        }

        SetEquipmentIndex(equiptIndex);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // If the player is not using a valid index, we don't compute.
        if (!IsValidEquipt)
            return;

        var equipt = equippables[equiptIndex];

        // Get mouse position in world pos
        // TODO: garbage is created here ...
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Vector3 equipDir = new Vector3(ray.origin.x - pivotPoint.position.x, ray.origin.y - pivotPoint.position.y);
        equipDir.Normalize();
        
        Vector3 calcPos = pivotPoint.position + equipDir * equipt.Distance;
        equipt.transform.position = calcPos;
        equipmentParticles.transform.position = calcPos;

        // TODO: use quaternion instead ..
        float angl = Vector3.Angle(Vector3.right, equipDir);
        angl *= (equipDir.y < 0) ? -1 : 1;
        equipt.transform.rotation = Quaternion.AngleAxis(angl, Vector3.forward);

        // TODO: flip y on sprite renderer of equip when we have common component for that.
        if (angl > 89f || angl < -90f)
            equipt.SetFlipY(true);
        else
            equipt.SetFlipY(false);
    }
}
