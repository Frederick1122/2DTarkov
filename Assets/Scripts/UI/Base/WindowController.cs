namespace UI
{
    public class WindowController<T> : UIController<T> where T : UIView
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
