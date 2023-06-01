using System.Collections.Generic;
using UnityEngine;

class EnemyFactory
{
    #region 单例
    private static EnemyFactory _instance = null;
    private EnemyFactory() { }
    private Dictionary<string, GameObject> _enemys = new Dictionary<string, GameObject>();

    public static EnemyFactory Instance()
    {
        if (_instance == null)
        {
            _instance = new EnemyFactory();
        }

        return _instance;
    }
    #endregion

    public GameObject CreatEnemy(string nameOfEnemy)
    {
        GameObject tempObj = null;
        if (_enemys.TryGetValue(nameOfEnemy, out tempObj))
        {
            return tempObj;
        }

        tempObj = Resources.Load<GameObject>("Prefabs/enemy/" + nameOfEnemy);
        _enemys.Add(nameOfEnemy, tempObj);
        return tempObj;

    }
}