using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// A wrapper for a health value
/// Usually when components want to work with health they either 
///     A) want to apply damage
///     B) know when something has died
/// This components enables other components to do either in a simple manner
/// </summary>
public class OAKillable : MonoBehaviour
{
    [SerializeField]
    private OAHealth healthStats = null;

    [Tooltip("Health when killable is spawned, -1 means killable will spawn with max health")]
    [SerializeField]
    private int health = -1;
    public int Health { get => health; }

    [SerializeField]
    private UnityEvent onDeath;

    // Start is called before the first frame update
    void Awake()
    {
        if (onDeath == null)
            onDeath = new UnityEvent();

        OAExtentions.AssertObjectNotNull(healthStats, "OAKillable missing healthStats");

        if (health <= 0)
        {
            health = healthStats.maxHealth;
        }
    }

    void ApplyDamage(int damage)
    {
        health -= damage;

        if (health <= 0)
        {
            onDeath.Invoke();
        }
    }

    public void AddDeathListener(UnityAction call)
    {
        onDeath.AddListener(call);
    }

    [ContextMenu("Debug: kill unit")]
    void DebugKill()
    {
        health = 0;
        onDeath.Invoke();
    }

}
