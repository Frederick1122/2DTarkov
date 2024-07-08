using Base.MVC;
using UI;
using UI.Storage;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuWindowController : WindowController //<MainMenuView, MainMenuModel>
{
    [SerializeField] private StorageWindowController _storageWindowUI;
    private void OnEnable()
    {
        GetView<MainMenuView>().OnClickSortie += ClickOnSortie;
        GetView<MainMenuView>().OnClickStorage += ClickOnStorage;
        GetView<MainMenuView>().OnClickMerchants += ClickOnMerchants;

        _storageWindowUI.OnClickExitWindow += ReturnToMainMenu;
        _storageWindowUI.Init();
        
        ReturnToMainMenu();
    }

    private void OnDisable()
    {
        GetView<MainMenuView>().OnClickSortie -= ClickOnSortie;
        GetView<MainMenuView>().OnClickStorage -= ClickOnStorage;
        GetView<MainMenuView>().OnClickMerchants -= ClickOnMerchants;
        
        _storageWindowUI.OnClickExitWindow -= ReturnToMainMenu;
    }
    
    private void ClickOnSortie()
    {
        SceneManager.LoadScene("Game");
    }
    
    private void ClickOnStorage()
    {
        _storageWindowUI.Show();
        _view.Hide();
    }
    
    private void ClickOnMerchants()
    {
        
    }

    private void ReturnToMainMenu()
    {
        _storageWindowUI.Hide();
        _view.Show();
    }

    protected override UIModel GetViewData()
    {
        return new UIModel();
    }
}
