using Assets.VirusKillerProject.scripts.Factorys.PlayerFactory;
using Unity.Mathematics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class PlayerLogic : MonoBehaviour
{
    //玩家的基础属性
    private int _playerDamage = 3;
    private int _playerShotSpeed = 1;
    private int _playerHp = 1;
    private Buff _buff;

    //玩家经过角色和子弹加成的最终属性
    private int _trueDamage;
    private int _trueShotTimeNeeded;    //每发子弹间隔的时间
    private int _extraBullets = 0;  //额外子弹数量

    //玩家的动画控件
    private Animator _playerDeadAnimator;

    //移动判断
    private bool _isStopMove = true;

    //角色和子弹的预制体
    private GameObject _characterObj;
    private GameObject _bulletObj;

    private Vector3 _mousePosition;

    //玩家当前选择的角色（默认为c_01）
    private string _characterNameInNow = "c_01";

    //玩家的开火组件
    private AutoShotBull _shotBullet;

    private Vector3 _lastPos;
    private Vector3 _deltaPos;

    #region 单例
    public static PlayerLogic instance;
    private PlayerLogic() { }

    void Awake()
    {
        _playerDeadAnimator = GetComponent<Animator>();
        _buff = GameObject.Find("GameUI").transform.Find("InGameUI").GetComponent<Buff>();
        _shotBullet = GetComponent<AutoShotBull>();
        instance = this;
        EventManager.AddEvent(GameEventConst.HitPlayer, HitPlayer);
    }
    #endregion
    
    void OnEnable()
    {
        _playerDeadAnimator.enabled = false;
        UpdatePlayerInfo(_characterNameInNow);
    }

    //PC、手机平台通用控制移动方法
    public void PCMouseController()
    {
        if (Input.GetMouseButtonUp(0))
        {
            _isStopMove = true;
            return;
        }

        if (_isStopMove)
        {
            if (Input.GetMouseButtonDown(0))
            {
                _lastPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                _isStopMove = false;
            }
        }
        _deltaPos = Camera.main.ScreenToWorldPoint(Input.mousePosition) - _lastPos;
        _deltaPos.z = 0;
        transform.position += _deltaPos;

        _lastPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        #region 限制出框
        _mousePosition = Camera.main.WorldToScreenPoint(transform.position);

        if (_mousePosition.x >= Screen.width || _mousePosition.x <= 0 || _mousePosition.y >= Screen.height || _mousePosition.y <= 0)
        {
            transform.position -= _deltaPos;
        }
        #endregion
    }

    //更新玩家属性和模型（在每一局游戏的开始和选择角色界面调用）
    public void UpdatePlayerInfo(string nameOfCharacter)
    {
        _characterObj = CharacterFactory.Instance().CreatCharacter(nameOfCharacter);
        switch (nameOfCharacter)
        {
            case "c_01":
                _bulletObj = BullFactory.Instance().CreatBull("b_01");
                break;
            case "c_02":
                _bulletObj = BullFactory.Instance().CreatBull("b_02");
                break;
        }
        EventManager.FireEvent(GameEventConst.SetPlayerBullet, _bulletObj);

        //更新角色模型
        if (transform.childCount != 0)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                Destroy(transform.GetChild(i).gameObject);
            }
        }
        Instantiate(_characterObj, transform);

        //更新真实属性
        _trueDamage = _characterObj.GetComponent<ICharacter>()
            .AddBulletDamage(_bulletObj.GetComponent<IBullet>())+_playerDamage;
        _trueShotTimeNeeded = _characterObj.GetComponent<ICharacter>().GetShotIntervalTime() - _playerShotSpeed;
        if (_trueShotTimeNeeded < 0)  //当子弹发射间隔时间为0时，溢出的真实射速将计算成额外子弹数量以改变玩家的攻击范围
        {
            _extraBullets = math.abs(_trueShotTimeNeeded) / 5;  //溢出5射速加1额外子弹数量
        }

        EventManager.FireEvent(GameEventConst.SetPlayerIntervalTime, _trueShotTimeNeeded);
    }

    //取当前的额外子弹数量
    public int GetExtraBullets()
    {
        return _extraBullets + (int)_buff.GetBuff(BuffNameConst.Buff2);
    }

    //取当前游戏的真实伤害
    public int GetTrueDamage()
    {
        return _trueDamage * (int)_buff.GetBuff(BuffNameConst.Buff4);
    }

    //增加基础伤害(商店用)
    public void AddDamage()
    {
        _playerDamage += 1;
    }

    //增加基础射速(商店用)
    public void AddShotSpeed()
    {
        _playerShotSpeed += 1;
    }

    //取玩家基础射速
    public int GetPlayerShotSpeed()
    {
        return _playerShotSpeed;
    }

    //取玩家基础伤害
    public int GetPlayerDamage()
    {
        return _playerDamage;
    }

    //设置开火许可
    public void BeAble2Shot(bool isShot)
    {
        if (isShot)
        {
            _shotBullet.SetAble2Shot(true);
        }
        else
        {
            _shotBullet.SetAble2Shot(false);
        }
    }

    //更新玩家选择的角色
    public void SetCharacterNameInNow(string cName)
    {
        _characterNameInNow = cName;
    }

    //取玩家当前选的角色
    public string GetCharacterNameInNow()
    {
        return _characterNameInNow;
    }
    
    //玩家受击
    private void HitPlayer(string nameOfEvent, object obj)
    {
        int _tempHp = _playerHp + (int)_buff.GetBuff(BuffNameConst.Buff1);
        _tempHp--;
        if (_tempHp <= 0)
        {
            EventManager.FireEvent(GameEventConst.PlayerDeadEvent, obj);
            //播放死亡动画
            transform.GetChild(0).gameObject.SetActive(false);
            _playerDeadAnimator.enabled = true;
            //清空得分
            GameManager.Instance().ClearScore();
            InGameCtrl.instance.SetPlayerScoreInView();
        }
    }

    //Animation动画结束帧触发事件
    public void OnEndEvent()
    {
        EventManager.FireEvent(GameEventConst.StateToEnd, "StateToEnd");
        gameObject.SetActive(false);
    }

    public void SetIsMoveStopToTrue()
    {
        _isStopMove = true;
    }
}