using UnityEngine;

class Enemy_03a : FatherEnemy
{
    private GameObject _lifeNumber;
    private Animator _anima;
    private int flag;

    private void Awake()
    {
        enemyName = "e_03a";
        spriteX = 0.45f;
        spriteY = 0.83f;
        hp = 35;
        deadScore = 200;
        InitEnemyOnce();
        _lifeNumber = transform.GetChild(0).gameObject;
        _anima = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        InitEnemy();
        tempMoveSpeedInX = 0;
        flag = 0;
        _lifeNumber.SetActive(true);
    }

    protected override void Dead(int enemyScore)
    {
        flag++;
        if (flag == 1)
        {
            //分裂召唤两个enemy_03b
            fenLie(transform.position, true);
            fenLie(transform.position, false);

            //移出检测列表
            mainArea.DeleteSelfInList(gameObject, "enemy");
            mainArea = null;

            //死亡得分
            GameManager.Instance().SetScore(enemyScore);
            StopCoroutine("HitEffect");
            _lifeNumber.SetActive(false);
            _anima.SetTrigger("isFenlie");
        }
    }

    private void fenLie(Vector3 position, bool isRight)
    {
        GameObject obj = EnemyPool.instance.GetEnemy("e_03b", position);
        obj.GetComponent<IWillCollision>().AddSubArea(subArea, indexOfSubArea);
        obj.GetComponent<IWillCollision>().AddToAreas(mainArea, indexOfMainArea);
        obj.SetActive(true);
        obj.GetComponent<Enemy_03b>().SetFaceTo(isRight);
    }

    public void OnEndEvent()
    {
        EnemyPool.instance.Back(enemyName, gameObject);
    }
}