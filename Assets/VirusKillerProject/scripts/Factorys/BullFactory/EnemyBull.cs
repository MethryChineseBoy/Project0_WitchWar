using System;
using UnityEngine;

namespace Assets.VirusKillerProject.scripts.Factorys.BullFactory
{
    class EnemyBull:PoolUser
    {
        private int _bulletId;
        private Vector3 _distanceToPlayer;
        private GameObject _player;
        private float _viewCheekOfRight;
        private float _viewCheekOfLeft;
        private float _viewCheekOfButtom;
        private float _viewCheekOfTop;
        private Vector3 _playerViewPosition;
        private Vector3 _objViewPosition;
        private float _objX;
        private float _objY;
        private float _radiusX = 0.05f;
        private float _radiusY = 0.03f;

        void Awake()
        {
            EventManager.AddEvent(GameEventConst.DestroyInEnd, DestroySelf);
            _player =GameObject.Find("GamePlayer").transform.Find("Player").gameObject;
            _viewCheekOfRight = Camera.main.ScreenToViewportPoint(new Vector3(Screen.width, 0)).x;
            _viewCheekOfLeft = Camera.main.ScreenToViewportPoint(new Vector3(0, 0)).x;
            _viewCheekOfButtom = Camera.main.ScreenToViewportPoint(new Vector3(0, 0)).y;
            _viewCheekOfTop = Camera.main.ScreenToViewportPoint(new Vector3(0, Screen.height)).y;
        }

        void OnEnable()
        {
            _distanceToPlayer = _player.transform.position - transform.position;
            _distanceToPlayer.z = 0;
        }

        void Update()
        {
            MoveAndCheck();
        }

        void OnDestroy()
        {
            EventManager.RemoveEvent(GameEventConst.DestroyInEnd, DestroySelf);
        }

        private void MoveAndCheck()
        {
            if (!(bool)Buff.instance.GetBuff(BuffNameConst.Buff5))
            {
                transform.position += _distanceToPlayer * Time.deltaTime;
            }

            _objViewPosition = Camera.main.WorldToViewportPoint(transform.position);
            _playerViewPosition = Camera.main.WorldToViewportPoint(_player.transform.position);
            _objX = _objViewPosition.x;
            _objY = _objViewPosition.y;

            if (_objY <= _viewCheekOfButtom || _objY >= _viewCheekOfTop || _objX >= _viewCheekOfRight || _objX <= _viewCheekOfLeft)
            {
                BullPool.instance.Back("enemyBullet", gameObject);
            }
            else if (Math.Abs(_objX - _playerViewPosition.x) <= _radiusX && Math.Abs(_objY - _playerViewPosition.y) <= _radiusY)
            {
                EventManager.FireEvent(GameEventConst.HitPlayer, "hitPlayer");
            }
        }

        private void DestroySelf(string eventName, object obj)
        {
            BullPool.instance.Back("enemyBullet", gameObject);
        }
    }
}
