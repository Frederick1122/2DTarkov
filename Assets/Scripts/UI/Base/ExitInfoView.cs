using TMPro;
using UnityEngine;

public class ExitInfoView : UIView<ExitInfoModel>
{
    [SerializeField] private TMP_Text text;

    public override void UpdateView(ExitInfoModel uiModel)
    {
        text.text = uiModel.exitName;
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
