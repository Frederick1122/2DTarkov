using System;
using Base;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "NewWeapon", menuName = "Item/Weapon")]
public class Weapon : Item, IEquip
{
    [Space]
    public Bullet bullet;
    [Space]
    public Sprite topSprite;
    [Space] 
    public bool isKnife;
    public bool isSecondWeapon;
    public int maxAmmoInMagazine;
    public float rateOfFire;
    public float bulletDispersion;
    public float maxFiringDistance;

    public bool noNeedAmmo;
    
    internal override void OnValidate()
    {
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

        if (isKnife)
            return EquipmentType.knife;
        
        return EquipmentType.firstWeapon;
    }
}
