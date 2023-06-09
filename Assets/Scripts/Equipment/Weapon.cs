using System;
using Base;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "NewWeapon", menuName = "Item/Weapon")]
public class Weapon : Item, IEquip
{
    [Space]
    public string weaponPrefabPath = "";
    public Bullet bullet;
    public int maxAmmoInMagazine;
    public float rateOfFire;
    public float bulletDispersion;
    public bool isSecondWeapon;
    internal override void OnValidate()
    {
        if (weaponPrefabPath == "")
            Debug.Log($"Incorrect weaponPrefabPath! Check {this.name}");

        if(_directory == "")
            _directory = "Weapons/";
        
        base.OnValidate();
    }

    public void Equip()
    {
        Debug.Log("EQUIP " + name);
        Equipment.Instance.AddEquipment(this);
    }

    public EquipmentType GetEquipmentType()
    {
        if (isSecondWeapon)
            return EquipmentType.secondWeapon;

        return EquipmentType.firstWeapon;
    }
}
