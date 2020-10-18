using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OATideAnimator : MonoBehaviour
{
    [Tooltip("How the water will move towards the lowTide")]
    [SerializeField]
    private AnimationCurve tideCycle;

    [Tooltip("How low the water will be at its lowest")]
    [SerializeField]
    private float lowTideDecline = 10f;

    [Tooltip("How long a cycle takes in total in seconds")]
    [SerializeField]
    private float cycleDuration = 180;

    private float cycleDurationPos;
    private float cycleDir = 1;
    private Vector3 startPos;
    private Vector3 updatePos;

    // Start is called before the first frame update
    void Start()
    {
        // Negative values does not really make sense here, so we abs the value
        lowTideDecline = Mathf.Abs(lowTideDecline);

        cycleDurationPos = 0;
        startPos = transform.position;
    }

    void FixedUpdate()
    {
        // We start with low-tide, so we do -1 
        float tideState = 1 - tideCycle.Evaluate(cycleDurationPos / cycleDuration);
        
        updatePos = startPos;
        updatePos.y = startPos.y - (lowTideDecline * tideState);
        transform.position = updatePos;

        // If we are moving from low -> high tide or vice versa
        if (cycleDurationPos >= cycleDuration)
            cycleDir = -1;
        else if (cycleDurationPos <= 0) 
            cycleDir = 1;

        cycleDurationPos += (Time.fixedDeltaTime * cycleDir);
    }
}
