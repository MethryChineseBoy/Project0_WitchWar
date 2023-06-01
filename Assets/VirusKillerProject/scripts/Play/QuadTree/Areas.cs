using System.Collections.Generic;
using UnityEngine;

public class Areas  //划分完成后屏幕区域(屏幕直接划分)
{
    private float _x;   //基准左上角x轴坐标
    private float _y;   //基准左上角y轴坐标
    private float _w;   //区域宽度
    private float _h;   //区域高度
    private List<GameObject> _bulletList = new List<GameObject>();   //待检查碰撞子弹列表
    private List<GameObject> _enemyList = new List<GameObject>();    //待检查碰撞敌人列表

    //屏幕区域构造方法：指定划分区域的左上角坐标、区域的高度和宽度
    public Areas(float x, float y, float w, float h)
    {
        _x = x;
        _y = y;
        _h = h;
        _w = w;
    }

    //分割区域为四份
    public void DivideSelf(List<Areas> areas)
    {
        areas.Add(new Areas(_x, _y, _w / 4, _h));
        areas.Add(new Areas(_x + _w / 4, _y, _w / 4, _h));
        areas.Add(new Areas(_x + _w / 2, _y, _w / 4, _h));
        areas.Add(new Areas(_x + _w * 0.75f, _y, _w / 4, _h));
    }

    //检查区域重载一，通过判断y轴的坐标是否出界返回bool值
    public bool CheckOutArea(float objY)
    {
        if (objY > _y)
        {
            return true;
        }
        return false;
    }

    //检查区域重载二，通过判断左下右三个端点是否出界返回bool值
    public bool CheckOutArea(float viewXOfLeft, float viewXOfRight, float viewYOfButtom)
    {
        if (viewYOfButtom <= _y - _h || viewXOfLeft <= _x || viewXOfRight >= _x + _w)   //触发出界条件
        {
            return true;
        }
        return false;
    }

    //添加游戏对象至对应的列表中
    public void AddToList(GameObject obj, string typeOfObj)
    {
        switch (typeOfObj)
        {
            case "bullet":
                _bulletList.Add(obj);
                break;
            case "enemy":
                _enemyList.Add(obj);
                break;
            default:
                Debug.Log("不存在的游戏对象类型，请输入bullet或者enemy");
                break;
        }
    }

    //获取区域内的游戏对象列表
    public List<GameObject> GetListInArea(string typeOfObj)
    {
        switch (typeOfObj)
        {
            case "bullet":
                return _bulletList;
            case "enemy":
                return _enemyList;
            default:
                return null;
        }
    }

    //从对应列表中删除游戏对象
    public void DeleteSelfInList(GameObject obj, string typeOfObj)
    {
        switch (typeOfObj)
        {
            case "bullet":
                _bulletList.Remove(obj);
                break;
            case "enemy":
                _enemyList.Remove(obj);
                break;
        }
    }

    //获取区域左上角的x轴坐标
    public float GetXPosition()
    {
        return _x;
    }

    //获取区域左上角的y轴坐标
    public float GetYPosition()
    {
        return _y;
    }

    //获取区域下边的y轴坐标
    public float GetYPositionOfButtom()
    {
        return _y - _h;
    }

    //获取区域右边框的x轴
    public float GetXPositionOfRight()
    {
        return _x + _w;
    }

    //获取区域中点的y轴坐标
    public Vector3 GetCenterPositionOfArea()
    {
        return new Vector3(_x + _w / 2, _y - _h / 2);
    }
}

