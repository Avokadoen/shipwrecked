using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// TODO: Change text based on bound key to built button
public class OABuildingArea : MonoBehaviour
{
    [Tooltip("When the building area gets a new (better) building")]
    [SerializeField]
    private UnityEvent onBuilt;
    public UnityEvent OnBuilt { get => onBuilt; }

    [Tooltip("When the building area gets destroyed one tier")]
    [SerializeField]
    private UnityEvent onDestroyed;

    [Tooltip("Child object that holds canvas for text")]
    [SerializeField]
    private GameObject textCanvas = null;
    private TMPro.TextMeshProUGUI buildText;
    private TMPro.TextMeshProUGUI resourceText;

    [Tooltip("How long the player needs to hold Build button")]
    [SerializeField]
    private float timeToBuild = 0.0f;

    [Tooltip("Buildings you can build from this area\n0 is the default, 1 is the first ...")]
    [SerializeField]
    private List<OABuildingCost> buildings;

    private OAPlayerInventory inventory;

    private int currentBuilding = 0;
    private bool CanBuild {
        get {
            // remove the base building from count
            bool isMaxed = currentBuilding >= buildings.Count - 1;
            return !isMaxed && inventory.CanWithdraw(buildings[currentBuilding + 1].Cost);
        }
    }

    private readonly string BuildInstructions = "BuildInstructions";
    private readonly string ResourceInformation = "ResourceInformation";

    private float buildButtonHeldDuration;

    // Start is called before the first frame update
    void Awake()
    {
        OAExtentions.AssertObjectNotNull(textCanvas, "OABuildingArea missing textObject");
        inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<OAPlayerInventory>();

        buildText = textCanvas.transform
            .Find(BuildInstructions)
            .GetComponent<TMPro.TextMeshProUGUI>();

        resourceText = textCanvas.transform
            .Find(ResourceInformation)
            .gameObject
            .GetComponent<TMPro.TextMeshProUGUI>();


        buildText.gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        resourceText.gameObject.SetActive(UpdateResourceText());

        if (!CanBuild)
            return;

        buildText.gameObject.SetActive(true);
    }

    public void OnBuildingTierDestroyed()
    {
        if (currentBuilding > 1)
            return;

        buildings[currentBuilding].gameObject.SetActive(false);
        currentBuilding -= 1;
        buildings[currentBuilding].gameObject.SetActive(true);

        UpdateResourceText();

        onDestroyed.Invoke();
    }

    void OnTriggerStay2D(Collider2D col)
    {
        if (!CanBuild)
            return;

        if (Input.GetAxis("Build") > 0)
        {
            buildButtonHeldDuration += Time.deltaTime;
        } else
        {
            buildButtonHeldDuration = 0;
        }

        if (buildButtonHeldDuration >= timeToBuild)
        {
            bool didWithdraw = inventory.OnWithdrawResource(buildings[currentBuilding + 1].Cost);

            System.Action<int> revive = (index) =>
            {
                if (buildings[currentBuilding].Killable)
                {
                    buildings[currentBuilding].Killable.Revive();
                }
            };

            revive(currentBuilding); // heal previous tier if it is killable

            buildings[currentBuilding].gameObject.SetActive(false);
            currentBuilding += 1;
            buildings[currentBuilding].gameObject.SetActive(true);
            buildButtonHeldDuration = 0;

            revive(currentBuilding); // heal new tier if it is killable

            resourceText.gameObject.SetActive(UpdateResourceText());

            onBuilt.Invoke();

            buildText.gameObject.SetActive(CanBuild);
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        buildText.gameObject.SetActive(false);
        resourceText.gameObject.SetActive(false);

        buildButtonHeldDuration = 0;
    }

    private bool UpdateResourceText()
    {
        if (currentBuilding >= buildings.Count - 1)
            return false;

        // set resource information text
        var cost = buildings[currentBuilding + 1].Cost;
        var resourceType = OAResource.TypeToString(cost.InstanceType);
        var amount = cost.Amount;
        var holdingAmount = inventory.GetAmount(cost.InstanceType);
        resourceText.text = $"{holdingAmount}/{amount} {resourceType}s";

        return true;
    }
}
