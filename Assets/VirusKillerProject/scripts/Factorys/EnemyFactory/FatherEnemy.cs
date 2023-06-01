using Assets.VirusKillerProject.scripts.Play.GoldPool;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = System.Random;

class FatherEnemy : PoolUser, IEnemy, IWillCollision
{
    protected string enemyName;
    protected float spriteX;    //图像x轴半径
    protected float spriteY;    //图像y轴半径
    protected int hp;   //敌人血量
    protected float moveSpeedInX; //敌人在x轴上的移动速度
    protected float moveSpeedInY; //敌人在y轴上的移动速度
    protected Vector2 speedVector;  //敌人的速度向量
    protected TextMeshPro lifeNumber; //血量数字
    protected int randomScale; //敌人的随机大小
    protected float xRadius;   //敌人的x轴碰撞判断半径
    protected float yRadius;   //敌人的y轴碰撞判断半径
    protected float viewCheekOfRight; //视图右边框x轴坐标
    protected float viewCheekOfLeft;  //视图左边框x轴坐标
    protected float viewCheekOfButtom;  //视图下方边框y轴坐标
    protected float worldCheekOfRight; //世界坐标右边框x轴坐标
    protected float worldCheekOfLeft;  //世界坐标左边框x轴坐标

    //四个端点的World坐标
    protected float objXOfLeft;  //敌人的左端坐标
    protected float objXOfRight; //敌人的右端坐标
    protected float objYOfButtom;   //敌人的下端坐标
    protected float objYOfTop;  //敌人的上方坐标

    //四个端点的Viewport坐标
    protected float viewXOfLeft;
    protected float viewXOfRight;
    protected float viewYOfButtom;
    protected float viewYOfTop;

    protected EnemyLogic enemyLogic;
    protected int cost = 20;    //敌人掉落的金币数量
    protected int tempHp;    //用于临时扣除的血量
    protected int deadScore;   //敌人的死亡得分
    protected bool isRight;    //敌人朝向的标记,默认左朝向
    protected float tempMoveSpeedInX;  //用于变速处理的x轴速度
    protected float tempMoveSpeedInY;  //用于变速处理的y轴速度
    protected bool isHit;    //用于打击特效的协程开启标志
    protected List<GameObject> bulletList;  //用于子弹碰撞检测的子弹列表
    protected List<int> hitBulletFlagList;       //存储上一次被碰撞检测过的子弹的id
    protected List<GameObject> enemyList;    //用于敌人之间碰撞检测的敌人列表
    protected GameObject player;
    protected PlayerLogic playerLogic;
    protected bool playerIsDead;
    protected GameObject enemySpawnPoint;
    protected Random randomRange;
    protected SpriteRenderer spriteRenderer;
    protected Areas mainArea;
    protected Areas subArea;
    protected int indexOfMainArea;
    protected int indexOfSubArea;
    protected bool withoutArea;
    protected int noAreaIndex = 404;

    protected void InitEnemyOnce()
    {
        speedVector = new Vector2(0, 0); 
        lifeNumber = gameObject.GetComponentInChildren<TextMeshPro>();
        player = GameObject.Find("GamePlayer").transform.Find("Player").gameObject;
        playerLogic = player.GetComponent<PlayerLogic>();
        enemySpawnPoint = GameObject.Find("EnemySpawnPoint");
        enemyLogic = EnemyLogic.instance;
        randomRange = new Random();
        spriteRenderer = GetComponent<SpriteRenderer>();
        hitBulletFlagList = new List<int>();
        enemyList = new List<GameObject>();
        viewCheekOfRight = Camera.main.ScreenToViewportPoint(new Vector3(Screen.width, 0)).x;
        viewCheekOfLeft = Camera.main.ScreenToViewportPoint(new Vector3(0, 0)).x;
        viewCheekOfButtom = Camera.main.ScreenToViewportPoint(new Vector3(0, -Screen.height / 12f)).y;
        worldCheekOfRight = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0)).x;
        worldCheekOfLeft = Camera.main.ScreenToWorldPoint(new Vector3(0, 0)).x;
        EventManager.AddEvent(GameEventConst.PlayerDeadEvent, PlayerDead);
        EventManager.AddEvent(GameEventConst.DestroyInEnd, InActiveSelf);
    }

    protected void InitEnemy()
    {
        //初始化玩家状态
        playerIsDead = false;

        //把sprite颜色和所有的标志初始化
        spriteRenderer.color = Color.white;
        isHit = false;

        //设置随机速度(实际速度/(增量时间*0.5))
        moveSpeedInX = randomRange.Next(2, 4) * 0.25f;
        moveSpeedInY = randomRange.Next(4, 6) * 0.25f;

        //设置随机大小，同时初始化碰撞判断半径，有几率出现较小的敌人
        randomScale = randomRange.Next(8, 11);
        transform.localScale = new Vector3(randomScale * 0.1f, randomScale * 0.1f);
        xRadius = randomScale * 0.1f * spriteX;
        yRadius = randomScale * 0.1f * spriteY;

        ////每次唤醒要把血量加满,并更新血量数字,且随关卡数变化血量和价格
        if (enemyLogic.GetLevelCount() <= 10)
        {
            tempHp = hp + enemyLogic.GetLevelCount() * 40;
        }
        else
        {
            tempHp = hp + enemyLogic.GetLevelCount() * 70;
        }

        lifeNumber.text = tempHp.ToString();
        //设置随机朝向
        double tempDouble = randomRange.NextDouble();
        if (tempDouble >= 0.5)
        {
            spriteRenderer.flipX = true;
            isRight = true;
        }
        else
        {
            isRight = false;
        }

        //初始化移动速度
        tempMoveSpeedInX = moveSpeedInX;
        tempMoveSpeedInY = moveSpeedInY;
    }

    protected void Update()
    {

        if (playerIsDead)
        {
            return;
        }


        if (tempHp <= 0)
        {
            Dead(deadScore);
        }

        else
        {
            CheckBulletList(mainArea);
            if (subArea != null)
            {
                CheckBulletList(subArea);
            }

            if (isHit)
            {
                StartCoroutine("HitEffect");
            }

            float objX = gameObject.transform.position.x;
            float objY = gameObject.transform.position.y;
            objXOfLeft = objX - xRadius;
            objXOfRight = objX + xRadius;
            objYOfButtom = objY - yRadius;
            objYOfTop = objY + yRadius;

            viewXOfLeft = Camera.main.WorldToViewportPoint(new Vector3(objXOfLeft, 0)).x;
            viewXOfRight = Camera.main.WorldToViewportPoint(new Vector3(objXOfRight, 0)).x;
            viewYOfButtom = Camera.main.WorldToViewportPoint(new Vector3(0, objYOfButtom)).y;
            viewYOfTop = Camera.main.WorldToViewportPoint(new Vector3(0, objYOfTop)).y;

            //从最下方出界返回上方
            if (viewYOfButtom <= viewCheekOfButtom)     //敌人从画面下沿出界消失之后会从画面最上方同一x轴位置再次出现。
            {
                transform.position = new Vector3(transform.position.x, enemySpawnPoint.transform.position.y);
                mainArea.DeleteSelfInList(gameObject, "enemy");
                AddToAreas(QuadTreeCheck.areas[indexOfMainArea - 12], indexOfMainArea - 12);
                mainArea.AddToList(gameObject, "enemy");
                if (indexOfSubArea != 404)
                {
                    subArea.DeleteSelfInList(gameObject, "enemy");
                    AddSubArea(QuadTreeCheck.areas[indexOfSubArea - 12], indexOfSubArea - 12);
                    subArea.AddToList(gameObject, "enemy");
                }
            }
            else
            {
                CheckSelfIsInArea();
            }

            if (!(bool)Buff.instance.GetBuff(BuffNameConst.Buff5))
            {
                Move();
            }

            CheckPlayer();

        }
    }

    //实时检查自己是否仍在目前的主区域或次要区域内，否则重新进行区域归类
    protected void CheckSelfIsInArea()
    {
        //主区域检查
        withoutArea = mainArea.CheckOutArea(viewXOfLeft, viewXOfRight, viewYOfButtom);
        if (withoutArea)
        {
            QuadTreeCheck.EnemyCheckToNextArea(gameObject, indexOfMainArea, indexOfSubArea, viewXOfLeft, viewXOfRight, viewYOfButtom);
            withoutArea = false;
        }
        //次要区域检查
        if (subArea != null)
        {
            if (isRight)
            {
                if (viewXOfLeft > subArea.GetXPositionOfRight())     //若是检查到敌人的端点完全离开次要区域，则从区域检查列表中删除自身并置空区域
                {
                    subArea.DeleteSelfInList(gameObject, "enemy");
                    subArea = null;
                    indexOfSubArea = noAreaIndex;
                }
            }
            else
            {
                if (viewXOfRight < subArea.GetXPosition())
                {
                    subArea.DeleteSelfInList(gameObject, "enemy");
                    subArea = null;
                    indexOfSubArea = noAreaIndex;
                }
            }
        }
    }

    //综合移动方法
    protected void Move()
    {
        //①触墙转向
        if (viewXOfRight >= viewCheekOfRight)
        {
            //防止出界
            float dValue = objXOfRight - worldCheekOfRight;
            transform.position -= new Vector3(dValue, 0);

            //sprite翻转
            isRight = !isRight;
            spriteRenderer.flipX = isRight;
        }
        else if (viewXOfLeft <= viewCheekOfLeft)
        {
            //防止出界
            float dValue = worldCheekOfLeft - objXOfLeft;
            transform.position += new Vector3(dValue, 0);

            //sprite翻转
            isRight = !isRight;
            spriteRenderer.flipX = isRight;
        }

        //②自动移动
        speedVector.y = tempMoveSpeedInY * Time.deltaTime * -1f;
        speedVector.x = isRight ? tempMoveSpeedInX * Time.deltaTime : tempMoveSpeedInX * Time.deltaTime * -1f;
        transform.Translate(speedVector, Space.World);
    }

    //子弹列表检查碰撞
    protected void CheckBulletList(Areas area)
    {
        bulletList = area.GetListInArea("bullet");
        if (bulletList.Count > 0)
        {
            bool isPenetrate = false;
            //当子弹为穿透子弹时
            if (bulletList[0].GetComponent<IBullet>().GetBulletHp() != 1)
            {
                isPenetrate = true;
            }

            //遍历区域内的子弹列表
            for (int i = 0; i < bulletList.Count; i++)
            {
                if (isPenetrate)
                {
                    if (hitBulletFlagList.Count != 0 && hitBulletFlagList.Exists(id => id == bulletList[i].GetComponent<IBullet>().GetId()))
                    {
                        continue;
                    }

                    Vector3 bulletPosition = Camera.main.WorldToViewportPoint(bulletList[i].transform.position);
                    if (bulletPosition.y >= viewYOfButtom && bulletPosition.y <= viewYOfTop && bulletPosition.x >= viewXOfLeft && bulletPosition.x <= viewXOfRight)
                    {
                        hitBulletFlagList.Add(bulletList[i].GetComponent<IBullet>().GetId());
                        tempHp -= playerLogic.GetTrueDamage();
                        isHit = true;
                        bulletList[i].GetComponent<IBullet>().HitBullet();
                    }
                }
                else
                {
                    Vector3 bulletPosition = Camera.main.WorldToViewportPoint(bulletList[i].transform.position);
                    if (bulletPosition.y >= viewYOfButtom && bulletPosition.y <= viewYOfTop && bulletPosition.x >= viewXOfLeft && bulletPosition.x <= viewXOfRight)
                    {
                        tempHp -= playerLogic.GetTrueDamage();
                        isHit = true;
                        bulletList[i].GetComponent<IBullet>().HitBullet();
                    }
                }
            }
        }
    }

    //检查玩家碰撞
    protected void CheckPlayer()
    {
        Vector3 playerViewPosition = Camera.main.WorldToViewportPoint(player.transform.position);
        if (playerViewPosition.x > viewXOfLeft && playerViewPosition.x < viewXOfRight && playerViewPosition.y > viewYOfButtom && playerViewPosition.y < viewYOfTop)
        {
            EventManager.FireEvent(GameEventConst.HitPlayer, "hitPlayer");
        }
    }

    //更新玩家生存状态
    protected void PlayerDead(string nameOfEvent, object obj)
    {
        playerIsDead = true;
        spriteRenderer.color = Color.white;
        StopCoroutine("HitEffect");
    }

    protected void InActiveSelf(string nameOfEvent, object obj)
    {
        EnemyPool.instance.Back(enemyName, gameObject);
    }

    //死亡时
    protected virtual void Dead(int enemyScore)
    {
        //清空子弹标记列表
        hitBulletFlagList.Clear();

        //判断是否掉落道具
        bool isWithItem = randomRange.Next(1, 15) < 7 ? true : false;
        if (isWithItem)
        {
            GameObject obj = ItemPool.instance.GetItem(transform.position);
            obj.SetActive(true);
        }

        //移出检测列表
        if (subArea != null)
        {
            subArea.DeleteSelfInList(gameObject, "enemy");
            subArea = null;
        }
        mainArea.DeleteSelfInList(gameObject, "enemy");
        mainArea = null;

        //死亡得分
        GameManager.Instance().SetScore(enemyScore);
        for (int i = 0; i < cost; i++)
        {
            //设置金币对象随机激活在怪物模型轮廓内
            float temp1 = randomRange.Next(-4, 3) * 0.1f;
            float temp2 = randomRange.Next(-4, 3) * 0.1f;
            GameObject obj = GoldPool.instance.GetGold(transform.position + new Vector3(temp1, temp2));
            obj.SetActive(true);
        }
        InGameCtrl.instance.ChangeLevelProgress(1f);
        StopCoroutine("HitEffect");
        EnemyPool.instance.Back(enemyName, gameObject);
    }

    //受击效果&特效协程
    protected IEnumerator HitEffect()
    {
        isHit = false;
        tempMoveSpeedInY = moveSpeedInY / 2;
        if (tempHp <= 0)
        {
            lifeNumber.text = "0";
        }
        else
        {
            lifeNumber.text = tempHp.ToString();
        }
        spriteRenderer.color = Color.red;

        yield return Yielder.WaitForVeryShort();

        spriteRenderer.color = Color.white;
        tempMoveSpeedInY = moveSpeedInY;
    }

    //改变移动的方向
    public void ChangeDir()
    {
        isRight = !isRight;
        spriteRenderer.flipX = isRight;
    }

    //改变移动速度
    public void SetSpeed(int value)
    {
        tempMoveSpeedInY = value;
    }

    //获取移动速度
    public float GetMoveSpeedInY()
    {
        return moveSpeedInY;
    }

    //获取x轴上的半径
    public float GetXRadius()
    {
        return xRadius;
    }

    //获取y轴上的半径
    public float GetYRadius()
    {
        return yRadius;
    }

    //更新该敌人的区域信息
    public void AddToAreas(Areas area)
    {
        mainArea = area;
    }

    //更新该敌人的区域信息和索引
    public void AddToAreas(Areas area, int index)
    {
        mainArea = area;
        indexOfMainArea = index;
    }

    //更新该敌人的次要区域信息
    public void AddSubArea(Areas area, int index)
    {
        subArea = area;
        indexOfSubArea = index;
    }
}