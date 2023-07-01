using UnityEngine;

public class UIController<T> : MonoBehaviour where T : UIView  
{
   [SerializeField] protected T _view;

   virtual public void Show()
   {
      _view.Show();
   }

   virtual public void Hide()
   {
      _view.Hide();
   }
      
   virtual public void Init()
   {
      
   }

   virtual public void Terminate()
   {
      
   }
}
