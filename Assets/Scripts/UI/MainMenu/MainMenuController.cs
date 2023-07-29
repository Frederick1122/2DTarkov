using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : UIController<MainMenuView, MainMenuModel>
{
    [SerializeField] private StorageWindowController _storageWindowUI;
    private void OnEnable()
    {
        _view.OnClickSortie += ClickOnSortie;
        _view.OnClickStorage += ClickOnStorage;
        _view.OnClickMerchants += ClickOnMerchants;

        _storageWindowUI.OnClickExitWindow += ReturnToMainMenu;
        _storageWindowUI.Init();
        
        ReturnToMainMenu();
    }

    private void OnDisable()
    {
        _view.OnClickSortie -= ClickOnSortie;
        _view.OnClickStorage -= ClickOnStorage;
        _view.OnClickMerchants -= ClickOnMerchants;
        
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
}
