using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OAParallaxEffect : MonoBehaviour
{
    enum Axis
    {
        X = 0,
        Y = 1
    }

    [Tooltip("Simulated distance of the object, greater distance leads to less scrolling")]
    [SerializeField]
    float distance = 0f;

    [Tooltip("Divisor that will reduce y axis parallaxing")]
    [SerializeField]
    float yReduce = 4f;

    Transform cTrans;

    Vector2 startPos;

    void Start()
    {
        cTrans = Camera.main.transform;

        startPos = transform.position;
    }

    void FixedUpdate()
    {
        float parallax(Axis axis)
        {
            var effectiveYReduce = axis == Axis.Y ? yReduce : 1;

            var index = (int) axis;
            var divisor = (1 + distance);
            var offset = ((startPos[index] - cTrans.position[index]) / divisor) / effectiveYReduce;
            return startPos[index] + offset;
        }

        transform.position = new Vector3(parallax(Axis.X), parallax(Axis.Y), 1);
    }

    private void OnBecameVisible()
    {
        enabled = true;
    }

    private void OnBecameInvisible()
    {
        enabled = false;
    }
}
