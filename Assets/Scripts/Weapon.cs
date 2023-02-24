using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    private static string OBSTACLE_TAG = "Obstacle";
    
    [SerializeField] private ShootArea _shootArea;
    [SerializeField] private Bullet _bullet;
    [SerializeField] private GameObject _bulletSpawnPoint;
    [SerializeField] private float _rateOfFire;
    [SerializeField] private float _bulletDispersion;

    [Header("AUTOSERIALIZED FIELD")] [SerializeField]
    private GameObject _player;
    
    private List<GameObject> _enemies = new List<GameObject>();
    private YieldInstruction _rateOfFireInstruction;
    private Coroutine _attackRoutine;

    private void OnValidate() => UpdateFields();

    private void Start()
    {
        _shootArea.onEnter += AddEnemy;

        _shootArea.onExit += RemoveEnemy;

        _rateOfFireInstruction = new WaitForSeconds(_rateOfFire);
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
        var bullet = Instantiate(_bullet, _bulletSpawnPoint.transform.position, _player.transform.localRotation);
        var currentDispersion = Random.Range(0, _bulletDispersion) - _bulletDispersion / 2;
        bullet.transform.Rotate(0,0,currentDispersion);
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
