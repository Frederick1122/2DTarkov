using System.Collections;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private ShootArea _shootArea;
    [SerializeField] private Bullet _bullet;
    [SerializeField] private GameObject _bulletSpawnPoint;
    [SerializeField] private float _rateOfFire;
    [SerializeField] private float _bulletDispersion;

    [Header("AUTOSERIALIZED FIELD")] [SerializeField]
    private GameObject _player;
    
    private int _enemyCount = 0;
    private YieldInstruction _rateOfFireInstruction;
    private Coroutine _attackRoutine;

    private void OnValidate() => UpdateFields();

    private void Start()
    {
        _shootArea.onEnter += () =>
        {
            _enemyCount++;
        };

        _shootArea.onExit += () =>
        {
            _enemyCount--;
        };

        _rateOfFireInstruction = new WaitForSeconds(_rateOfFire);
        UpdateFields();
    }

    private void Update()
    {
        if (_enemyCount == 0)
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

    private void Shoot()
    {
        var bullet = Instantiate(_bullet, _bulletSpawnPoint.transform.position, _player.transform.localRotation);
        var currentDispersion = Random.Range(0, _bulletDispersion) - _bulletDispersion / 2;
        bullet.transform.Rotate(0,0,currentDispersion);
    }
    
    private IEnumerator AttackRoutine()
    {
        while (_enemyCount != 0)
        {
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
}
