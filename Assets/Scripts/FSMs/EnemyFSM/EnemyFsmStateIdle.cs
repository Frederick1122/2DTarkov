using UnityEngine.AI;

public class EnemyFsmStateIdle : EnemyFsmState
{
    public EnemyFsmStateIdle(EnemyFsm fsm, NavMeshAgent meshAgent) : base(fsm, meshAgent)
    {
    }

    public override void Enter()
    {
        _meshAgent.SetDestination(_meshAgent.transform.position);
    }
}