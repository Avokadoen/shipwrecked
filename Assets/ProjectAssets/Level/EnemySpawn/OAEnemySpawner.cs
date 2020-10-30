using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Currently we only have one enemy so we keep this simpler than it should be ...
// TODO: use pooling instead of destroying and creating new enemies
public class OAEnemySpawner : MonoBehaviour
{
    [Tooltip("How much the spawning should scale for each wave")]
    [SerializeField]
    float spawnRateScale = 1.2f;

    [Tooltip("Enemies that can spawn each frame")]
    [SerializeField]
    List<OAEnemyPoolPrototype> initialWave;

    [Tooltip("Tide animator that animates the scene ocean")]
    [SerializeField]
    OATideAnimator tides;

    // [Tooltip("The sequence enemies spawn in. Make sure the count of this list is the sum on all pool data")]
    // [SerializeField]
    // List<int> indexSpawnSequence;

    bool isLowTide = false;
    int tideCycleCount = 1;

    // Start is called before the first frame update
    void Start()
    {
        tides.AddLowTideListener(OnLowTideBegin);
        tides.AddHighTideListener(OnLowTideEnd);
    }

    private void Update()
    {
        if (!isLowTide)
            return;
    }

    void OnLowTideBegin()
    {
        isLowTide = true;

        foreach (var prototype in initialWave)
        {
            var spawnCount = Mathf.Floor(prototype.initialSpawnCount * tideCycleCount * spawnRateScale);
            for (int i = 0; i < spawnCount; i++)
            {
                var enemy = Instantiate(prototype.prefab);
                enemy.transform.position = transform.position;
            }
        }

        tideCycleCount += 1;
    }

    void OnLowTideEnd()
    {
        isLowTide = true;
        // TODO: each enemy should despawn here
        //       Probably do this decentralized (Each rat listens)
    }
}
