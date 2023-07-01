using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(SpriteRenderer))]
public class WeaponInHands : MonoBehaviour
{
    private static string OBSTACLE_TAG = "Obstacle";
    [SerializeField] private Vector3 _firstWeaponPosition;
    [SerializeField] private Vector3 _secondWeaponPosition;
    
    [SerializeField] private ShootArea _shootArea;
    [SerializeField] private GameObject _bulletSpawnPoint;

    [SerializeField] private SpriteRenderer _spriteRenderer;
    
    private GameObject _player;
    private List<GameObject> _enemies = new List<GameObject>();
    
    private YieldInstruction _rateOfFireInstruction;
    private Coroutine _attackRoutine;
    
    private Weapon _activeWeapon;
    private Dictionary<string, BulletLogic> _weaponPool = new();
    private int _ammo = 0;
    private float _bulletDispersion; 

    private void OnValidate()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        _shootArea.onEnter += AddEnemy;
        _shootArea.onExit += RemoveEnemy;
    }

    private void OnDisable()
    {
        Equipment.Instance.OnEquipmentChanged -= SetWeapon; 
        _shootArea.onEnter -= AddEnemy;
        _shootArea.onExit -= RemoveEnemy;
    }

    private void Start()
    {
        Equipment.Instance.OnEquipmentChanged += SetWeapon; 
        SetWeapon(Equipment.Instance.GetEquipment());
        UpdateFields();
    }
    
    private void SetWeapon(EquipmentData equipmentData)
    {
        Weapon newWeapon;
        if (equipmentData.isSecondWeapon)
        {
            newWeapon = (Weapon) equipmentData.GetEquipment(EquipmentType.secondWeapon);
            _ammo = equipmentData.secondWeaponAmmoInMagazine;
            transform.localPosition = _secondWeaponPosition;
        }
        else
        {
            newWeapon = (Weapon) equipmentData.GetEquipment(EquipmentType.firstWeapon);
            _ammo = equipmentData.firstWeaponAmmoInMagazine;
            transform.localPosition = _firstWeaponPosition;
        }
        
        if (newWeapon == null)
        {
            _activeWeapon = null;
            _spriteRenderer.enabled = false;
        }
        else
        {
            _spriteRenderer.enabled = true;
        }
        
        if (newWeapon == _activeWeapon)
        {
            return;
        }
        
        _bulletDispersion = newWeapon.bulletDispersion;
        _rateOfFireInstruction = new WaitForSeconds(newWeapon.rateOfFire);
        
        if (!_weaponPool.ContainsKey(newWeapon.itemName))
        {
            var newBulletLogic = Resources.Load(newWeapon.bullet.bulletPrefabPath).GetComponent<BulletLogic>();
            _weaponPool.Add(newWeapon.itemName, newBulletLogic);
        }
        
        _spriteRenderer.sprite = newWeapon.topSprite;

        var bulletSpawnPointX = _spriteRenderer.localBounds.max.x;
        var bulletSpawnPointY = _spriteRenderer.localBounds.center.y;
        _bulletSpawnPoint.transform.localPosition = new Vector3(bulletSpawnPointX, bulletSpawnPointY);

        _shootArea.SetDistance(newWeapon.maxFiringDistance, _bulletSpawnPoint.transform.position);
        
        _activeWeapon = newWeapon;
    }

    private void AddEnemy(GameObject enemy)
    {
        _enemies.Add(enemy);
    }

    private void RemoveEnemy(GameObject enemy)
    {
        if (_enemies.Contains(enemy)) 
            _enemies.Remove(enemy);
    }
    
    private void Update()
    {
        if (_enemies.Count == 0 || _ammo <= 0 || _activeWeapon == null)
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
        var bullet = Instantiate(_weaponPool[_activeWeapon.itemName], _bulletSpawnPoint.transform.position, _player.transform.localRotation);
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
            var playerComponent = GetComponentInParent<PlayerHumanoid>();
            
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
