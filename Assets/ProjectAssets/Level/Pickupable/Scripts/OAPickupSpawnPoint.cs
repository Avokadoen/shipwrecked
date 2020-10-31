using System.Collections.Generic;
using UnityEngine;

// Only support BoxCollider2D for now
[RequireComponent(typeof(BoxCollider2D))]
public class OAPickupSpawnPoint : MonoBehaviour
{
    // TODO: probability distribution 
    [SerializeField]
    private OAPickupableSpawnConfig config;

    [SerializeField]
    private OATideAnimator tide;

    [SerializeField]
    private OAPlayerInventory inventory;

    // store all spawned elements
    private List<OAPickupable> spawned = new List<OAPickupable>();
    private List<OAPickupable> prevSpawned = new List<OAPickupable>();

    private BoxCollider2D spawnArea;

    private uint tideCount = 0; 

    void Start()
    {
        if (!inventory)
        {
            inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<OAPlayerInventory>();
        }

        spawnArea = GetComponent<BoxCollider2D>();

        tide.AddHighTideListener(OnHighTide);
        tide.AddLowTideListener(DespawnAll);
    }

    public void OnHighTide()
    {
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
