using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class StartGameCtrl : GameStateCtrl
{
    public static StartGameCtrl instance;

    void Awake()
    {
        instance = this;
        StateInitGame();
    }

    void Update()
    {
        if (GetGameState() == GameState.InStart)
        {
            if (PlayerFSM.GetCurrentPlayerState() == PlayerState.Act)
            {
                SetGameState(GameState.InGame);
            }

            if (!_inStartUI.activeInHierarchy)
            {
                _inStartUI.SetActive(true);
            }
        }
        else
        {
            ExitStart();
        }
    }

    private void ExitStart()
    {
        if (_inStartUI.activeInHierarchy)
        {
            _characterShop.SetActive(false);
            _playerShop.SetActive(false);
            _inStartUI.SetActive(false);
        }
    }
}
