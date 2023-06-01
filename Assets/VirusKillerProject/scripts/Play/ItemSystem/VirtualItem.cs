using System;
using UnityEngine;
using Random = System.Random;

/*
 * 抽象道具效果表
 * 道具1——无敌5秒
 * 道具2——5秒内额外子弹数量加2
 * 道具3——5秒内子弹可穿透
 * 道具4——5秒内攻击力提升为3倍
 * 道具5——时间停止5秒
 * 道具6——从前面5种buff中随机一种
 */

//道具的图像，自动移动和触碰玩家后加载buff组件并销毁自身
public class VirtualItem : PoolUser
{
    private SpriteRenderer _spriteRenderer;
    private InGameCtrl _inGameCtrl;
    private Texture2D _texture;
    private GameObject _player;
    private Buff _buff;
    private Random _ran;
    private int _itemID;    //道具生成即确定的类型编号
    private float _moveSpeed = 7f;//道具在画面中的移动速度
    private int _moveDirInX; //道具在画面中的x轴方向
    private int _moveDirInY; //道具在画面中的y轴方向
    private Vector3 _moveVec3;
    private float _playerObjX;
    private float _playerObjY;
    private float _objX;
    private float _objY;

    //游戏界面边框视图坐标值
    private float _viewCheekOfRight;
    private float _viewCheekOfLeft;
    private float _viewCheekOfButtom;
    private float _viewCheekOfTop;

    void Awake()
    {
        EventManager.AddEvent(GameEventConst.DestroyInEnd, DestroySelf);

        _spriteRenderer = GetComponent<SpriteRenderer>();
        _inGameCtrl = GameObject.Find("GameUI").GetComponent<InGameCtrl>();
        _texture = new Texture2D(27, 30);
        _player = GameObject.Find("GamePlayer").transform.Find("Player").gameObject;
        _buff = GameObject.Find("GameUI").transform.Find("InGameUI").GetComponent<Buff>();
        _ran = new Random();
        _viewCheekOfRight = Camera.main.ScreenToViewportPoint(new Vector3(Screen.width, 0)).x;
        _viewCheekOfLeft = Camera.main.ScreenToViewportPoint(new Vector3(0, 0)).x;
        _viewCheekOfButtom = Camera.main.ScreenToViewportPoint(new Vector3(0, 0)).y;
        _viewCheekOfTop = Camera.main.ScreenToViewportPoint(new Vector3(0, Screen.height)).y;
    }

    void OnEnable()
    {
        //随机设置移动方向和道具类型的图像
        _moveDirInX = _ran.NextDouble() < 0.5 ? -1 : 1;
        _moveDirInY = _ran.NextDouble() < 0.5 ? -1 : 1;
        _moveVec3 = new Vector3(_moveDirInX * _moveSpeed * Time.deltaTime * 0.1f, _moveDirInY * _moveSpeed * Time.deltaTime * 0.1f);
        _itemID = _ran.Next(1, 7);
        int itemId = _itemID - 1;
        _texture = Resources.Load<Texture2D>("textures/Items/item" + itemId);     //sprite图像切换
        _spriteRenderer.sprite = Sprite.Create(_texture, _spriteRenderer.sprite.textureRect, new Vector2(0.5f, 0.5f));
    }

    void Update()
    {
        //移动
        if (!(bool)Buff.instance.GetBuff(BuffNameConst.Buff5))
        {
            transform.Translate(_moveVec3);
        }

        //触墙反弹
        _objX = Camera.main.WorldToViewportPoint(transform.position).x;
        _objY = Camera.main.WorldToViewportPoint(transform.position).y;
        _playerObjX = Camera.main.WorldToViewportPoint(_player.transform.position).x;
        _playerObjY = Camera.main.WorldToViewportPoint(_player.transform.position).y;
        
        if (Math.Abs(_objX - _playerObjX) <= 0.05f && Math.Abs(_objY - _playerObjY) <= 0.05f)
        {
            //6号随机道具
            if (_itemID == 6)
            {
                _itemID = _ran.Next(1, 6);
            }
            
            //与玩家相遇即销毁自身，同时传递buff类型编号
            _inGameCtrl.SetBuffWithIndex(_itemID);
            _buff.SetValue(BuffNameConst.BuffNameList[_itemID - 1]);
            ItemPool.instance.Back(gameObject);
        }

        if (_objX >= _viewCheekOfRight || _objX <= _viewCheekOfLeft)
        {
            var v = _moveVec3;
            v.x *= -1;
            _moveVec3 = v;
        }
        else if (_objY >= _viewCheekOfTop || _objY <= _viewCheekOfButtom)
        {
            var v = _moveVec3;
            v.y *= -1;
            _moveVec3 = v;
        }
    }

    private void DestroySelf(string eventName, object obj)
    {
        ItemPool.instance.Back(gameObject);
    }
}