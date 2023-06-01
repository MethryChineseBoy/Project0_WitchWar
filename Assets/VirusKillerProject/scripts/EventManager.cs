using System;
using System.Collections.Generic;

public static class EventManager
{
    private static Dictionary<string, Dictionary<Action<string, object>, bool>> _eventMap = new Dictionary<string, Dictionary<Action<string, object>, bool>>();

    //事件加入方法
    public static void AddEvent(string eventName, Action<string, object> ea)
    {
        if (!_eventMap.ContainsKey(eventName))
        {
            _eventMap[eventName] = new Dictionary<Action<string, object>, bool>();
        }
        _eventMap[eventName].Add(ea, true);
    }

    //事件移除方法
    public static void RemoveEvent(string eventName, Action<string, object> ea)
    {
        Dictionary<Action<string, object>, bool> tempDic = _eventMap[eventName];
        tempDic.Remove(ea);
    }

    //事件触发
    public static void FireEvent(string eventName, object par)
    {
        Dictionary<Action<string, object>, bool> tempActionDic;
        if(_eventMap.TryGetValue(eventName, out tempActionDic))
        {
            foreach(var temp in tempActionDic.Keys)
            {
                temp(eventName, par);
            }
        }
    }
}