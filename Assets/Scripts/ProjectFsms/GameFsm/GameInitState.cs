using Base.FSM;
using UI;
using UI.Base;

namespace ProjectFsms.GameFsm
{
    public class GameInitState : FsmState
    {
        public GameInitState(Fsm fsm) : base(fsm)
        {
        }

        public override void Enter()
        {
            UIManager.Instance.OpenWindow<BaseUIWindowController>();
            base.Enter();
        }
    }
}