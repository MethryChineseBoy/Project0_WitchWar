using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActState : PlayerFSM
{
    void Awake()
    {
        InitPlayer();
    }

    void Update()
    {
        if (GetPlayerState() == PlayerState.Act)
        {
            _playerLogic.PCMouseController();

            if (!Input.GetKey(KeyCode.Mouse0))
            {
                SetPlayerState(PlayerState.Idle);
                return;
            }

            if (_inGameUI.activeInHierarchy)
            {
                if (!_isAble2Shot)
                {
                    _playerLogic.BeAble2Shot(true);
                    _isAble2Shot = true;
                }
            }
        }
        else
        {
            ExitAct();
        }
    }

    private void ExitAct()
    {
        if (_isAble2Shot)
        {
            _playerLogic.BeAble2Shot(false);
            _isAble2Shot = false;
        }
    }
}
