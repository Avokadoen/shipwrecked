using System.Collections.Generic;
using UnityEngine;

// Only support BoxCollider2D for now
[RequireComponent(typeof(BoxCollider2D))]
public class OAPickupSpawnPoint : MonoBehaviour, IOAResourceMaster
{
    // TODO: make tide emit hightide on wave clear to avoid having this type of logic all over the place ...
    private enum PersonalTideState
    {
        Unset,
        HighTide,
        LowTide
    }

    // TODO: probability distribution 
    [SerializeField]
    private OAPickupableSpawnConfig config;

    [SerializeField]
    private OATideAnimator tide;

    [SerializeField]
    private OAEnemySpawner spawner;

    [SerializeField]
    private OAPlayerInventory inventory;

    [Tooltip("Will make it spawn when Start is called")]
    [SerializeField]
    private bool spawnOnStart = false;

    // store all spawned elements
    private List<OAPickupable> spawned = new List<OAPickupable>();
    private List<OAPickupable> prevSpawned = new List<OAPickupable>();

    private BoxCollider2D spawnArea;

    private uint tideCount = 0;
    private PersonalTideState state = PersonalTideState.Unset;

    void Start()
    {
        if (!inventory)
        {
            inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<OAPlayerInventory>();
        }

        spawnArea = GetComponent<BoxCollider2D>();

        spawner.OnEradicated.AddListener(OnHighTide);
        tide.OnHighTide.AddListener(OnHighTide);
        tide.OnLowTide.AddListener(DespawnAll);

        if (spawnOnStart)
            OnHighTide();
    }

    public void OnHighTide()
    {
        if (state == PersonalTideState.HighTide)
            return;

        state = PersonalTideState.HighTide;
        tideCount += 1;
        var spawnCount = config.initialSpawnCount + (config.spawnScaling * tideCount);
        SpawnStuff((int) Mathf.Floor(spawnCount));
    }

    public void SpawnStuff(int amount)
    {
        for (var i = 0; i < amount; i++)
        {
            if (prevSpawned.Count > 0)
            {
                // Reuse previous spawn
                MoveItem(prevSpawned, spawned, prevSpawned.Count - 1, true);
            } else
            {
                var typeIndex = Random.Range(0, config.spawnable.Count);
                var pickup = Instantiate(config.spawnable[typeIndex]);
                var center = spawnArea.bounds.center;

                var minX = center.x - spawnArea.size.x * 0.5f;
                var minY = center.y - spawnArea.size.y * 0.5f;
                var maxX = center.x + spawnArea.size.x * 0.5f;
                var maxY = center.y + spawnArea.size.y * 0.5f;

                var x = Random.Range(minX, maxX);
                var y = Random.Range(minY, maxY);

                pickup.transform.position = new Vector2(x, y);
                pickup.transform.parent = transform;

                pickup.Master = this; 

                spawned.Add(pickup);
            }
        }
    }

    public void OnPickup(OAPickupable pickup)
    {
        inventory.OnPickup(pickup.Value);
        var index = spawned.IndexOf(pickup);
        MoveItem(spawned, prevSpawned, index, false);
    }

    // TODO: convert to coroutine to do this gradually
    // TODO: animation for this (Playe pop sound, visual cue ...)
    public void DespawnAll()
    {
        state = PersonalTideState.LowTide;
        for (var i = spawned.Count - 1; i >= 0; i--)
        {
            MoveItem(spawned, prevSpawned, i, false);
        }
    }


    private void MoveItem(List<OAPickupable> from, List<OAPickupable> to, int index, bool active)
    {
        var pickup = from[index];
        pickup.gameObject.SetActive(active);
        to.Add(pickup);
        from.SwapRemoveAt(index);
    }
}
