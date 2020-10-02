using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO: here we can put other relevant stats, i.e stamina
[CreateAssetMenu(fileName = "Health", menuName = "OceanAlien/Health", order = 2)]
public class OAHealth : ScriptableObject
{
    public int maxHealth;
}
