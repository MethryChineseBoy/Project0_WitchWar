using System.Collections.Generic;
using UnityEngine;

//buff组件
public class Buff : MonoBehaviour
{
    /*
    * 抽象道具效果表
    * 道具1——无敌5秒
    * 道具2——5秒内额外子弹数量加2
    * 道具3——5秒内子弹可穿透
    * 道具4——5秒内攻击力提升为3倍
    * 道具5——时间停止5秒
    * 道具6——从前面5种buff中随机一种
    */
    private Dictionary<string, BuffData> _buffMap = new Dictionary<string, BuffData>();

    public static Buff instance;
    private Buff() { }
    
    void Awake()
    {
        instance = this;
        _buffMap.Add(BuffNameConst.Buff1, new BuffData(5f, 1, 0));
        _buffMap.Add(BuffNameConst.Buff2, new BuffData(5f, 2, 0));
        _buffMap.Add(BuffNameConst.Buff3, new BuffData(5f, 1, 0));
        _buffMap.Add(BuffNameConst.Buff4, new BuffData(5f, 3, 1));
        _buffMap.Add(BuffNameConst.Buff5, new BuffData(5f, true, false));
    }

    void OnEnable()
    {
        //所有buff的当前持续时间清零、效果清空
        foreach (var temp in _buffMap)
        {
            temp.Value.SetBuffTime(0f);
            temp.Value.SetBuffConst(false);
        }
    }

    void Update()
    {
        BuffDelay();
    }

    public void SetValue(string buffName)
    {
        _buffMap[buffName].ResetBuffTime();
        _buffMap[buffName].SetBuffConst(true);

    }

    private void BuffDelay()
    {
        foreach(var temp in _buffMap)
        {
            BuffData tempBuffData = temp.Value;
            if(tempBuffData.GetBuffTime() > 0f)
            {
                tempBuffData.SetBuffTime(Time.deltaTime, 1);
                if (!tempBuffData.GetBuffConstState())
                {
                    tempBuffData.SetBuffConst(true);
                }
            }
            else
            {
                tempBuffData.SetBuffTime(0f);
                if (tempBuffData.GetBuffConstState())
                {
                    tempBuffData.SetBuffConst(false);
                }
            }
        }
    }

    //获取对应索引的buff当前的持续时间
    public float GetBuffTime(string buffName)
    {
        return _buffMap[buffName].GetBuffTime();
    }
    
    public object GetBuff(string buffName)
    {
        return _buffMap[buffName].GetBuffConst();
    }
}