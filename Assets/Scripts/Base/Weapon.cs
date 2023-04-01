
using System;
using UnityEngine;

[CreateAssetMenu(fileName = "NewWeapon", menuName = "Item/Weapon")]
public class Weapon : Item
{
    [Space]
    public string weaponPrefabPath = "";
    public Bullet bullet;
    public int cartrigeClip;
    public float rateOfFire;
    public float bulletDispersion;

    private void OnValidate()
    {
        if (weaponPrefabPath == "")
            Debug.Log($"Incorrect weaponPrefabPath! Check {this.name}");
    }
}
