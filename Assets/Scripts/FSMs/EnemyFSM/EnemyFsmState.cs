using Base.FSM;
using UnityEngine.AI;

public class EnemyFsmState : FsmState
{
    protected NavMeshAgent _meshAgent;
    
    public EnemyFsmState(EnemyFsm fsm, NavMeshAgent meshAgent) : base(fsm)
    {
        _meshAgent = meshAgent;
    }
}