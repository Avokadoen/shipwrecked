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
    [Tooltip("When the entity recieves damage")]
    [SerializeField]
    private UnityEvent onHurt;

    [Tooltip("When the entity recieves damage")]
    [SerializeField]
    private UnityEvent<int> onHurtHealth;
    public UnityEvent<int> OnHurtHealth { get => onHurtHealth; }

    [SerializeField]
    private OAHealth healthStats = null;

    [Tooltip("Health when killable is spawned, -1 means killable will spawn with max health")]
    [SerializeField]
    private int health = -1;
    public int Health { get => health; }

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

        if (onHurtHealth == null)
            onHurtHealth = new UnityEvent<int>();

        OAExtentions.AssertObjectNotNull(healthStats, "OAKillable missing healthStats");

        if (health <= 0)
        {
            health = healthStats.maxHealth;
        }
    }

    void ApplyDamage(int damage)
    {
        health -= damage;

        if (health + damage <= 0) // if we did not die in this call
        {
            onHurtHealth.Invoke(health);
            return;
        }

        if (health <= 0)
        {
            onDeath.Invoke();
            OnDeathSelf.Invoke(this);
        }
        else
        {
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

}
