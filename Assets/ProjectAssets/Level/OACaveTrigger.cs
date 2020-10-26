using UnityEngine;
using UnityEngine.Events;

public class OACaveTrigger : MonoBehaviour
{
    public enum TriggerType
    {
        Exit,
        Enter
    }

    UnityEvent<TriggerType> caveEvent;

    void Awake()
    {
        caveEvent = new UnityEvent<TriggerType>();
    }

    public void AddListener(UnityAction<TriggerType> call)
    {
        caveEvent.AddListener(call);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        caveEvent.Invoke(TriggerType.Enter);
    }

    void OnTriggerExit2D(Collider2D other)
    {
        caveEvent.Invoke(TriggerType.Exit);
    }
}
