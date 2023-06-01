using System.Collections.Generic;
using UnityEngine;

public static class QuadTreeCheck
{
    private static Vector3 _gameScreen = Camera.main.ScreenToViewportPoint(new Vector3(Screen.width, Screen.height));  //适配屏幕
    private static Areas[] _spawnAreaArray;   //怪物的四个随机生成区域数组
    public static List<Areas> areas = new List<Areas>(16); //屏幕区域列表(敌人用)

    //初始化区域信息
    public static void InitAreas()
    {
        //初始化四块屏幕区域
        Areas bigArea1 = new Areas(0, _gameScreen.y, _gameScreen.x, _gameScreen.y / 4);
        Areas bigArea2 = new Areas(0, _gameScreen.y * 0.75f, _gameScreen.x, _gameScreen.y / 4);
        Areas bigArea3 = new Areas(0, _gameScreen.y / 2, _gameScreen.x, _gameScreen.y / 4);
        Areas bigArea4 = new Areas(0, _gameScreen.y / 4, _gameScreen.x, _gameScreen.y / 2.5f);
        
        bigArea1.DivideSelf(areas);
        bigArea2.DivideSelf(areas);
        bigArea3.DivideSelf(areas);
        bigArea4.DivideSelf(areas);

        _spawnAreaArray = new Areas[] { areas[0], areas[1], areas[2], areas[3]};
    }

    //获取随机生成点数组
    public static Areas[] GetSpawnAreasArray()
    {
        return _spawnAreaArray;
    } 

    //获取当前游戏设备的适配高度
    public static float GameScreenHeight()
    {
        return _gameScreen.y;
    }

    //获取当前游戏设备的适配宽度
    public static float GameScreenWidth()
    {
        return _gameScreen.x;
    }
    
    //遍历区域列表进行检查并设置子弹所属区域及索引
    public static void CheckInAreaOfBullet(GameObject obj)
    {
        float objX = Camera.main.WorldToViewportPoint(obj.transform.position).x;
        float objY = Camera.main.WorldToViewportPoint(obj.transform.position).y;

        //排除掉子弹到屏幕外的情况
        if(objX > _gameScreen.x || objX < 0 || objY >= _gameScreen.y)
        {
            obj.GetComponent<FatherBullet>().SetNullArea();
            return;
        }

        for (int i = 0; i < 16; i++)
        {
            if ((objX > areas[i].GetXPosition() && objX <= areas[i].GetXPositionOfRight()) 
                &&
                (objY > areas[i].GetYPositionOfButtom() && objY <= areas[i].GetYPosition()))
            {
                obj.GetComponent<IWillCollision>().AddToAreas(areas[i], i);
                areas[i].AddToList(obj, "bullet");
                return;
            }
        }
    }
    
    //(子弹专享)离开当前区域时直接赋值前一帧区域正上方区域的信息
    public static void BulletCheckToNextArea(GameObject obj, int indexOfLastArea)
    {
        if (indexOfLastArea > 3) //当子弹位于最上方区域时出界不考虑
        {
            areas[indexOfLastArea - 4].AddToList(obj, "bullet"); //加入正上方区域的列表
            obj.GetComponent<IWillCollision>().AddSubArea(areas[indexOfLastArea], indexOfLastArea);  //将当前区域赋给SubArea，并改变区域索引
            obj.GetComponent<IWillCollision>().AddToAreas(areas[indexOfLastArea - 4], indexOfLastArea - 4); //赋值正上方区域的Areas变量，并改变区域索引
        }
    }

    //(敌人专享)通过敌人的走向预先给敌人设置主要区域和次要区域
    public static void EnemyCheckToNextArea(GameObject obj, int indexOfLastArea, int indexOfSubArea, float viewXOfLeft, float viewXOfRight, float viewYOfButtom)
    {
        //解决敌人触碰左右边框时被误判断为离开区域，导致区域切换的BUG
        if (((indexOfLastArea - 3) % 4 == 0 && viewXOfLeft > areas[indexOfLastArea].GetXPosition() || indexOfLastArea % 4 == 0 && viewXOfRight < areas[indexOfLastArea].GetXPositionOfRight())
            &&
            viewYOfButtom > areas[indexOfLastArea].GetYPositionOfButtom())
        {
            return;
        }

        //向下切换区域
        if (viewYOfButtom <= areas[indexOfLastArea].GetYPositionOfButtom())
        {
            areas[indexOfLastArea].DeleteSelfInList(obj, "enemy");
            indexOfLastArea += 4;
            if (indexOfLastArea > 15)
            {
                indexOfLastArea -= 16;
            }
            areas[indexOfLastArea].AddToList(obj, "enemy");
            obj.GetComponent<IWillCollision>().AddToAreas(areas[indexOfLastArea], indexOfLastArea);

            if (indexOfSubArea != 404)  //当存在次要区域时，主要区域向下切换，次要区域也需要跟着向下切换
            {
                areas[indexOfSubArea].DeleteSelfInList(obj, "enemy");
                indexOfSubArea += 4;
                if (indexOfSubArea > 15)
                {
                    indexOfSubArea -= 16;
                }
                areas[indexOfSubArea].AddToList(obj, "enemy");
                obj.GetComponent<IWillCollision>().AddSubArea(areas[indexOfSubArea], indexOfSubArea);
            }
        }

        //向左右切换区域
        if (viewXOfLeft <= areas[indexOfLastArea].GetXPosition() || viewXOfRight >= areas[indexOfLastArea].GetXPositionOfRight())
        {
            if (indexOfSubArea != 404)  //因为随机出来的最大敌人横向长度也没有一个区域的width这么大，所以当敌人左右切换区域时，不可能存在subArea。
            {
                return;
            }

            obj.GetComponent<IWillCollision>().AddSubArea(areas[indexOfLastArea], indexOfLastArea); //将上一区域设置为次要区域
            if (viewXOfRight >= areas[indexOfLastArea].GetXPositionOfRight())
            {
                ++indexOfLastArea;
            }
            else
            {
                --indexOfLastArea;
            }
            areas[indexOfLastArea].AddToList(obj, "enemy");
            obj.GetComponent<IWillCollision>().AddToAreas(areas[indexOfLastArea], indexOfLastArea);
        }
    }
}
