using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponInHands : MonoBehaviour
{
    private static string OBSTACLE_TAG = "Obstacle";

    [SerializeField] private Weapon _firstWeapon;
    
    private ShootArea _shootArea;
    private GameObject _bulletSpawnPoint;
    private Bullet _bullet;
    
    private float _bulletDispersion;

    private GameObject _player;
    private List<GameObject> _enemies = new List<GameObject>();
    private YieldInstruction _rateOfFireInstruction;
    private Coroutine _attackRoutine;
    
    public void SetWeapon(Weapon newWeapon)
    {
        if (_shootArea != null)
        {
            _shootArea.onEnter -= AddEnemy;
            _shootArea.onExit -= RemoveEnemy;
        }
        
        _bullet = newWeapon.bullet;
        _bulletDispersion = newWeapon.bulletDispersion;
        
        var weapon = Instantiate(newWeapon.weaponPrefab, transform);
        _shootArea = weapon.GetShootArea();
        _bulletSpawnPoint = weapon.GetBulletSpawnPoint();
        _rateOfFireInstruction = new WaitForSeconds(newWeapon.rateOfFire);
        
        _shootArea.onEnter += AddEnemy;
        _shootArea.onExit += RemoveEnemy;
    }
    
    private void Start()
    {
        if(_firstWeapon != null)
            SetWeapon(_firstWeapon);
        
        
        UpdateFields();
    }

    private void AddEnemy(GameObject enemy) => _enemies.Add(enemy);
    
    private void RemoveEnemy(GameObject enemy)
    {
        if (_enemies.Contains(enemy)) 
            _enemies.Remove(enemy);
    }


    private void Update()
    {
        if (_enemies.Count == 0)
        {
            if (_attackRoutine != null)
            {
                StopCoroutine(_attackRoutine);
                _attackRoutine = null;
            }
            return;
        }

        _attackRoutine ??= StartCoroutine(AttackRoutine());
        DrawRays();
    }

    private void Shoot()
    {
        var bullet = Instantiate(_bullet.bulletPrefab, _bulletSpawnPoint.transform.position, _player.transform.localRotation);
        var currentDispersion = Random.Range(0, _bulletDispersion) - _bulletDispersion / 2;
        bullet.transform.Rotate(0,0,currentDispersion);
        bullet.GetComponent<BulletLogic>().Init(_bullet.speed, _bullet.damage);
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
