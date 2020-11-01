using UnityEngine;

[CreateAssetMenu(fileName = "Enemy pool prototype", menuName = "OceanAlien/EnemyPoolPrototype")]
public class OAEnemyPoolPrototype : ScriptableObject
{
    public OAEnemySensors prefabSensor;
    public OAKillable prefabKillable;
    public int initialSpawnCount;
    public float spawnScaleRate;
}
