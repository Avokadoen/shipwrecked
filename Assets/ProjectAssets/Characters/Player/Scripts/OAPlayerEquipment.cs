using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(OAPlayerStateStore))]
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

    private OAPlayerStateStore stateStore;

    [Tooltip("Set the start equipment, -1 means no equipment")]
    [SerializeField] 
    private int equiptIndex = -1;
    private bool IsValidEquipt
    {
        get => equiptIndex >= 0 && equiptIndex < equippables.Count;
    }

    public void SetEquipmentIndex(int index)
    {
        // If player is under water we don't allow setting the index
        int isUnderWater = System.Convert.ToInt32(stateStore.IsUnderWater);
        index = index - ((index + 1) * isUnderWater);

        if (IsValidEquipt)
            equippables[equiptIndex].gameObject.SetActive(false);

        equiptIndex = index;

        if (IsValidEquipt)
        {
            equippables[equiptIndex].gameObject.SetActive(true);
            equipmentParticles.SetActive(true);
            headParticles.SetActive(true);
        }
        else
        {
            equipmentParticles.SetActive(false);
            headParticles.SetActive(false);
        }
    }
    void Awake()
    {
        OAExtentions.AssertObjectNotNull(hudPrefab, "Player missing HUD object");

        hudInstance = Instantiate(hudPrefab);

        stateStore = GetComponent<OAPlayerStateStore>();
    }

    void Start()
    {
        // TODO: validate that variables are set
        foreach (var equipment in equippables)
        {
            Instantiate(equipment.HudPrefab, hudInstance.transform);
            equipment.gameObject.SetActive(false);
        }

        SetEquipmentIndex(equiptIndex);
    }

    void Update()
    {
        // TODO: we migth not need this many
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SetEquipmentIndex(0);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SetEquipmentIndex(1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SetEquipmentIndex(2);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            SetEquipmentIndex(3);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            SetEquipmentIndex(4);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            SetEquipmentIndex(5);
        }
    }

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

        // TODO: use quaternion instead ..
        float angl = Vector3.Angle(Vector3.right, equipDir);
        angl *= (equipDir.y < 0) ? -1 : 1;
        equipt.transform.rotation = Quaternion.AngleAxis(angl, Vector3.forward);

        Vector3 calcPos = pivotPoint.position + equipDir * equipt.Distance;
        equipt.transform.position = calcPos;
        equipmentParticles.transform.position = calcPos;

        // TODO: flip y on sprite renderer of equip when we have common component for that.
        if (angl > 89f || angl < -90f)
            equipt.SetFlipY(true);
        else
            equipt.SetFlipY(false);
    }
}
