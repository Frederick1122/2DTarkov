using Managers.SaveLoadManagers;

namespace Base
{
    public interface IEquip
    {
        public void Equip();

        public EquipmentType GetEquipmentType();

    }
    
    public interface IUse
    {
        public void Use();
    }
}
