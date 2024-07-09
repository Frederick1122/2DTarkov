using Base.FSM;
using ProjectFsms.GameFsm;
using Zenject;

namespace Installers.SceneInstallers
{ 
    public class GameInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            FsmManager.Instance.SetActiveFsm<GameFsm>();
        }
    }
}