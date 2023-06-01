using UnityEngine;

public class Main : MonoBehaviour
{
    void Awake()
    {
        Application.targetFrameRate = 60;       //锁60帧
        QuadTreeCheck.InitAreas();              //初始化分屏操作的每一块屏幕的信息
    }
}