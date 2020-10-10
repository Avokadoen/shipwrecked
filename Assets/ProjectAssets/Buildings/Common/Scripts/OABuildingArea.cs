using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO: Register event when user holds build button
// TODO: Change text based on bound key to built button
public class OABuildingArea : MonoBehaviour
{
    [Tooltip("Child object that holds canvas for text")]
    [SerializeField]
    private GameObject textObject = null;

    [Tooltip("How long the player needs to hold Build button")]
    [SerializeField]
    private float timeToBuild = 0.0f;

    [Tooltip("Buildings you can build from this area\n0 is the default, 1 is the first ...")]
    [SerializeField]
    private List<GameObject> buildings;
    private int currentBuilding = 0;
    private bool CanBuild {
        get { return currentBuilding < buildings.Count - 1; }
    }

    private float buildButtonHeldDuration;

    // Start is called before the first frame update
    void Start()
    {
        OAExtentions.AssertObjectNotNull(textObject, "OABuildingArea missing textObject");
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!CanBuild)
            return;

        textObject.SetActive(true);
    }

    void OnTriggerStay2D(Collider2D col)
    {
        if (!CanBuild)
            return;

        if (Input.GetAxis("Build") > 0)
            buildButtonHeldDuration += Time.deltaTime;

        if (buildButtonHeldDuration >= timeToBuild)
        {
            // TODO: check and drain resources at this stage
            // TODO: move all enemies outside of collider
            buildings[currentBuilding].SetActive(false);
            currentBuilding += 1;
            buildings[currentBuilding].SetActive(true);

            if (!CanBuild)
                textObject.SetActive(false);
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        textObject.SetActive(false);
    }
}
