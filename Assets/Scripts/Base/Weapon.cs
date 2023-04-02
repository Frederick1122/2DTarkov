
using System;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "NewWeapon", menuName = "Item/Weapon")]
public class Weapon : Item
{
    [Space]
    public string weaponPrefabPath = "";
    public Bullet bullet;
    public int cartrigeClip;
    public float rateOfFire;
    public float bulletDispersion;

    internal override void OnValidate()
    {
        if (weaponPrefabPath == "")
            Debug.Log($"Incorrect weaponPrefabPath! Check {this.name}");

        if(_directory == "")
            _directory = "Weapons/";
        
        base.OnValidate();
    }
}
