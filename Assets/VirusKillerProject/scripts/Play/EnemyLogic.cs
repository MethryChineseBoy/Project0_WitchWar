using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class EnemyLogic : MonoBehaviour
{
    private Random _ran;
    private List<string> _namesOfEnemy;
    private Areas[] _spawnAreaArray;    //区域数组
    private int _gameLevel = 1; //初始化关卡数

    #region 单例
    public static EnemyLogic instance;
    private EnemyLogic(){}

    void Awake()
    {
        _namesOfEnemy = new List<string>() { "e_01", "e_02", "e_03a" };
        _ran = new Random();
        _spawnAreaArray = QuadTreeCheck.GetSpawnAreasArray();
        instance = this;

        Action<string, object> disEnableSelf = (string str, object obj) => { StopCoroutine("SpawnEnemyWithTimes"); };
        EventManager.AddEvent(GameEventConst.PlayerDeadEvent, disEnableSelf);
    }
    #endregion

    private IEnumerator SpawnEnemyWithTimes()
    {
        if (_gameLevel <= 10)
        {
            for (int i = 0; i < _gameLevel * 3; i++)
            {
                SpawnEnemy();
                yield return Yielder.WaitForShort();
            }
        }
        else
        {
            for (int i = 0; i < 30; i++)
            {
                SpawnEnemy();
                yield return Yielder.WaitForShort();
            }
        }
    }

    //开启怪物生成的协程
    public void StartSpawnEnemy()
    {
        StartCoroutine("SpawnEnemyWithTimes");
    }

    //关卡数增加
    public void NextLevel()
    {
        _gameLevel++;
    }

    //获取当前关卡数对应的敌人数量
    public int GetEnemyNumberInThisLevel()
    {
        if (_gameLevel <= 10)
        {
            return _gameLevel * 3;
        }

        return 30;
        
    }

    //获取关卡数
    public int GetLevelCount()
    {
        return _gameLevel;
    }

    //在生成点出怪
    private void SpawnEnemy()
    {
        //随机生成敌人的种类
        int ranOfEnemy = _ran.Next(0, 3);
        string nameOfEnemy = _namesOfEnemy[ranOfEnemy];

        //随机敌人的出生区域
        Areas tempArea;
        int ran = _ran.Next(1,5);

        GameObject _obj = EnemyPool.instance.GetEnemy(nameOfEnemy, new Vector3(Camera.main.ViewportToWorldPoint(new Vector3(_spawnAreaArray[ran - 1].GetXPosition() + QuadTreeCheck.GameScreenWidth() / 8, 0)).x, transform.position.y));
        tempArea = _spawnAreaArray[ran - 1];
        
        _obj.GetComponent<IWillCollision>().AddToAreas(tempArea, ran - 1);    //设置主要区域
        _obj.GetComponent<IWillCollision>().AddSubArea(null, 404);    //初始默认将次要区域设置为空
        _obj.SetActive(true);
    }
}