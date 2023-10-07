using UnityEngine;
using UnityEngine.AI;

public class EnemyFsm : Fsm
{
    [SerializeField] private NavMeshAgent _meshAgent;

    public override void Init()
    {
        AddState<EnemyFsmStateIdle>(new EnemyFsmStateIdle(this, _meshAgent));
        AddState<EnemyFsmStateAttack>(new EnemyFsmStateAttack(this, _meshAgent));
        AddState<EnemyFsmStateSearch>(new EnemyFsmStateSearch(this, _meshAgent));
        
        SetState<EnemyFsmStateIdle>();
    }
}