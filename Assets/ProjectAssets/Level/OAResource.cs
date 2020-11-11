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
        Spike
    }

    public static string TypeToString(Type rType)
    {
        switch (rType) {
            case Type.ShinyScale:
                return "shiny scale";
            case Type.BlueScale:
                return "blue scale";
            case Type.Spike:
                return "spike";
            default:
                return "ILLEGAL TYPE, YOU FOUND A BUG ;)";
        }
    }


    [SerializeField]
    [Min(1)]
    uint amount = 1;
    public uint Amount { get => amount; }

    [SerializeField]
    Type instanceType = Type.Unset;
    public Type InstanceType { get => instanceType; }
}
