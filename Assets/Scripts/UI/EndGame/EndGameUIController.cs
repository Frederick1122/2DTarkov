using UI;

public class EndGameUIController : WindowController<EndGameUIView, EndGameUIModel>
{
    public void Init(string endText)
    {
        _view.UpdateView(new EndGameUIModel(endText));
    }
}

