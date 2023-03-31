using UnityEngine;

[CreateAssetMenu(fileName = "NewBullet", menuName = "Item/Bullet")]
public class Bullet : Item
{
    [Space]
    public float speed;
    public float damage;
    public BulletLogic bulletPrefab;
}
