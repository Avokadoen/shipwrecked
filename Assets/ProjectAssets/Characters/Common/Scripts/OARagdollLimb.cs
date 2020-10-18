using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(HingeJoint2D))]
public class OARagdollLimb : MonoBehaviour
{
    [SerializeField]
    Rigidbody2D rigid;

    [SerializeField]
    HingeJoint2D joint;

    void Start()
    {
        if (!rigid)
        {
            // We warn about this because this component will be on A LOT of objects in the game. 
            // Doing this implictly is fine in smaller scale, but for this component we warn
            Debug.LogWarning($"{gameObject.name} OARagdollLimb is missing Rigidbody2D, assigning automatically");
            rigid = GetComponent<Rigidbody2D>();
        }

        if (!joint)
        {
            Debug.LogWarning($"{gameObject.name} OARagdollLimb is missing HingeJoint2D, assigning automatically");
            joint = GetComponent<HingeJoint2D>();
        }

        rigid.simulated = false; // basically disable it
        joint.enabled = false;
    }

    // irreversible event where character will start to ragdoll
    public void OnRagdoll()
    {
        rigid.simulated = true; 
        joint.enabled = true;
    }
}
