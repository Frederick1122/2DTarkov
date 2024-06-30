using Base.MVC;
using TMPro;
using UnityEngine;

public class ExitInfoView : UIView
{
    [SerializeField] private TMP_Text text;

    public override void UpdateView(UIModel uiModel)
    {
        var castData = (ExitInfoModel) uiModel;
        text.text = castData.exitName;
        base.UpdateView(uiModel);
    }
}

public class ExitInfoModel : UIModel
{
    public string exitName;

    public ExitInfoModel(string exitName)
    {
        this.exitName = exitName;
    }
}
