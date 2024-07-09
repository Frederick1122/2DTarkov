using Base.FSM;

namespace ProjectFsms.MainMenuFsm
{
    public class MainMenuFsm : GlobalFsm
    {
        public override void Init()
        {
            _states.Add(typeof(MainMenuInitState), new MainMenuInitState(this));
            _states.Add(typeof(MainMenuState), new MainMenuState(this));

            base.Init();
        }
    }
}