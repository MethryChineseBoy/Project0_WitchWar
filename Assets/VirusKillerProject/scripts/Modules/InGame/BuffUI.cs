using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class BuffUI : MonoBehaviour
{
    private Image _buffProgress;
    private float _buffTime;
    private Sprite _buffSprite;
    private Image _buffImage;
    
    private Buff _buff;
    private string _buffName;

    private int _indexOfBuff;

    void Awake()
    {
        _buffProgress = GetComponent<Image>();
        _buffImage = transform.GetChild(0).GetComponent<Image>();
        _buff = GameObject.Find("GameUI").transform.Find("InGameUI").GetComponent<Buff>();
    }

    private void OnEnable()
    {
        _buffProgress.fillAmount = 1f;
    }

    private void Update()
    {
        _buffTime = _buff.GetBuffTime(_buffName);

        if (_buffTime > 0)
        {
            _buffProgress.fillAmount = _buff.GetBuffTime(_buffName) / 5f;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    public void SetBuffImage(int index)
    {
        _buffName = BuffNameConst.BuffNameList[index];
        _buffSprite = Resources.Load("textures/BuffIcon/" + _buffName, typeof(Sprite)) as Sprite;
        _buffImage.overrideSprite = _buffSprite;
    }
}
