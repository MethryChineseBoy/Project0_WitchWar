using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public enum GameState
{
    InStart,    //开始状态
    InGame,     //游戏状态
    InEnd,      //结算状态
}

public class GameStateCtrl : MonoBehaviour
{
    protected GameObject _player;
    protected GameObject _inStartUI;
    protected GameObject _inGameUI;
    protected GameObject _inEndUI;
    protected GameObject _characterShop;
    protected GameObject _playerShop;
    protected PlayerLogic _playerLogic;
    protected EnemyLogic _enemyLogic;

    private static GameState _currentStateInParent;

    #region 单例
    public static GameStateCtrl _instance;
    private void Awake()
    {
        _instance = this;
    }
    #endregion
    
    protected void StateInitGame()
    {
        _player = GameObject.Find("GamePlayer").transform.Find("Player").gameObject;
        _inStartUI = GameObject.Find("StartUI").transform.Find("InStartUI").gameObject;
        _inGameUI = GameObject.Find("GameUI").transform.Find("InGameUI").gameObject;
        _inEndUI = GameObject.Find("EndUI").transform.Find("InEndUI").gameObject;
        _characterShop = GameObject.Find("StartUI").transform.Find("InStartUI/ShopCanvas/CharacterShop").gameObject;
        _playerShop = GameObject.Find("StartUI").transform.Find("InStartUI/ShopCanvas/PlayerShop").gameObject;
        _playerLogic = GameObject.Find("GamePlayer").transform.Find("Player").GetComponent<PlayerLogic>();
        _enemyLogic = GameObject.Find("EnemySpawnPoint").GetComponent<EnemyLogic>();
    }

    protected void SetGameState(GameState currentState)
    {
        _currentStateInParent = currentState;
        switch (_currentStateInParent)
        {
            case GameState.InStart:
                break;
            case GameState.InGame:
                _enemyLogic.StartSpawnEnemy();
                _playerLogic.UpdatePlayerInfo(_playerLogic.GetCharacterNameInNow());
                break;
            case GameState.InEnd:
                _playerLogic.SetIsMoveStopToTrue();
                EventManager.FireEvent(GameEventConst.DestroyInEnd, "DestroyInEnd");
                break;
        }
    }

    protected GameState GetGameState()
    {
        return _currentStateInParent;
    }
}