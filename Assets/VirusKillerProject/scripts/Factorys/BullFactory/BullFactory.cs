using System.Collections.Generic;
using UnityEngine;

//子弹工厂
class BullFactory
{
    private Dictionary<string, GameObject> _bullDic = new Dictionary<string, GameObject>();
    private static BullFactory _instance = null;
    private BullFactory() { }

    public static BullFactory Instance()
    {
        if (_instance == null)
        {
            _instance = new BullFactory();
        }
        return _instance;
    }

    public GameObject CreatBull(string nameOfBull)
    {
        GameObject obj = null;
        if (_bullDic.TryGetValue(nameOfBull, out obj))
        {
            return obj;
        }

        obj = Resources.Load<GameObject>("Prefabs/bullet/" + nameOfBull + "/bullet");
        _bullDic.Add(nameOfBull, obj);
        return obj;
    }
}