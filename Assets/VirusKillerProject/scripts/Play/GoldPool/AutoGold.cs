using System;
using UnityEngine;

namespace Assets.VirusKillerProject.scripts.Play.GoldPool
{
    class AutoGold : PoolUser
    {
        private EnemyLogic _enemyLogic;

        private int _goldCost;    //每个金币对象的价值
        private int _levelGold = 7;   //每一关的金币价值倍率
        private Vector3 _distanceToPoint;   //金币对象和回收点的距离
        private Vector3 _goldIconCanvasPoint;   //回收点(Canvas画布的金币图标)在世界坐标系中的坐标
        private Vector3 _activePoint;       //金币动态生成以后的坐标

        void Awake()
        {
            _enemyLogic=EnemyLogic.instance;
            _activePoint = transform.position;
            _goldIconCanvasPoint = GameObject.Find("GameUI/InGameUI/Canvas/GoldUI/GoldIcon").transform.position;
        }

        void OnEnable()
        {
            if (_enemyLogic.GetLevelCount()<=10)
            {
                _goldCost = 10;
            }
            else
            {
                _goldCost = _enemyLogic.GetLevelCount() * _levelGold;
            }

            FindRoad();
        }

        void Update()
        {
            GoToRoad();
        }

        public void SetActivePoint(Vector3 activePoint)
        {
            _activePoint = activePoint;
        }

        //找路
        private void FindRoad()
        {
            _distanceToPoint = _goldIconCanvasPoint - transform.position;
        }

        //寻路方法
        private void GoToRoad()
        {
            Vector3 delta = _distanceToPoint * (Time.deltaTime * 2f);
            delta.z = 0;
            //金币对象自动前往回收点
            transform.position += delta;
            //到达回收点，回收完成，游戏界面金币数增加，金币入池
            if (Math.Abs(transform.position.x - _goldIconCanvasPoint.x) <= 0.2f && Math.Abs(transform.position.y - _goldIconCanvasPoint.y) <= 0.2f)
            {
                InGameCtrl.instance.ChangeGoldCountInGame(_goldCost);
                GoldPool.instance.Back(gameObject);
            }
        }
    }
}