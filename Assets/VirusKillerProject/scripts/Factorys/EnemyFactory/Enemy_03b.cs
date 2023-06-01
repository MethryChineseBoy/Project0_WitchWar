using Assets.VirusKillerProject.scripts.Play.GoldPool;
using UnityEngine;

class Enemy_03b : FatherEnemy
{
    private void Awake()
    {
        enemyName = "e_03b";
        spriteX = 0.38f;
        spriteY = 0.32f;
        hp = 65;
        deadScore = 400;
        InitEnemyOnce();
    }

    private void OnEnable()
    {
        InitEnemy();
    }

    protected override void Dead(int enemyScore)
    {
        //掉落道具
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
        InGameCtrl.instance.ChangeLevelProgress(0.5f);
        StopCoroutine("HitEffect");
        EnemyPool.instance.Back(enemyName, gameObject);
    }

    public void SetFaceTo(bool isRight)
    {
        if (isRight)
        {
            spriteRenderer.flipX = true;
            this.isRight = true;
        }
        else
        {
            this.isRight = false;
        }
    }
}
