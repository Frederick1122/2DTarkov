using Base.MVC;
using UI;

public class EndGameWindowController : WindowController //<EndGameUIView, EndGameUIModel>
{
    private string _endText;
    
    public void Init(string endText)
    {
        _endText = endText;
        UpdateView();
    }

    protected override UIModel GetViewData()
    {
        return new EndGameUIModel(_endText);
    }
}

