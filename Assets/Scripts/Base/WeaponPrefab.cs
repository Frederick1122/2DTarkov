using UnityEngine;

public class WeaponPrefab : MonoBehaviour
{
    [SerializeField] private ShootArea _shootArea;
    [SerializeField] private GameObject _bulletSpawnPoint;


    public ShootArea GetShootArea() => _shootArea;

    public GameObject GetBulletSpawnPoint() => _bulletSpawnPoint;
}
