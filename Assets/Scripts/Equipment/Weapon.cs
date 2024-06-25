using System;
using Base;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "NewWeapon", menuName = "Item/Weapon")]
public class Weapon : Item, IEquip
{
    public Bullet bullet;
    [Space]
    public Sprite topSprite;
    [Space] 
    public bool isMelee;
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
        EquipmentSaveLoadManager.Instance.AddEquipment(this);
    }

    public EquipmentType GetEquipmentType()
    {
        if (isSecondWeapon)
            return EquipmentType.secondWeapon;

        if (isMelee)
            return EquipmentType.melee;
        
        return EquipmentType.firstWeapon;
    }
    
    
}
