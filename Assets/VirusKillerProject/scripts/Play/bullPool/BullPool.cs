using System.Collections.Generic;
using UnityEngine;

public class BullPool : MonoBehaviour
{
    private int _bulletId = 0;
    private int _maxCount = 100;
    private Dictionary<string, List<GameObject>> _bulletDic = new Dictionary<string, List<GameObject>>();

    #region 单例
    public static BullPool instance;
    private BullPool() { }
    void Awake()
    {
        instance = this;
    }
    #endregion

    //子弹出池，参数为子弹生成时所在的位置
    public GameObject GetBullet(string key, Vector3 position, GameObject bullObj)
    {
        if (!_bulletDic.ContainsKey(key))
        {
            _bulletDic.Add(key, new List<GameObject>(_maxCount / 2));
        }

        List<GameObject> tempList = _bulletDic[key];

        GameObject tempObj = bullObj;

        if (tempList.Count == 0)
        {
            tempObj = Instantiate(bullObj, position, bullObj.transform.rotation,transform);
        }
        else
        {
            tempObj = tempList[0];
            tempList.Remove(tempObj);
        }

        if (key == "playerBullet")
        {
            tempObj.GetComponent<IBullet>().SetId(_bulletId);
            _bulletId++;
        }
        tempObj.GetComponent<PoolUser>().SetIsUse(true);
        return tempObj;
    }

    //子弹归还
    public void Back(string key, GameObject go)
    {
        if (!go.GetComponent<PoolUser>().GetIsUse())
        {
            return;
        }

        List<GameObject> tempList = _bulletDic[key];
        go.SetActive(false);

        if(tempList.Count < _maxCount)
        {
            go.GetComponent<PoolUser>().SetIsUse(false);
            tempList.Add(go);
        }
        else
        {
            Destroy(go);
        }
    }
    
    //清池，重新确定子弹Prefab
    public void ClearAll()
    {
        for(int i = 0; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }

        foreach (List<GameObject> tempList in _bulletDic.Values)
        {
            tempList.Clear();
        }
    }
}
