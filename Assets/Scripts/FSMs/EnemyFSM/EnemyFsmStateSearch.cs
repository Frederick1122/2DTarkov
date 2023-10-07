using UnityEngine.AI;

public class EnemyFsmStateSearch : EnemyFsmState
{
    public EnemyFsmStateSearch(EnemyFsm fsm, NavMeshAgent meshAgent) : base(fsm, meshAgent)
    {
    }
}