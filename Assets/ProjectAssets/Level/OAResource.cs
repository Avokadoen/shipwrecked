using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Resource", menuName = "OceanAlien/Resource")]
public class OAResource : ScriptableObject
{
    public enum Type
    {
        Unset,
        ShinyScale,
        BlueScale,
        Spike,
    }


    [SerializeField]
    [Min(1)]
    uint amount = 1;
    public uint Amount { get => amount; }

    [SerializeField]
    Type instanceType = Type.Unset;
    public Type InstanceType { get => instanceType; }
}
