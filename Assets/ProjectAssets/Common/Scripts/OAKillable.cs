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

    [Tooltip("When the entity recieves damage")]
    [SerializeField]
    private UnityEvent onHurt;
    public UnityEvent OnHurt { get => onHurt; }

    [Tooltip("When the entity recieves damage")]
    [SerializeField]
    private UnityEvent<int> onHurtHealth;
    public UnityEvent<int> OnHurtHealth { get => onHurtHealth; }

    [SerializeField]
    private UnityEvent onDeath;
    public UnityEvent OnDeath { get => onDeath; }

    [SerializeField]
    private UnityEvent<OAKillable> onDeathSelf;
    public UnityEvent<OAKillable> OnDeathSelf { get => onDeathSelf; }

    // Start is called before the first frame update
    void Awake()
    {
        if (onDeath == null)
            onDeath = new UnityEvent();

        if (onDeathSelf == null)
            onDeathSelf = new UnityEvent<OAKillable>();

        OAExtentions.AssertObjectNotNull(healthStats, "OAKillable missing healthStats");

        if (health <= 0)
        {
            health = healthStats.maxHealth;
        }
    }

    void ApplyDamage(int damage)
    {
        health -= damage;

        // if we are dead, but did not die in this call
        if (health + damage <= 0) 
        {
            return;
        }

        if (health <= 0)
        {
            onDeath.Invoke();
            OnDeathSelf.Invoke(this);
        }
        else
        {
            onHurtHealth.Invoke(health);
            onHurt.Invoke();
        }
    }

    public void Revive()
    {
        health = healthStats.maxHealth;
    }

    [ContextMenu("Debug: kill unit")]
    public void Kill()
    {
        if (health <= 0)
            return;

        ApplyDamage(healthStats.maxHealth);
    }


    [ContextMenu("Debug: damage unit")]
    public void DamageUnit()
    {
        ApplyDamage(1);
    }

}
