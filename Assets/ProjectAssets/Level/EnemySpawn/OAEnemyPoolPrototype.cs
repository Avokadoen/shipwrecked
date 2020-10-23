using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy pool prototype", menuName = "OceanAlien/EnemyPoolPrototype")]
public class OAEnemyPoolPrototype : ScriptableObject
{
    public OAEnemySensors prefab;
    public int initialSpawnCount;
    public float spawnScaleRate;
}
