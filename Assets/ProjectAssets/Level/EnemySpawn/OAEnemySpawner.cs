using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


// Currently we only have one enemy so we keep this simpler than it should be ...
// TODO: use pooling instead of destroying and creating new enemies
public class OAEnemySpawner : MonoBehaviour
{
    private struct EnemyReference
    {
        public OAKillable killableRef;
        public OADespawner despawnRef;
        public OADeathActivater deathActivaterRef;
    }

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

    [Tooltip("How long the spawner should wait between each enemy \nX is the lowest possible value (Inclusive) \nY is the highest possible value (Inclusive)")]
    [SerializeField]
    Vector2 spawnIntervalRndRange = new Vector2(0.1f, 2f);

    UnityEvent onEradicated = new UnityEvent();
    public UnityEvent OnEradicated { get => onEradicated; }

    int waveCount = 0;

    int tideCycleCount = 1;

    List<EnemyReference> spawned = new List<EnemyReference>();
    List<EnemyReference> prevSpawned = new List<EnemyReference>();

    // Start is called before the first frame update
    void Start()
    {
        tides.OnLowTide.AddListener(() => StartCoroutine(OnLowTideBegin()));
        tides.OnHighTide.AddListener(() => StopAllCoroutines());

        if (!playerState)
            playerState = GameObject.FindWithTag("Player").GetComponent<OAPlayerStateStore>();
    }

    IEnumerator OnLowTideBegin()
    {
        waveCount = 0;

        foreach (var prototype in initialWave)
        {
            
            var spawnCount = (int) Mathf.Floor(prototype.initialSpawnCount * tideCycleCount * spawnRateScale);
            waveCount += spawnCount;
            for (int i = 0; i < spawnCount; i++)
            {
                if (prevSpawned.Count > 0)
                {
                    prevSpawned[0].killableRef.Revive();
                    prevSpawned[0].deathActivaterRef.OnRespawn();
                    prevSpawned[0].killableRef.transform.position = transform.position;

                    MoveItem(prevSpawned, spawned, 0, true);
                } else
                {
                    var killable = Instantiate(prototype.prefabKillable);
                    killable.transform.position = transform.position;
                    killable.transform.parent = transform;

                    tides.OnHighTide.AddListener(killable.Kill);
                    killable.OnDeathSelf.AddListener(OnDead);

                    var sensor = killable.GetComponent<OAEnemySensors>();
                    sensor.PlayerState = playerState;

                    var despawner = killable.GetComponent<OADespawner>();
                    despawner.OnDespawnSelf.AddListener(OnDespawn);

                    var deathActivater = killable.GetComponent<OADeathActivater>();

                    EnemyReference enemy;
                    enemy.killableRef = killable;
                    enemy.despawnRef = despawner;
                    enemy.deathActivaterRef = deathActivater;

                    spawned.Add(enemy);
                }
                float waitTime = Random.Range(spawnIntervalRndRange.x, spawnIntervalRndRange.y);
                yield return new WaitForSeconds(waitTime);
            }
        }

        tideCycleCount += 1;
    }


    void OnDead(OAKillable dead)
    {
        waveCount -= 1;

        if (waveCount == 0)
        {
            onEradicated.Invoke();
        }
    }

    void OnDespawn(OADespawner despawner)
    {
        var index = spawned.FindIndex(e => e.despawnRef == despawner);
        MoveItem(spawned, prevSpawned, index, false);
    }


    // TODO: move this to OAExtensions
    private void MoveItem(List<EnemyReference> from, List<EnemyReference> to, int index, bool active)
    {
        var pickup = from[index];
        pickup.killableRef.gameObject.SetActive(active);
        to.Add(pickup);
        from.SwapRemoveAt(index);
    }
}
