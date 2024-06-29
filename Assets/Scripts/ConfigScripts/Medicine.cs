using UnityEngine;

namespace ConfigScripts
{
    [CreateAssetMenu(fileName = "NewBullet", menuName = "Item/Medicine")]
    public class MedicineConfig : ItemConfig
    {
        [Space] public float restoredHp;
        public float useTime;
    }
}