using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "OAPickupableSpawnConfig", menuName = "OceanAlien/OAPickupableSpawnConfig")]
public class OAPickupableSpawnConfig : ScriptableObject
{
    // TODO: Probability distribution
    [Tooltip("Pickupable prefabs that can spawn with this config")]
    public List<OAPickupable> spawnable;

    [Tooltip("How many will spawn initialy")]
    public float initialSpawnCount;

    [Tooltip("How much more will spawn for each wave")]
    public float spawnScaling;
}
