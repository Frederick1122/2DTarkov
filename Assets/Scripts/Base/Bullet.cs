using UnityEngine;

[CreateAssetMenu(fileName = "NewBullet", menuName = "Item/Bullet")]
public class Bullet : Item
{
    [Space]
    public float speed;
    public float damage;
    public string bulletPrefabPath = "";
    
    private void OnValidate()
    {
        if (bulletPrefabPath == "") 
            Debug.Log($"Incorrect bulletPrefabPath! Check {this.name}");
    }
}
