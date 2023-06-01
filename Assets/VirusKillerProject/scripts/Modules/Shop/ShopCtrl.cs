using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Vector3 = UnityEngine.Vector3;

public class ShopCtrl : MonoBehaviour
{
    private ShopView _view;
    private GameObject _player;
    private GameObject _characterShop;
    private GameObject _playerShop;
    private GameObject _c01Text;
    private GameObject _c02Text;

    private int _playerShopFlag;   //玩家商店按钮触发flag
    private int _characterShopFlag;//角色商店按钮触发flag

    public static ShopCtrl instance;
    private ShopCtrl(){}

    void Awake()
    {
        instance = this;
        _view = gameObject.GetComponent<ShopView>();
        _player = GameObject.Find("GamePlayer").transform.Find("Player").gameObject;
        _characterShop = GameObject.Find("StartUI").transform.Find("InStartUI/ShopCanvas/CharacterShop").gameObject;
        _playerShop = GameObject.Find("StartUI").transform.Find("InStartUI/ShopCanvas/PlayerShop").gameObject;
        _c01Text = transform.Find("ShopCanvas/CharacterShop/C_01Button/CText").gameObject;
        _c02Text = transform.Find("ShopCanvas/CharacterShop/C_02Button/CText").gameObject;
    }

    void OnEnable()
    {
        _characterShopFlag = 0;
        _playerShopFlag = 0;
        _view.ChangeCostWithPlayer();
        _view.ChangeCostColor();
    }

    #region 玩家属性商店
    public void BuyAddDamage()  //购买加攻商品
    {
        
        if (GameManager.Instance().GetGoldCount()-_view.GetDamageCost() >= 0)
        {
            PlayerLogic.instance.AddDamage();
            GameManager.Instance().ChangeGold(-_view.GetDamageCost());
        }
        _view.ChangeGoldCountInStart();
        _view.ChangeCostWithPlayer();
        _view.ChangeCostColor();
    }

    public void BuyAddShotSpeed()   //购买加射速商品
    {
        if (GameManager.Instance().GetGoldCount()-_view.GetShotCost() >= 0)
        {
            PlayerLogic.instance.AddShotSpeed();
            GameManager.Instance().ChangeGold(-_view.GetShotCost());
        }
        _view.ChangeGoldCountInStart();
        _view.ChangeCostWithPlayer();
        _view.ChangeCostColor();
    }

    public void PutDownPlayerShopButton()    //打开和关闭玩家商店
    {
        if (_player.transform.position.y < 0)
        {
            _player.transform.position = Vector3.zero;
        }

        if (_characterShopFlag==1) //防止两个商店冲突
        {
            _characterShopFlag = 0;
        }

        _playerShopFlag++;
        if (_playerShopFlag == 1)
        {
            _characterShop.SetActive(false);
            _playerShop.SetActive(true);
        }
        else
        {
            _player.transform.position = new Vector3(0,-2);
            _playerShop.SetActive(false);
            _playerShopFlag = 0;
        }
    }
    #endregion

    #region 角色商店
    public void ChooseCharacter1()
    {
        PlayerLogic.instance.SetCharacterNameInNow("c_01");
        PlayerLogic.instance.UpdatePlayerInfo("c_01");
        BullPool.instance.ClearAll();
        _c02Text.SetActive(false);
        _c01Text.SetActive(true);
    }

    public void ChooseCharacter2()
    {
        PlayerLogic.instance.SetCharacterNameInNow("c_02");
        PlayerLogic.instance.UpdatePlayerInfo("c_02");
        BullPool.instance.ClearAll();
        _c01Text.SetActive(false);
        _c02Text.SetActive(true);
    }

    public void PutDownCharacterShopButton()    //打开和关闭角色商店
    {
        if (_player.transform.position.y < 0)
        {
            _player.transform.position = Vector3.zero;
        }

        if (_playerShopFlag == 1)    //防止两个商店冲突
        {
            _playerShopFlag = 0;
        }

        _characterShopFlag++;
        if (_characterShopFlag == 1)
        {
            _playerShop.SetActive(false);
            _characterShop.SetActive(true);
        }
        else
        {
            _player.transform.position = new Vector3(0, -2);
            _characterShop.SetActive(false);
            _characterShopFlag = 0;
        }

    }

    #endregion
}
