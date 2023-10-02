public class EnemyFsm : Fsm
{
    public EnemyFsm()
    {
        AddState<EnemyFsmStateIdle>(new EnemyFsmStateIdle(this));
        AddState<EnemyFsmStateAttack>(new EnemyFsmStateAttack(this));
        AddState<EnemyFsmStateSearch>(new EnemyFsmStateSearch(this));
        
        SetState<EnemyFsmStateIdle>();
    }
}