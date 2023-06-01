using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//商店的文本显示和更新脚本
public class ShopView : MonoBehaviour
{
    private PlayerLogic _playerLogic;
    private Text _goldTextInStart;

    private Text _playerDamageText;   //玩家基础伤害值文本
    private Text _playerShotSpeedText;    //玩家基础射速文本

    private int _damageCost;  //增伤商品的购买价格
    private int _shotCost;   //增速商品的购买价格

    private Text _damageCostText;  //增伤商品价格文本
    private Text _shotCostText;     //增速商品价格文本

    void Awake()
    {
        _playerLogic = GameObject.Find("GamePlayer").transform.Find("Player").GetComponent<PlayerLogic>();
        _goldTextInStart = GameObject.Find("StartUI/InStartUI/ShopCanvas/GoldUI/GoldNumber").GetComponent<Text>();
        _playerDamageText = GameObject.Find("StartUI/InStartUI/ShopCanvas/PlayerShop/DamageText").GetComponent<Text>();
        _playerShotSpeedText = GameObject.Find("StartUI/InStartUI/ShopCanvas/PlayerShop/ShotSpeedText").GetComponent<Text>();
        _damageCostText = GameObject.Find("StartUI/InStartUI/ShopCanvas/PlayerShop/LevelUpDamageButton/DNeedGold")
            .GetComponent<Text>();
        _shotCostText= GameObject.Find("StartUI/InStartUI/ShopCanvas/PlayerShop/LevelShotSpeedButton/SNeedGold")
            .GetComponent<Text>();
    }

    void OnEnable()
    {
        //初始化金币数量
        _goldTextInStart.text = GameManager.Instance().GetGoldCount().ToString();
    }

    public void ChangeCostColor()   //更新攻击和射速数值以及金币不足变色警告
    {
        _playerDamageText.text = "基本攻击：" + _playerLogic.GetPlayerDamage();
        _playerShotSpeedText.text = "基本射速：" + _playerLogic.GetPlayerShotSpeed();

        if (GameManager.Instance().GetGoldCount() < _damageCost)
        {
            _damageCostText.color = Color.red;
        }
        else
        {
            _damageCostText.color = Color.white;
        }

        if (GameManager.Instance().GetGoldCount() < _shotCost)
        {
            _shotCostText.color = Color.red;
        }
        else
        {
            _shotCostText.color = Color.white;
        }
    }

    public int GetDamageCost()  //获取当前加攻商品价格
    {
        return _damageCost;
    }

    public int GetShotCost()    //获取当前加射速商品价格
    {
        return _shotCost;
    }

    public void ChangeCostWithPlayer()  //越买越贵方法
    {
        _damageCost = (_playerLogic.GetPlayerDamage() * 50)/2;
        _damageCostText.text = _damageCost.ToString();
        _shotCost = (_playerLogic.GetPlayerShotSpeed() * 1000)/2;
        _shotCostText.text = _shotCost.ToString();
    }

    public void ChangeGoldCountInStart()    //更新金币数量
    {
        _goldTextInStart.text = GameManager.Instance().GetGoldCount().ToString();
    }
}
