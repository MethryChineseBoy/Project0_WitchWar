using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

class InGameView : MonoBehaviour
{
    private BackGroundScoll _background;
    private GameObject _player;
    protected PlayerLogic _playerLogic;
    private EnemyLogic _enemyLogic;
    private Slider _levelProgress;  //关卡进度条（反映当前关卡剩余敌人数量）
    private Image _progressImage;
    private Text _scoreText;
    private Text _goldTextInGame;
    private int _playerScore;
    private int _oldScore;
    private int _tempGoldCount; //游戏界面记录的暂时金币数量
    private int _jumpTimes = 10;  //跳动的次数
    private bool _isLevelPass;
    private int _indexOfBuff;
    private GameObject _buffProgress;
    private Buff _buff;
    private GameObject _buffProgressesObj;

    void Awake()
    {
        _player = GameObject.Find("GamePlayer").transform.Find("Player").gameObject;
        _playerLogic = PlayerLogic.instance;
        _enemyLogic = GameObject.Find("EnemySpawnPoint").GetComponent<EnemyLogic>();
        _levelProgress = GameObject.Find("GameUI/InGameUI/Canvas/LevelProgress").GetComponent<Slider>();
        _progressImage = GameObject.Find("GameUI/InGameUI/Canvas/LevelProgress/Fill Area/Fill").GetComponent<Image>();
        _scoreText = GameObject.Find("GameUI/InGameUI/Canvas/ScoreText").GetComponent<Text>();
        _goldTextInGame = GameObject.Find("GameUI/InGameUI/Canvas/GoldUI/GoldNumber").GetComponent<Text>();
        _buffProgress = Resources.Load<GameObject>("Prefabs/UI/BuffProgress");
        _buff = GameObject.Find("GameUI").transform.Find("InGameUI").GetComponent<Buff>();
        _buffProgressesObj = GameObject.Find("GameUI").transform.Find("InGameUI/Canvas/BuffProgresses").gameObject;
        _background = GameObject.Find("BackGround").GetComponent<BackGroundScoll>();
    }

    void OnEnable()
    {
        _background.enabled = true;

        //开场清buff
        foreach (Transform temp in _buffProgressesObj.transform)
        {
            Destroy(temp.gameObject);
        }
        _playerScore = GameManager.Instance().GetScore();
        _oldScore = Convert.ToInt32(_scoreText.text);
        _isLevelPass = false;
        _progressImage.color = Color.white;
        _levelProgress.maxValue = _enemyLogic.GetEnemyNumberInThisLevel();  //由敌人数量重置进度条最大值
        _levelProgress.value = _levelProgress.maxValue;
        _tempGoldCount = 0;
        _goldTextInGame.text = _tempGoldCount.ToString();
        StartCoroutine("AddScoreToText");
    }

    void Update()
    {
        if (_isLevelPass)
        {
            EventManager.FireEvent(GameEventConst.StateToEnd, "StateToEnd");
        }
    }

    void OnDisable()
    {
        StopCoroutine("AddScoreToText");
    }

    //在游戏界面上更新分数
    public void SetPlayerScoreInView()
    {
        //获取当前玩家的得分
        _playerScore = GameManager.Instance().GetScore();
        //获取当前游戏的画面显示的分数
        _oldScore = Convert.ToInt32(_scoreText.text);
    }

    //分数跳变
    private IEnumerator AddScoreToText()
    {
        //获取每一跳动次数的幅度
        int delta = (_playerScore - _oldScore) / _jumpTimes;
        for (int i = 0; i < _jumpTimes; i++)
        {
            yield return Yielder.WaitForEndOfFrame();
            //由显示分数跳向实际分数
            _oldScore += delta;
            _scoreText.text = _oldScore.ToString();
        }
        StartCoroutine("AddScoreToText");
    }

    //实时更新游戏中界面的金币数量
    public void ChangeGoldTextInGame(int changeCount)
    {
        _tempGoldCount += changeCount;
        _goldTextInGame.text = _tempGoldCount.ToString();
    }

    //关卡进度条变更
    public void ChangeLevelProgress(float value)
    {
        _levelProgress.value -= value;
        if (_levelProgress.value <= 0)
        {
            _progressImage.color = Color.clear; //处理应该消失的进度条最后还剩一点点的问题。
            _isLevelPass = true;
        }
    }

    //获取当局游戏赚取的金币数量
    public int GetWinGold()
    {
        return _tempGoldCount;
    }

    //获取当前的关卡进度条是否清空
    public bool GetIsLevelPass()
    {
        return _isLevelPass;
    }

    //在UGUI上设置buff
    public void SetBuffWithIndex(int index)
    {
        _indexOfBuff = index;

        if (_buff.GetBuffTime(BuffNameConst.BuffNameList[_indexOfBuff - 1]) > 0)
        {
            return;
        }

        GameObject obj = Instantiate(_buffProgress, _buffProgress.transform.position, _buffProgress.transform.rotation);
        obj.GetComponent<BuffUI>().SetBuffImage(_indexOfBuff - 1);
        obj.transform.SetParent(_buffProgressesObj.transform);

    }
}