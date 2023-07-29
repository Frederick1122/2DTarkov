using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EndGameUIView : WindowView<EndGameUIModel>
{
    [SerializeField] private TMP_Text _text;
    [SerializeField] private Button _goToMainMenuButton;

    private void Start()
    {
        _goToMainMenuButton.onClick.AddListener(SceneLoader.Instance.GoToMainMenu);
    }

    private void OnDisable()
    {
        _goToMainMenuButton.onClick.RemoveListener(SceneLoader.Instance.GoToMainMenu);
    }

    public override void UpdateView(EndGameUIModel uiModel)
    {
        _text.text = uiModel.endText;
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
