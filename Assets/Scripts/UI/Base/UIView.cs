using UnityEngine;

public class UIView : MonoBehaviour
{
    virtual public void Show()
    {
        for (var i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(true);
        }
    }

    virtual public void Hide()
    {
        for (var i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
    }

    virtual public void Init()
    {
        
    }

    virtual public void Terminate()
    {
        
    }
}
