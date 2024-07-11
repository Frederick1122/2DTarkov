using System;
using Base.MVC;

namespace UI
{
    public abstract class WindowController : UIController
    {
        public event Action OnClickExitWindow;
        
        public override void Init()
        {
            GetView<WindowView>().OnClickExitWindow += OnClickExitWindow;

            base.Init();
        }

        public override void Terminate()
        {
            GetView<WindowView>().OnClickExitWindow -= OnClickExitWindow;

            base.Terminate();
        }
    }
}
