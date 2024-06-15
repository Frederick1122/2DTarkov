using Base.FSM;
using UnityEngine;
using UnityEngine.AI;

public class EnemyFsm : Fsm
{
    [SerializeField] private NavMeshAgent _meshAgent;

    public override void Init()
    {
        _states.Add(typeof(EnemyFsmStateIdle), new EnemyFsmStateIdle(this, _meshAgent));
        _states.Add(typeof(EnemyFsmStateAttack), new EnemyFsmStateAttack(this, _meshAgent));
        _states.Add(typeof(EnemyFsmStateSearch), new EnemyFsmStateSearch(this, _meshAgent));
        
        SetState<EnemyFsmStateIdle>();
    }
}