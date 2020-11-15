using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OAFleeingFish : MonoBehaviour
{

    [SerializeField]
    Vector2 endPoint;

    [Tooltip("how long fish should take to reach end point in seconds")]
    [SerializeField]
    float totalFleeTime = 4f;
    float currentFleeTime = 0f;

    [Tooltip("how long fish should wait before fleeing in seconds")]
    [SerializeField]
    float fleeDelay = 2f;
    float seeTime = 0f;

    Vector2 startPoint;

    bool isFleeing = false;

    private void Start()
    {
        startPoint = transform.position;
    }

    private void FixedUpdate()
    {
        if (!isFleeing)
            return;

        if (seeTime < fleeDelay)
        {
            seeTime += Time.fixedDeltaTime;
            return;
        }

        transform.position = Vector2.Lerp(startPoint, endPoint, currentFleeTime / totalFleeTime);

        if (currentFleeTime > totalFleeTime)
        {
            Destroy(gameObject);

            isFleeing = false;

            return;
        }

        currentFleeTime += Time.fixedDeltaTime;
    }

    void OnBecameVisible()
    {
        // TODO: we could also just check the distance to the player
        isFleeing = true;
    }
}
