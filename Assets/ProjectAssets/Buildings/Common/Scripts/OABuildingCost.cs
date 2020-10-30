using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OABuildingCost : MonoBehaviour
{
    [Tooltip("The cost of building this structure")]
    [SerializeField]
    private OAResource cost;
    public OAResource Cost { get => cost; }
}
