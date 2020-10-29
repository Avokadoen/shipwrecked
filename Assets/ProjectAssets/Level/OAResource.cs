using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OAResource : MonoBehaviour
{
    public enum Type
    {
        Unset,
        Wood,
        Scale
    }


    [SerializeField]
    [Min(1)]
    uint amount = 1;
    public uint Amount { get => amount; }

    [SerializeField]
    Type instanceType = Type.Unset;
    public Type InstanceType { get => instanceType; }

    // Start is called before the first frame update
    void Start()
    {
        if (instanceType == Type.Unset)
        {
            Debug.LogError("OAResource has instanceType of Unset");
        }
    }
}
