using System;
using Base.MVC;

namespace UI
{
    public abstract class WindowController : UIController
    {
        public event Action OnClickExitWindow;

        private void Start()
        {
            GetView<WindowView>().OnClickExitWindow += OnClickExitWindow;
        }

        protected virtual void OnDestroy()
        {
            GetView<WindowView>().OnClickExitWindow -= OnClickExitWindow;
        }
    }
}
