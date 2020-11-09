using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OAPointOfInterest : MonoBehaviour
{
    public enum PoiId
    {
        Boat,
        Cave,
        Sand,
        Mountain,
        BlueShellTut
    }

    [SerializeField]
    OASpaceShipSensor sensor;

    [SerializeField]
    PoiId poiId;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        sensor.OnTriggerEnterPOI(poiId);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        sensor.OnTriggerExitPOI(poiId);
    }
}
