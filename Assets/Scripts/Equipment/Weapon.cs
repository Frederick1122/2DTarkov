using System;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "NewWeapon", menuName = "Item/Weapon")]
public class Weapon : EquipmentItem
{
    [Space]
    public string weaponPrefabPath = "";
    public Bullet bullet;
    public int maxAmmoInMagazine;
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

    public override void Equip()
    {
        Debug.Log("EQUIP " + name);
    }

    public override EquipmentType GetEquipmentType() => EquipmentType.weapon;
}
