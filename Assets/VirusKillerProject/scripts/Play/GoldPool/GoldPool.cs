using System.Collections.Generic;
using UnityEngine;

namespace Assets.VirusKillerProject.scripts.Play.GoldPool
{
    class GoldPool : MonoBehaviour
    {
        private List<GameObject> _goldPool;
        private int _poolCount = 100;     //金币池的稳定大小

        #region 单例
        public static GoldPool instance;
        private GoldPool() { }
        void Awake()
        {
            instance = this;
            _goldPool = new List<GameObject>(_poolCount);
        }
        #endregion
        
        public GameObject GetGold(Vector3 activePoint)
        {
            GameObject obj = null;

            if (_goldPool.Count == 0)
            {
                GameObject _poolPrefab = (GameObject)Resources.Load("Prefabs/other/gold");
                obj = Instantiate(_poolPrefab, activePoint, _poolPrefab.transform.rotation);
                obj.transform.parent = transform;
            }
            else
            {
                obj = _goldPool[0];
                obj.transform.position = activePoint;
                _goldPool.Remove(obj);
            }

            obj.GetComponent<PoolUser>().SetIsUse(true);
            return obj;
        }

        public void Back(GameObject go)
        {
            if (go.GetComponent<PoolUser>().GetIsUse())
            {
                go.transform.position = Vector3.zero;
                go.SetActive(false);
                _goldPool.Add(go);
                go.GetComponent<PoolUser>().SetIsUse(false);
            }
        }
    }
}