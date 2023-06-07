using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private MainMenuView _mainMenuView;
    
    private void OnEnable()
    {
        _mainMenuView.OnClickSortie += ClickOnSortie;
        _mainMenuView.OnClickStorage += ClickOnStorage;
        _mainMenuView.OnClickMerchants += ClickOnMerchants;
    }

    private void OnDisable()
    {
        _mainMenuView.OnClickSortie -= ClickOnSortie;
        _mainMenuView.OnClickStorage -= ClickOnStorage;
        _mainMenuView.OnClickMerchants -= ClickOnMerchants;
    }
    
    private void ClickOnSortie()
    {
        SceneManager.LoadScene("Game");
    }
    
    private void ClickOnStorage()
    {
        
    }
    
    private void ClickOnMerchants()
    {
        
    }
}
