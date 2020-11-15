using UnityEngine;

[CreateAssetMenu(fileName = "Enemy pool prototype", menuName = "OceanAlien/EnemyPoolPrototype")]
public class OAEnemyPoolPrototype : ScriptableObject
{
    // TODO: remove OAKillable and instantiate using sensor
    public OAEnemySensors prefabSensor;
    public OAKillable prefabKillable;
    public int initialSpawnCount;
    public float spawnScaleRate;
}
