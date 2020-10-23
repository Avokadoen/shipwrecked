using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Script to animate a tide cycle. 
/// This script is also capable of calling functions when low/high- tide begins
/// </summary>
public class OATideAnimator : MonoBehaviour
{
    [Tooltip("How the water will move towards the lowTide")]
    [SerializeField]
    private AnimationCurve tideCycle;

    // TODO: find a more user friendly way of defining this
    [Tooltip("When the animation is above this value, the ocean is in high tide")]
    [SerializeField]
    private float lowTideThreshold = 0.37f;

    [Tooltip("How low the water will be at its lowest")]
    [SerializeField]
    [Min(0.1f)]
    private float lowTideDecline = 10f;

    [Tooltip("How long a cycle takes in total in seconds")]
    [SerializeField]
    private float cycleDuration = 180;

    [Tooltip("Delegate of low tide event")]
    [SerializeField]
    UnityEvent onLowTide;

    [Tooltip("Delegate of high tide event")]
    [SerializeField]
    UnityEvent onHighTide;

    private float cycleDurationPos;
    private float cycleDir = 1;
    private bool wasUnderLowTideThresholdPrevious = false;
    private Vector3 startPos;
    private Vector3 updatePos;

    // Start is called before the first frame update
    void Start()
    {
        // Negative values does not really make sense here, so we abs the value
        lowTideDecline = Mathf.Abs(lowTideDecline);


        cycleDurationPos = 0;
        startPos = transform.position;
        startPos.y -= lowTideDecline;
        transform.position = startPos;
        
    }

    public void AddLowTideListener(UnityAction call)
    {
        onLowTide.AddListener(call);
    }

    public void AddHighTideListener(UnityAction call)
    {
        onHighTide.AddListener(call);
    }

    void Update()
    {
        float tideState = tideCycle.Evaluate(cycleDurationPos / cycleDuration);
        
        updatePos = startPos;
        updatePos.y = startPos.y + (lowTideDecline * tideState);
        transform.position = updatePos;

        // TODO: maybe refactor this bool hell?
        // Announce tide state if it changes one way or another
        bool isUnderThreshold = tideState <= lowTideThreshold;
        if (isUnderThreshold && !wasUnderLowTideThresholdPrevious)
        {
            onLowTide.Invoke();
        }
        else if (!isUnderThreshold && wasUnderLowTideThresholdPrevious)
        {
            onHighTide.Invoke();
        }
        wasUnderLowTideThresholdPrevious = isUnderThreshold;

        // If we are moving from low -> high tide or vice versa
        if (cycleDurationPos >= cycleDuration)
            cycleDir = -1;
        else if (cycleDurationPos <= 0)
            cycleDir = 1;

        cycleDurationPos += (Time.deltaTime * cycleDir);
    }
}
