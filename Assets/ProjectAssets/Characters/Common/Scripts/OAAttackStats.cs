using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// used by equipment and animals
[CreateAssetMenu(fileName = "AttackStats", menuName = "OceanAlien/AttackStats", order = 4)]
public class OAAttackStats : ScriptableObject
{
    public float range;
    public int damage;
    // How many seconds it takes to do one attack
    public float attackSpeed;
}
