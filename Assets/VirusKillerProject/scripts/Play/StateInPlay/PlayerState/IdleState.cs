using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class IdleState : PlayerFSM
{
    void Awake()
    {
        InitPlayer();
    }

    void Update()
    {
        if (GetPlayerState() == PlayerState.Idle)
        {
            if (_inStartUI.activeInHierarchy)
            {
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    if (this.IsPointerOverGameObject(Input.mousePosition))  //没有按在UI上才执行
                    {
                        return;
                    }
                    SetPlayerState(PlayerState.Act);
                }
            }
            else if(_inGameUI.activeInHierarchy)
            {
                if (Input.GetMouseButton(0))
                {
                    SetPlayerState(PlayerState.Act);
                }
            }
        }
        else
        {
            if (!_inGameUI.activeInHierarchy)
            {
                SetPlayerState(PlayerState.Idle);
            }
        }
    }

    //重写EventSystem类的IsPointerOverGameObject方法为射线检测，使其在真机上依然可行
    private bool IsPointerOverGameObject(Vector2 mousePoint)
    {
        //创建一个点击事件
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = mousePoint;
        List<RaycastResult> raycastResults = new List<RaycastResult>();
        //向点击位置发射一条射线，检测是否点击到UI
        EventSystem.current.RaycastAll(eventData, raycastResults);
        return raycastResults.Count > 0;
    }
}