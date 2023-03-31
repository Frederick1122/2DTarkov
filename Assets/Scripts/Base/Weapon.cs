
using UnityEngine;

[CreateAssetMenu(fileName = "NewWeapon", menuName = "Item/Weapon")]
public class Weapon : Item
{
    [Space]
    public WeaponPrefab weaponPrefab;
    public Bullet bullet;
    public int cartrigeClip;
    public float rateOfFire;
    public float bulletDispersion;
}
