using System.Collections.Generic;
using UnityEngine;

public class EnemyPool : MonoBehaviour
{
    private int _maxCount = 30;
    private Dictionary<string, List<GameObject>> _enemyListDic = new Dictionary<string, List<GameObject>>();

    #region 单例
    public static EnemyPool instance;
    private EnemyPool() { }

    void Awake()
    {
        instance = this;
    }
    #endregion

    public GameObject GetEnemy(string key, Vector3 position)
    {
        if (!_enemyListDic.ContainsKey(key))
        {
            _enemyListDic.Add(key, new List<GameObject>(_maxCount));
        }
        List<GameObject> _enemyList = _enemyListDic[key];

        GameObject obj = null;

        if(_enemyList.Count == 0)
        {
            GameObject _enemyPrefab = EnemyFactory.Instance().CreatEnemy(key);
            obj = Instantiate(_enemyPrefab, position, _enemyPrefab.transform.rotation);
            obj.transform.parent = transform;
        }
        else
        {
            obj = _enemyList[0];
            obj.transform.position = position;
            _enemyList.Remove(obj);
        }

        obj.GetComponent<PoolUser>().SetIsUse(true);
        return obj;
    }

    public void Back(string key, GameObject go)
    {
        if (go.GetComponent<PoolUser>().GetIsUse())
        {
            List<GameObject> enemyList = _enemyListDic[key];
            go.SetActive(false);
            enemyList.Add(go);
            go.GetComponent<PoolUser>().SetIsUse(false);
        }
    }

    //清池
    public void ClearAll()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }

        foreach (List<GameObject> _enemyList in _enemyListDic.Values)
        {
            _enemyList.Clear();
        }
    }
}