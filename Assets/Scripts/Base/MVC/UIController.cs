using UnityEngine;

namespace Base.MVC
{
   public abstract class UIController : MonoBehaviour, IUiController
   {
      [SerializeField] protected UIView _view;

      private bool _isInited = false;
      
      public virtual void Init()
      {
         if (_isInited)
            Debug.LogError($"{gameObject.name} {this} has double init");
         
         _isInited = true;
         _view.Init(GetViewData());
      }

      public virtual void Terminate()
      {
         _view.Terminate();
      }
      
      public virtual void Show()
      {
         if (!_isInited)
            Debug.LogError($"{this.name} not inited before show");

         _view.Show();
      }

      public virtual void Hide()
      {
         _view.Hide();
      }

      public virtual void UpdateView()
      {
         _view.UpdateView(GetViewData());
      }

      public virtual void UpdateView(UIModel uiModel)
      {
         UpdateView();
      }

      protected abstract UIModel GetViewData();

      protected T GetView<T>() where T : UIView
      {
         return (T) _view;
      }
   }

   public interface IUiController
   {
      public void Show() { }

      public virtual void Hide() { }

      public virtual void Init() { }

      public virtual void Terminate() { }
      
      public virtual void UpdateView() { }
   }
}