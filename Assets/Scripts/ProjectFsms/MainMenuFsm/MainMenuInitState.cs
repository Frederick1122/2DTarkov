using Base.FSM;
using UI;

namespace ProjectFsms.MainMenuFsm
{
    public class MainMenuInitState : FsmState
    {
        public MainMenuInitState(Fsm fsm) : base(fsm)
        {
        }

        public override void Enter()
        {
            UIManager.Instance.OpenWindow<MainMenuWindowController>();
            base.Enter();
        }
    }
}