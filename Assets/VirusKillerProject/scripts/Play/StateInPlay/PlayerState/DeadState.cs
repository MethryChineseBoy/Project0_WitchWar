using System;
using UnityEngine;

public class DeadState : PlayerFSM
{
    void Awake()
    {
        InitPlayer();
        Action<string, object> toDead = (string str, object obj) => SetPlayerState(PlayerState.Dead);
        EventManager.AddEvent(GameEventConst.PlayerDeadEvent, toDead);
    }

    void Update()
    {
        if (GetPlayerState() == PlayerState.Dead)
        {
            if (!_inGameUI.activeInHierarchy)
            {
                if (!_player.activeInHierarchy)
                {
                    _player.SetActive(true);
                    _player.transform.position = new Vector3(0, 0);
                }
                SetPlayerState(PlayerState.Idle);
            }
        }
    }
}
