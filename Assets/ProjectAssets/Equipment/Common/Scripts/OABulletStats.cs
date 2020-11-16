using UnityEngine;

[CreateAssetMenu(fileName = "Bullet", menuName = "OceanAlien/Bullet", order = 3)]
public class OABulletStats : ScriptableObject
{
    [Tooltip("Speed of the bullet")]
    public float speed;

    [Tooltip("Damage to apply on hit")]
    public int baseDamage;

    [Tooltip("Max range of the bullet")]
    public float maxRange;

    [Tooltip("Name of the bullet")]
    public string identifier;
}
