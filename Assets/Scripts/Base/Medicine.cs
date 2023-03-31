using UnityEngine;

[CreateAssetMenu(fileName = "NewBullet", menuName = "Item/Medicine")]
public class Medicine : Item
{
    [Space] public float restoredHp;
    public float useTime;
}
