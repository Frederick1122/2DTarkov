using Base.FSM;

namespace ProjectFsms.GameFsm
{
    public class GameFsm : GlobalFsm
    {
        public override void Init()
        {
            _states.Add(typeof(GameInitState), new GameInitState(this));
            _states.Add(typeof(GameState), new GameState(this));

            base.Init();
        }
    }
}