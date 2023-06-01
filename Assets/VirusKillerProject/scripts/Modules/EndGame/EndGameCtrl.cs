public class EndGameCtrl : GameStateCtrl
{
    private EndGameView _view;
    public static EndGameCtrl instance;
    private EndGameCtrl() { }

    void Awake()
    {
        instance = this;
        StateInitGame();
        _view = transform.Find("InEndUI").gameObject.GetComponent<EndGameView>();
    }

    void Update()
    {
        if (GetGameState() == GameState.InEnd)
        {
            if (!_inEndUI.activeInHierarchy)
            {
                _inEndUI.SetActive(true);
            }
        }
        else
        {
            ExitEnd();
        }
    }

    //直接开始下一关
    public void ToNextLevel()
    {
        AddGoldWithClickButton();
        SetGameState(GameState.InGame);
    }

    //返回开始界面
    public void ToMenu()
    {
        SetGameState(GameState.InStart);
        AddGoldWithClickButton();
    }

    private void AddGoldWithClickButton()
    {
        if (_view.GetWinGoldCount() != 0)
        {
            //全局的金币数增加
            GameManager.Instance().ChangeGold(_view.GetWinGoldCount());
            //赚取的金币减少
            _view.SetWinGoldCount(0);
        }
    }

    private void ExitEnd()
    {
        if (_inEndUI.activeInHierarchy)
        {
            _inEndUI.SetActive(false);
        }

    }
}
