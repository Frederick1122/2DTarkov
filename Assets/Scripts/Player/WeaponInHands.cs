using System;
using System.Collections;
using System.Collections.Generic;
using ConfigScripts;
using Managers.Libraries;
using Managers.SaveLoadManagers;
using Player.InputSystem;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(SpriteRenderer))]
public class WeaponInHands : MonoBehaviour
{
    private static string OBSTACLE_TAG = "Obstacle";

    [SerializeField] private WeaponConfig _knife;
    [Space]
    [SerializeField] private Vector3 _firstWeaponPosition;
    [SerializeField] private Vector3 _secondWeaponPosition;
    [Space]
    [SerializeField] private ShootArea _shootArea;
    [SerializeField] private Transform _bulletSpawnPoint;
    [Space]
    [SerializeField] private SpriteRenderer _spriteRenderer;
    
    private GameObject _player;
    private List<GameObject> _enemies = new List<GameObject>();
    
    private YieldInstruction _rateOfFire;
    private Coroutine _attackRoutine;
    
    private WeaponConfig _activeWeaponConfig;
    private Dictionary<string, BulletLogic> _weaponPool = new();
    private int _ammo = 0;
    private float _bulletDispersion;

    private IInputSystem _inputSystem;
    
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
        EquipmentSaveLoadManager.Instance.OnEquipmentChanged -= SetWeapon; 
        _shootArea.onEnter -= AddEnemy;
        _shootArea.onExit -= RemoveEnemy;
    }

    private void Start()
    {
        EquipmentSaveLoadManager.Instance.OnEquipmentChanged += SetWeapon; 
        SetWeapon(EquipmentSaveLoadManager.Instance.GetEquipment());
        _inputSystem = GameBus.Instance.PlayerInputSystem;
        UpdateFields();
    }
    
    private void SetWeapon(EquipmentData equipmentData)
    {
        WeaponConfig newWeaponConfig;
        if (equipmentData.isSecondWeapon)
        {
            newWeaponConfig = (WeaponConfig) equipmentData.GetEquipment(EquipmentType.secondWeapon);
            _ammo = equipmentData.secondWeaponAmmoInMagazine;
            transform.localPosition = _secondWeaponPosition;
        }
        else
        {
            newWeaponConfig = (WeaponConfig) equipmentData.GetEquipment(EquipmentType.firstWeapon);
            _ammo = equipmentData.firstWeaponAmmoInMagazine;
            transform.localPosition = _firstWeaponPosition;
        }
        
        if (newWeaponConfig == null)
        {
            _activeWeaponConfig = _knife;
            _spriteRenderer.enabled = false;
        }
        else
        {
            if (newWeaponConfig == _activeWeaponConfig)
                return;
            
            _activeWeaponConfig = newWeaponConfig;
            _spriteRenderer.enabled = true;
        }
        
        UpdateWeapon();
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
#if UNITY_EDITOR
        DrawRays();
#endif
        
        Debug.Log($"IsNeedAttack {IsNeedAttack()}");
        if (!IsNeedAttack())
        {
            if (_attackRoutine != null)
            {
                StopCoroutine(_attackRoutine);
                _attackRoutine = null;
            }
            
            return;
        }

        _attackRoutine ??= StartCoroutine(AttackRoutine());
    }

    private void UpdateWeapon()
    {
        _bulletDispersion = _activeWeaponConfig.bulletDispersion;
        _rateOfFire = new WaitForSeconds(_activeWeaponConfig.rateOfFire);
        
        if (!_weaponPool.ContainsKey(_activeWeaponConfig.itemName) && !_activeWeaponConfig.noNeedAmmo)
        {
            var newBulletLogic = ItemLibrary.Instance.GetConfig(_activeWeaponConfig.bulletConfig.configKey).GetComponent<BulletLogic>();
            _weaponPool.Add(_activeWeaponConfig.itemName, newBulletLogic);
        }
        
        _spriteRenderer.sprite = _activeWeaponConfig.topSprite;

        var bulletSpawnPointX = _spriteRenderer.localBounds.max.x;
        var bulletSpawnPointY = _spriteRenderer.localBounds.center.y;
        _bulletSpawnPoint.localPosition = new Vector3(bulletSpawnPointX, bulletSpawnPointY);

        _shootArea.SetDistance(_activeWeaponConfig.maxFiringDistance, _bulletSpawnPoint.position);
    }
    
    private void Attack()
    {
        var bullet = Instantiate(_weaponPool[_activeWeaponConfig.itemName], _bulletSpawnPoint.position, _player.transform.localRotation);
        var currentDispersion = Random.Range(0, _bulletDispersion) - _bulletDispersion / 2;
        _ammo--;
        EquipmentSaveLoadManager.Instance.SetAmmoInMagazine(_activeWeaponConfig, _ammo);
        bullet.transform.Rotate(0,0,currentDispersion);
        bullet.GetComponent<BulletLogic>().Init(_activeWeaponConfig.bulletConfig.speed, _activeWeaponConfig.bulletConfig.damage);
    }

    private bool IsNeedAttack()
    {
        if (_activeWeaponConfig == null || !_activeWeaponConfig.noNeedAmmo && _ammo <= 0)
            return false;

        if (_inputSystem.CurrentInputType == InputType.PC)
            return _inputSystem.ShootInput;
        
        if (_enemies.Count == 0 || !IsEnemyVisible())
            return false;

        return true;
    }
    
    private IEnumerator AttackRoutine()
    {
        while (_ammo > 0)
        {
            Attack();
            yield return _rateOfFire;
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
            var hit = Physics2D.Raycast(_bulletSpawnPoint.position, enemy.transform.position - _bulletSpawnPoint.transform.position);

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
            Debug.DrawRay(_bulletSpawnPoint.position, enemy.transform.position - _bulletSpawnPoint.position, Color.yellow);
        }
    }
    
    #endif
}
