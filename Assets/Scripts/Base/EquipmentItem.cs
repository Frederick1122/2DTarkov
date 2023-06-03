using Base;
using UnityEngine;

public abstract class EquipmentItem: Item, IEquip
{
    public virtual void Equip()
    {
        
    }

    public abstract EquipmentType GetEquipmentType();
}