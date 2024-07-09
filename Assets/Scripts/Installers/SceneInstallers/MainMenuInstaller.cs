using Base.FSM;
using ProjectFsms.MainMenuFsm;
using Zenject;

namespace Installers.SceneInstallers
{
    public class MainMenuInstaller: MonoInstaller
    {
        public override void InstallBindings()
        {
            FsmManager.Instance.SetActiveFsm<MainMenuFsm>();
        }
    }
}