using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OAEquipment : MonoBehaviour
{
    [Tooltip("This is the object that will represent the equipment in the hud")]
    [SerializeField]
    Button hudPrefab;
    public GameObject HudPrefab
    {
        get => hudPrefab.gameObject;
    }

    [Tooltip("Sprite renderer that will be flipped it equipment reaches >90* rotation")]
    [SerializeField]
    SpriteRenderer sr;

    [Tooltip("Distance between player ")]
    [SerializeField]
    float distance = 2;
    public float Distance
    {
        get => distance;
    }

    // Start is called before the first frame update
    void Start()
    {
        OAExtentions.AssertObjectNotNull(hudPrefab, "Equipment missing hud icon");
        OAExtentions.AssertObjectNotNull(sr, "Equipment missing sprite renderer");
    }

    public void SetFlipY(bool flipY)
    {
        // TODO: if not sr not null?
        sr.flipY = flipY;
    }
}
