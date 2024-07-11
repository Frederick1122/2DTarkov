using Base.MVC;
using UI;
using UI.Storage;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuWindowController : WindowController //<MainMenuView, MainMenuModel>
{
    
    public override void Init()
    {
        GetView<MainMenuView>().OnClickSortie += ClickOnSortie;
        GetView<MainMenuView>().OnClickStorage += ClickOnStorage;
        GetView<MainMenuView>().OnClickMerchants += ClickOnMerchants;

        base.Init();
    }

    public override void Terminate()
    {
        GetView<MainMenuView>().OnClickSortie -= ClickOnSortie;
        GetView<MainMenuView>().OnClickStorage -= ClickOnStorage;
        GetView<MainMenuView>().OnClickMerchants -= ClickOnMerchants;
        
        base.Terminate();
    }

    private void ClickOnSortie()
    {
        SceneManager.LoadScene("Game");
    }
    
    private void ClickOnStorage()
    {
        UIManager.Instance.OpenWindow<StorageWindowController>();
    }
    
    private void ClickOnMerchants()
    {
        
    }

    protected override UIModel GetViewData()
    {
        return new UIModel();
    }
}
