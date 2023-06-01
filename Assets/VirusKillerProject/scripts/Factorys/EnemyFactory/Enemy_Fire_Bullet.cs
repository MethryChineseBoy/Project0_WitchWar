using UnityEngine;

public class Enemy_Fire_Bullet : MonoBehaviour
{
    private GameObject _enemyBullet;
    private GameObject _obj;
    private float _intervalTime;
    private float _deltaTime;
    private BullPool _bullPool;
    private bool _playerIsDead;

    private void Awake()
    {
        _enemyBullet = BullFactory.Instance().CreatBull("e_b");
        _bullPool = GameObject.Find("BullPool").GetComponent<BullPool>();
        _intervalTime = 3f;
        EventManager.AddEvent(GameEventConst.PlayerDeadEvent, PlayerDead);
    }

    void OnEnable()
    {
        _playerIsDead = false;
        _deltaTime = _intervalTime;
    }

    void Update()
    {
        if (_playerIsDead || (bool)Buff.instance.GetBuff(BuffNameConst.Buff5))
        {
            return;
        }

        if (_deltaTime <= 0)
        {
            _obj = _bullPool.GetBullet("enemyBullet", transform.position, _enemyBullet);
            _obj.transform.position = transform.position;
            _obj.SetActive(true);
            _deltaTime = _intervalTime;
        }
        _deltaTime -= Time.deltaTime;
    }

    private void PlayerDead(string nameOfEvent, object obj)
    {
        _playerIsDead = true;
    }
}