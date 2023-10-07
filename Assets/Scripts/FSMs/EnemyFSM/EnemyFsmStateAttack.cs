using UnityEngine.AI;

public class EnemyFsmStateAttack : EnemyFsmState
{
    public EnemyFsmStateAttack(EnemyFsm fsm, NavMeshAgent meshAgent) : base(fsm, meshAgent)
    {
    }

    public override void Update()
    {
        base.Update();
    }
}