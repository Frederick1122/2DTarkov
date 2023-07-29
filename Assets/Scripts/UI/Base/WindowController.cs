using System;

namespace UI
{
    public class WindowController<T, T2> : UIController<T, T2> where T : WindowView<T2> where T2 : UIModel
    {
        public event Action OnClickExitWindow;

        private void Start()
        {
            _view.OnClickExitWindow += OnClickExitWindow;
        }

        private void OnDestroy()
        {
            _view.OnClickExitWindow -= OnClickExitWindow;
        }
    }
}
