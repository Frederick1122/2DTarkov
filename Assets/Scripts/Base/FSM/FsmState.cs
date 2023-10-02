public abstract class FsmState<T> where T : Fsm
{
    protected readonly T _fsm;

    public FsmState(T fsm)
    {
        _fsm = fsm;
    }
    
    public virtual void Enter() { }
    public virtual void Exit() { }
    public virtual void Update() { }
}
