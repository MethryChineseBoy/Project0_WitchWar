using UnityEngine;

public enum PlayerState
{
    Idle,
    Act,
    Dead,
}

public class PlayerFSM : MonoBehaviour
{
    protected GameObject _player;
    protected PlayerLogic _playerLogic;
    protected GameObject _inGameUI;
    protected InGameCtrl _inGameCtrl;
    protected GameObject _inStartUI;
    protected GameObject _inEndUI;
    protected static bool _isAble2Shot = false;
    private static PlayerState _currentPlayerState;

    //玩家初始化
    protected void InitPlayer()
    {
        _player = GameObject.Find("GamePlayer").transform.Find("Player").gameObject;
        _playerLogic = GameObject.Find("GamePlayer").transform.Find("Player").GetComponent<PlayerLogic>();
        _inGameUI = GameObject.Find("GameUI").transform.Find("InGameUI").gameObject;
        _inGameCtrl = GameObject.Find("GameUI").GetComponent<InGameCtrl>();
        _inStartUI= GameObject.Find("StartUI").transform.Find("InStartUI").gameObject;
        _inEndUI = GameObject.Find("EndUI").transform.Find("InEndUI").gameObject;
    }

    //设置玩家状态
    protected void SetPlayerState(PlayerState currentState)
    {
        _currentPlayerState = currentState;
    }

    //取得玩家状态
    protected PlayerState GetPlayerState()
    {
        return _currentPlayerState;
    }

    public static PlayerState GetCurrentPlayerState()
    {
        return _currentPlayerState;
    }
}
