using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


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

    [Tooltip("Player state, used to supply enemies with reference")]
    [SerializeField]
    OAPlayerStateStore playerState;

    [Tooltip("Tide animator that animates the scene ocean")]
    [SerializeField]
    OATideAnimator tides;

    UnityEvent onEradicated = new UnityEvent();
    public UnityEvent OnEradicated { get => onEradicated; }

    int waveCount = 0;

    int tideCycleCount = 1;

    // Start is called before the first frame update
    void Start()
    {
        tides.OnLowTide.AddListener(OnLowTideBegin);

        if (!playerState)
            playerState = GameObject.FindWithTag("Player").GetComponent<OAPlayerStateStore>();
    }

    void OnLowTideBegin()
    {
        waveCount = 0;

        foreach (var prototype in initialWave)
        {
            waveCount += 1;
            var spawnCount = Mathf.Floor(prototype.initialSpawnCount * tideCycleCount * spawnRateScale);
            for (int i = 0; i < spawnCount; i++)
            {
                var killable = Instantiate(prototype.prefabKillable);
                killable.transform.position = transform.position;

                tides.OnHighTide.AddListener(killable.Kill);
                killable.OnDeath.AddListener(OnDead);

                var sensor = killable.GetComponent<OAEnemySensors>();
                sensor.PlayerState = playerState;
            }
        }

        tideCycleCount += 1;
    }


    void OnDead()
    {
        waveCount -= 1;

        if (waveCount == 0)
        {
            onEradicated.Invoke();
        }
    }
}
