using UnityEngine;

public class AutoShotBull : MonoBehaviour
{
    private int _readyTime = 0;           //子弹准备时间
    private int _intervalTime;
    private int _bulletNumber;
    private bool _able2Shot;
    private GameObject _obj;
    private GameObject _playerBullet;
    private PlayerLogic _playerLogic;

    void Awake()
    {
        _playerLogic = GetComponent<PlayerLogic>();
        EventManager.AddEvent(GameEventConst.SetPlayerBullet, SetPlayerBullet);
        EventManager.AddEvent(GameEventConst.SetPlayerIntervalTime, SetShotIntervalTime);
    }

    void OnEnable()
    {
        _able2Shot = false;
    }

    void FixedUpdate()
    {
        if (_able2Shot)
        {
            _readyTime++;
            if (_readyTime >= _intervalTime)
            {
                ShotBullet();
                _readyTime = 0;
            }
        }
    }

    //设置开火许可
    public void SetAble2Shot(bool able2Shot)
    {
        _able2Shot = able2Shot;
    }

    //确定子弹起始位置
    public void ShotBullet()
    {
        _bulletNumber = _playerLogic.GetExtraBullets();

        if(_bulletNumber == 0)
        {
            Vector3 shotPoint = new Vector3(transform.position.x, transform.position.y + 0.45f);
            _obj = BullPool.instance.GetBullet("playerBullet", shotPoint, _playerBullet);
            _obj.transform.position = shotPoint;
            _obj.SetActive(true);
        }
        else
        {
            for (int i = -_bulletNumber; i <= _bulletNumber; i += 2)
            {
                Vector3 shotPoint = new Vector3(transform.position.x + 0.1f * i, transform.position.y + 0.45f);
                _obj = BullPool.instance.GetBullet("playerBullet", shotPoint, _playerBullet);
                _obj.transform.position = shotPoint;
                _obj.SetActive(true);
            }
        }
    }

    //设置子弹的预制体
    private void SetPlayerBullet(string nameOfEvent, object playerBullet)
    {
        _playerBullet = (GameObject)playerBullet;
    }

    //设置射速事件
    private void SetShotIntervalTime(string nameOfEvent, object shotIntervalTime)
    {
        _intervalTime = (int)shotIntervalTime;
        if (_intervalTime <= 4)
        {
            _intervalTime = 4;  //4为所允许的最小发射时间间隔
        }   
    }
}
