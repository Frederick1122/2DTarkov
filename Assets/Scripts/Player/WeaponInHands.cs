using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class WeaponInHands : MonoBehaviour
{
    private static string OBSTACLE_TAG = "Obstacle";
    
    private ShootArea _shootArea;
    private GameObject _bulletSpawnPoint;

    private float _bulletDispersion;

    private GameObject _player;
    private List<GameObject> _enemies = new List<GameObject>();
    private YieldInstruction _rateOfFireInstruction;
    private Coroutine _attackRoutine;
    private Weapon _activeWeapon;
    private Dictionary<Weapon, WeaponInPool> _weaponPool = new();
    private int _ammo = 0;
    
    private void SetWeapon(EquipmentData equipmentData)
    {
        var newWeapon = new Weapon();
        if (equipmentData.isSecondWeapon)
        {
            newWeapon = (Weapon) equipmentData.GetEquipment(EquipmentType.secondWeapon);
            _ammo = equipmentData.secondWeaponAmmoInMagazine;
        }
        else
        {
            newWeapon = (Weapon) equipmentData.GetEquipment(EquipmentType.firstWeapon);
            _ammo = equipmentData.firstWeaponAmmoInMagazine;
        }

        if (newWeapon == _activeWeapon)
        {
            return;
        }

        if (_shootArea != null)
        {
            _shootArea.onEnter -= AddEnemy;
            _shootArea.onExit -= RemoveEnemy;
        }
        
        if (newWeapon == null)
        {
             return;
        }

        _bulletDispersion = newWeapon.bulletDispersion;
        _rateOfFireInstruction = new WaitForSeconds(newWeapon.rateOfFire);

        WeaponPrefab weapon;
        
        if (_weaponPool.ContainsKey(newWeapon))
        {
            _weaponPool[newWeapon].WeaponPrefab.gameObject.SetActive(true);
            weapon = _weaponPool[newWeapon].WeaponPrefab;
        }
        else
        {
            weapon = Instantiate(Resources.Load(newWeapon.weaponPrefabPath), transform).GetComponent<WeaponPrefab>();
            var newWeaponInPool = new WeaponInPool(newWeapon, weapon);
            _weaponPool.Add(newWeapon, newWeaponInPool);
        }
        
        _shootArea = weapon.GetShootArea();
        _bulletSpawnPoint = weapon.GetBulletSpawnPoint();
        _shootArea.onEnter += AddEnemy;
        _shootArea.onExit += RemoveEnemy;
        
        if (_activeWeapon != null || _activeWeapon != default) 
            _weaponPool[_activeWeapon].WeaponPrefab.gameObject.SetActive(false);
        
        _activeWeapon = newWeapon;
    }
    
    private void Start()
    {
        Equipment.Instance.OnEquipmentChanged += SetWeapon; 
        
        SetWeapon(Equipment.Instance.GetEquipment());
        UpdateFields();
    }

    private void OnDisable()
    {
        Equipment.Instance.OnEquipmentChanged += SetWeapon; 
    }

    private void AddEnemy(GameObject enemy) => _enemies.Add(enemy);
    
    private void RemoveEnemy(GameObject enemy)
    {
        if (_enemies.Contains(enemy)) 
            _enemies.Remove(enemy);
    }


    private void Update()
    {
        if (_enemies.Count == 0 || _ammo <= 0)
        {
            if (_attackRoutine != null)
            {
                StopCoroutine(_attackRoutine);
                _attackRoutine = null;
            }
            return;
        }

        _attackRoutine ??= StartCoroutine(AttackRoutine());
        
        #if UNITY_EDITOR
        DrawRays();
        #endif
    }

    private void Shoot()
    {
        var bullet = Instantiate(_weaponPool[_activeWeapon].BulletLogic, _bulletSpawnPoint.transform.position, _player.transform.localRotation);
        var currentDispersion = Random.Range(0, _bulletDispersion) - _bulletDispersion / 2;
        _ammo--;
        Equipment.Instance.SetAmmoInMagazine(_activeWeapon, _ammo);
        bullet.transform.Rotate(0,0,currentDispersion);
        bullet.GetComponent<BulletLogic>().Init(_activeWeapon.bullet.speed, _activeWeapon.bullet.damage);
    }
    
    private IEnumerator AttackRoutine()
    {
        while (_enemies.Count != 0)
        {
            if(!IsEnemyVisible())
                break;
            
            Shoot();
            yield return _rateOfFireInstruction;
        }

        if (_attackRoutine != null && _attackRoutine != default) 
            _attackRoutine = null;
    }
    
    private void UpdateFields()
    {
        if (_player == null || _player == default)
        {
            var playerComponent = GetComponentInParent<Player>();
            
            if(playerComponent != null)
                _player = playerComponent.gameObject;
        }
    }

    private bool IsEnemyVisible()
    {
        foreach (var enemy in _enemies)
        {
            var hit = Physics2D.Raycast(_bulletSpawnPoint.transform.position, enemy.transform.position - _bulletSpawnPoint.transform.position);

            if (hit.collider != null)
            {
                if(hit.collider.gameObject.CompareTag(OBSTACLE_TAG))
                    continue;
            }

            return true;
        }

        return false;
    }

    struct WeaponInPool
    {
        public WeaponPrefab WeaponPrefab { get; }
        public BulletLogic BulletLogic { get; }

        public WeaponInPool(Weapon weaponConf, WeaponPrefab weaponPrefab)
        {
            WeaponPrefab = weaponPrefab;
            BulletLogic = Resources.Load(weaponConf.bullet.bulletPrefabPath).GetComponent<BulletLogic>();
        }
    }
    
    #if UNITY_EDITOR
    private void DrawRays()
    {
        foreach (var enemy in _enemies)
        {
            Debug.DrawRay(_bulletSpawnPoint.transform.position, enemy.transform.position - _bulletSpawnPoint.transform.position, Color.yellow);
        }
    }
    
    #endif
}
