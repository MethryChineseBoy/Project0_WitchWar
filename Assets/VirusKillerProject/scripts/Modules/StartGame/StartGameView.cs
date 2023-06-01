using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class StartGameView : MonoBehaviour
{
    private GameObject _player;
    private EnemyLogic _enemyLogic;
    private Text _menuText;
    private Text _otherText;
    private Text _levelText;
    private GameManager _gameManager=GameManager.Instance();
    private BackGroundScoll _background;

    void Awake()
    {
        _player = GameObject.Find("GamePlayer").transform.Find("Player").gameObject;
        _enemyLogic =EnemyLogic.instance;
        _menuText = GameObject.Find("MenuText").GetComponent<Text>();
        _otherText = GameObject.Find("OtherText").GetComponent<Text>();
        _levelText = GameObject.Find("StartUI/InStartUI/MenuCanvas/LevelText").GetComponent<Text>();
        _background = GameObject.Find("BackGround").GetComponent<BackGroundScoll>();
    }

    void Update()
    {
        if (_menuText.color != Color.white)
        {
            _menuText.color = _gameManager.ToWhite(_menuText.color, 1f);
            _otherText.color = _gameManager.ToWhite(_otherText.color, 1.25f);
            _levelText.color = _gameManager.ToWhite(_otherText.color, 1.25f);
        }
    }

    void OnEnable()
    {
        _background.enabled = false;
        _levelText.text = "当前关卡\n" + _enemyLogic.GetLevelCount();
        _player.SetActive(true);
        _player.transform.position = new Vector3(0, -2);
    }
}