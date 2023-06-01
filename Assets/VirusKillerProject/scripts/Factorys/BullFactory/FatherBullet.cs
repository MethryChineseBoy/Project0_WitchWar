using UnityEngine;

class FatherBullet : PoolUser, IBullet, IWillCollision
{
    protected int id;
    protected float objY;
    protected Areas mainArea; //子弹当前所在的区域
    protected Areas subArea;  //子弹途径的次要区域
    protected bool withoutArea = false;     //判断子弹当前是否离开前一帧所在的区域
    protected int indexOfArea;  //记录当前所在区域在区域列表中的索引
    protected int indexOfSubArea;   //记录当前次要区域在区域列表中的索引
    protected int hitScore;
    protected int indexOfEvent;
    protected bool isInView;
    protected Buff buff;

    protected int damage;
    protected float flySpeed;
    protected int hpOfBullet = 1;
    protected int noAreaIndex = 404;

    public virtual int GetDamage() { return damage; }
    public virtual int GetHitScore() { return hitScore; }
    
    //第一次初始化
    protected void InitBulletOnce()
    {
        EventManager.AddEvent(GameEventConst.DestroyInEnd, DestroySelf);
    }

    //每一次初始化变量
    protected void InitBullet()
    {
        buff = GameObject.Find("GameUI").transform.Find("InGameUI").GetComponent<Buff>();
        indexOfSubArea = noAreaIndex;
        isInView = true;
        subArea = null;
        //加入分屏检测列表
        QuadTreeCheck.CheckInAreaOfBullet(gameObject);
    }

    //在子类的Update方法中执行的方法的封装
    protected void ActiveInUpdate()
    {
        if (isInView)
        {
            CheckSelfIsInArea();
        }
        DestroyInWall();
    }

    //在子类的FixedUpdate方法中执行的方法的封装
    protected void ActiveInFixedUpdate()
    {
        transform.Translate(0, Time.deltaTime * flySpeed, 0, Space.Self);
    }

    void OnDestroy()
    {
        EventManager.RemoveEvent(GameEventConst.DestroyInEnd, DestroySelf);
    }

    //实时检查自己是否仍在目前的区域内，否则重新进行区域归类
    protected void CheckSelfIsInArea()
    {
        objY = Camera.main.WorldToViewportPoint(transform.position).y;
        //主区域检查
        withoutArea = mainArea.CheckOutArea(objY);
        if (withoutArea)
        {
            QuadTreeCheck.BulletCheckToNextArea(gameObject, indexOfArea);
            withoutArea = false;
        }
        //次要区域检查
        if (subArea != null)
        {
            if (objY > mainArea.GetCenterPositionOfArea().y)
            {
                subArea.DeleteSelfInList(gameObject, "bullet");
                subArea = null;
                indexOfSubArea = noAreaIndex;
            }
        }
    }

    //子弹触墙回收
    protected void DestroyInWall()
    {
        if (Camera.main.WorldToViewportPoint(transform.position).y >= Camera.main.ScreenToViewportPoint(new Vector3(0, Screen.height)).y)
        {
            if (isInView)
            {
                mainArea.DeleteSelfInList(gameObject, "bullet");
                GetComponent<IWillCollision>().AddToAreas(null);
            }
            BullPool.instance.Back("playerBullet", gameObject);
            
        }
    }

    //子弹加分回收
    public void HitBullet()
    {
        GameManager.Instance().SetScore(hitScore);
        InGameCtrl.instance.SetPlayerScoreInView();
        int tempHp = hpOfBullet + (int)buff.GetBuff(BuffNameConst.Buff3);
        tempHp--;

        if (tempHp <= 0)
        {
            DestroySelf("DestroySelf", "DestroySelf");
        }
    }

    //获取子弹当前生命
    public int GetBulletHp()
    {
        return hpOfBullet + (int)buff.GetBuff(BuffNameConst.Buff3);
    }

    //更新该子弹的区域信息
    public void AddToAreas(Areas area)
    {
        mainArea = area;
    }

    //更新该子弹的主要区域信息和索引
    public void AddToAreas(Areas area, int index)
    {
        mainArea = area;
        indexOfArea = index;
    }

    //更新该子弹的次要区域信息和索引
    public void AddSubArea(Areas area, int index)
    {
        subArea = area;
        indexOfSubArea = index;
    }

    //无区域情况
    public void SetNullArea()
    {
        isInView = false;
        mainArea = null;
        subArea = null;
        indexOfArea = noAreaIndex;
        indexOfSubArea = noAreaIndex;
    }

    //子弹对象隐藏和退出区域检测列表
    private void DestroySelf(string nameOfEvent, object obj)
    {
        if (indexOfSubArea != noAreaIndex)
        {
            subArea.DeleteSelfInList(gameObject, "bullet");
            subArea = null;
            indexOfSubArea = noAreaIndex;
        }
        if (mainArea != null)
        {
            mainArea.DeleteSelfInList(gameObject, "bullet");
            mainArea = null;
        }
        BullPool.instance.Back("playerBullet", gameObject);
    }

    public void SetId(int value)
    {
        id = value;
    }

    public int GetId()
    {
        return id;
    }
}