using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPool : MonoBehaviour
{
    private List<GameObject> _itemList;

    #region 单例
    public static ItemPool instance;
    private ItemPool() { }
    private void Awake()
    {
        instance = this;
        _itemList = new List<GameObject>(10);
    }
    #endregion

    public GameObject GetItem(Vector3 position)
    {
        GameObject tempObj = null;

        if(_itemList.Count == 0)
        {
            tempObj = Resources.Load<GameObject>("Prefabs/other/Item");
            tempObj = Instantiate(tempObj, position, tempObj.transform.rotation);
            tempObj.transform.SetParent(transform);
        }
        else
        {
            tempObj = _itemList[0];
            _itemList.Remove(tempObj);
            tempObj.transform.position = position;
        }

        tempObj.GetComponent<PoolUser>().SetIsUse(true);
        return tempObj;
    }

    public void Back(GameObject go)
    {
        if (go.GetComponent<PoolUser>().GetIsUse())
        {
            go.SetActive(false);
            _itemList.Add(go);
            go.GetComponent<PoolUser>().SetIsUse(false);
        }
    }
}