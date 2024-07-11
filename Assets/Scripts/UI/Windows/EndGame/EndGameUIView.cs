using System;
using Base.MVC;
using TMPro;
using UI.Base;
using UnityEngine;
using UnityEngine.UI;

public class EndGameUIView : WindowView
{
    [SerializeField] private TMP_Text _text;
    [SerializeField] private Button _goToMainMenuButton;

    public override void Init(UIModel uiModel)
    {
        _goToMainMenuButton.onClick.AddListener(SceneLoader.Instance.GoToMainMenu);
        base.Init(uiModel);
    }

    public override void Terminate()
    {
        _goToMainMenuButton.onClick.RemoveListener(SceneLoader.Instance.GoToMainMenu);
        base.Terminate();
    }

    public override void UpdateView(UIModel uiModel)
    {
        var castData = (EndGameUIModel) uiModel; 
        
        _text.text = castData.endText;
        base.UpdateView(uiModel);
    }
}

public class EndGameUIModel : UIModel
{
    public string endText;

    public EndGameUIModel(string endText)
    {
        this.endText = endText;
    }
}
