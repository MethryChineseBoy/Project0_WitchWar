using UnityEngine;
using UnityEngine.UI;

public class EndGameView : MonoBehaviour
{
    private GameObject _nextLevelButton;
    
    private PlayerLogic _playerLogic;

    private int _winGoldCount;  //本局游戏赚取的金币
    private Text _goldTextInEnd;  //全局金币数文本
    private Text _winGoldText;  //赚取金币数文本
    private Text _levelText;    //当前通过的关卡数

    void Awake()
    {
        _goldTextInEnd = GameObject.Find("EndUI/InEndUI/Canvas/GoldUI/GoldNumber").GetComponent<Text>();
        _playerLogic = GameObject.Find("GamePlayer").transform.Find("Player").GetComponent<PlayerLogic>();
        _winGoldText = GameObject.Find("EndUI/InEndUI/Canvas/WinGoldText").GetComponent<Text>();
        _levelText = GameObject.Find("EndUI/InEndUI/Canvas/LevelText").GetComponent<Text>();
        _nextLevelButton = transform.Find("Canvas/NextLevelButton").gameObject;
    }

    void OnEnable()
    { 

        if (_playerLogic.gameObject.activeInHierarchy)
        {
            _nextLevelButton.SetActive(true);
        }
        else
        {
            _nextLevelButton.SetActive(false);
        }

        SetWinGoldCount(InGameCtrl.instance.GetWinGold());
        _goldTextInEnd.text = GameManager.Instance().GetGoldCount().ToString();
        _winGoldText.text = "你赚到了\n" + _winGoldCount;
        if (_playerLogic.gameObject.activeInHierarchy)
        {
            int level = EnemyLogic.instance.GetLevelCount() - 1;
            _levelText.text = "第" + level + "关完成";
        }
        else
        {
            _levelText.text = "当前在第" + EnemyLogic.instance.GetLevelCount() + "关";
        }   
    }

    public void SetWinGoldCount(int winGoldCount)
    {
        _winGoldCount = winGoldCount;
    }

    public int GetWinGoldCount()
    {
        return _winGoldCount;
    }

    public void UpdateAllText()
    {
        _winGoldText.text = "你赚到了\n" + _winGoldCount;
        _goldTextInEnd.text = GameManager.Instance().GetGoldCount().ToString();
    }
}
