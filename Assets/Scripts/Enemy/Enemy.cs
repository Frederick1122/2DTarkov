using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class Enemy : Humanoid
{
    [Space(2)]
    [Header("Enemy Parameters")]
    [SerializeField] private int _minDamage;
    [SerializeField] private int _maxDamage;
    [SerializeField] private float _cooldown = 1f;
    private Coroutine _attackRoutine;
    private YieldInstruction _cooldownTime;
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.GetComponent<PlayerHumanoid>())
        {
            return;
        }
        
        if (_attackRoutine != null)
        {
            StopCoroutine(_attackRoutine);
        }

        _attackRoutine = StartCoroutine(AttackRoutine());
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.GetComponent<PlayerHumanoid>())
        {
            return;
        }
        
        if (_attackRoutine != null)
        {
            StopCoroutine(_attackRoutine);
        }
    }

    private void OnValidate()
    {
        if (_minDamage < 0)
        {
            _minDamage = 0;
        }
        
        if (_minDamage > _maxDamage)
        {
            _maxDamage = _minDamage;
        }
    }

    private void Start()
    {
        _cooldownTime = new WaitForSeconds(_cooldown);
    }

    private IEnumerator AttackRoutine()
    {
        while (true)
        {
            yield return _cooldownTime;
            var damage = Random.Range(_minDamage, _maxDamage);
            Player.Instance.ChangeHp(-damage);        
        }
    }
}
