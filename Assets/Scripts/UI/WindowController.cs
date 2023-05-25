using UnityEngine;

namespace UI
{
    public class WindowController : MonoBehaviour
    {
        [SerializeField] internal GameObject _windowView;
    
        internal bool _isOpen = false;

        public virtual void OpenWindow()
        {
            _windowView.SetActive(true);
            _isOpen = true;
        }
    
        public virtual void CloseWindow()
        {
            _windowView.SetActive(false);
            _isOpen = false;
        }
    }
}
