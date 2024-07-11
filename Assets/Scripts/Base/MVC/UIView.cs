using UnityEngine;

namespace Base.MVC
{
    public class UIView : MonoBehaviour
    {
        public virtual void Init(UIModel uiModel)
        {
            UpdateView(uiModel);
        }

        public virtual void Terminate()
        {
            Destroy(gameObject);
        }

        public virtual void Show()
        {
            gameObject.SetActive(true);

            for (var i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(true);
            }
        }

        public virtual void Hide()
        {
            gameObject.SetActive(false);

            for (var i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }
        }
        
        public virtual void UpdateView(UIModel uiModel)
        {

        }
    }
}