using System.Collections;
using NavMeshPlus.Components;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class Enemy : Humanoid
{
    [Space(2)]
    [Header("Enemy Parameters")]
    [SerializeField] private int _minDamage;
    [SerializeField] private int _maxDamage;
    [SerializeField] private float _cooldown = 1f;
    [Space (2)]
    [SerializeField] private NavMeshAgent _agent;

    private Coroutine _attackRoutine;
    private YieldInstruction _cooldownTime;
    private PlayerHumanoid _player;
    
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
    
    public void Init()
    {
        _cooldownTime = new WaitForSeconds(_cooldown);
        
        if (!_agent.isOnNavMesh)
        {
            Vector3 warpPosition = new Vector3(_agent.transform.position.x, _agent.transform.position.y,  GameBus.Instance.GetNavMeshSurface().transform.position.z + 1) ; //Set to position you want to warp to
            _agent.transform.position = warpPosition;
            _agent.enabled = false;
            _agent.enabled = true;
            var a = _agent.isOnNavMesh;
        }
        _agent.updateRotation = false;
        _agent.updateUpAxis = false;
        _player = GameBus.Instance.GetPlayer();
    }

    private void Update()
    {
        if(_player != null)
            _agent.SetDestination(_player.transform.position);
    }

    private IEnumerator AttackRoutine()
    {
        while (true)
        {
            yield return _cooldownTime;
            var damage = Random.Range(_minDamage, _maxDamage);
            PlayerSaveLoadManager.Instance.ChangeHp(-damage);        
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

        if (_agent == null)
        {
            _agent = GetComponent<NavMeshAgent>();
        }
    }
}
