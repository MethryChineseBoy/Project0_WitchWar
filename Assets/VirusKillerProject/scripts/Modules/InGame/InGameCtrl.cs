public class InGameCtrl : GameStateCtrl
{
    public static InGameCtrl instance;  //单例
    private InGameCtrl() { }
    private InGameView _view;

    void Awake()
    {
        instance = this;
        StateInitGame();
        _view = transform.Find("InGameUI").GetComponent<InGameView>();
        EventManager.AddEvent(GameEventConst.StateToEnd, StateToEnd);
    }

    void Update()
    {
        if (GetGameState() == GameState.InGame)
        {
            if (!_inGameUI.activeInHierarchy)
            {
                _inGameUI.SetActive(true);
            }
        }
        else
        {
            ExitGame();
        }
    }
    
    public void ChangeGoldCountInGame(int changeCount)
    {
        _view.ChangeGoldTextInGame(changeCount);
    }

    public void SetPlayerScoreInView()
    {
        _view.SetPlayerScoreInView();
    }

    public void ChangeLevelProgress(float value)
    {
        _view.ChangeLevelProgress(value);
    }

    public int GetWinGold()
    {
        return _view.GetWinGold();
    }

    public void SetBuffWithIndex(int index)
    {
        _view.SetBuffWithIndex(index);
    }

    private void ExitGame()
    {
        if (_inGameUI.activeInHierarchy)
        {
            //通过判断玩家是否在场上来决定是否进入下一关
            if (_player.activeInHierarchy)
            {
                _enemyLogic.NextLevel();
            }
            _inGameUI.SetActive(false);
        }
    }

    //切换游戏状态事件
    private void StateToEnd(string nameOfEvent, object obj)
    {
        SetGameState(GameState.InEnd);
    }
}

