namespace UI
{
    public class WindowController<T, T2> : UIController<T, T2> where T : UIView<T2> where T2 : UIModel
    {
        internal bool _isOpen = false;

        public override void Show()
        {
            _isOpen = true;
            base.Show();
        }
    
        public override void Hide()
        {
            _isOpen = false;
            base.Hide();
        }
    }
}
