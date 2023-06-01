using UnityEngine;

public class GameManager
{
    private int _gold = 10000;
    private int _score;

    #region 单例

    private static GameManager _instance = null;

    private GameManager(){}

    public static GameManager Instance()
    {
        if (_instance == null)
        {
            _instance = new GameManager();
        }

        return _instance;
    }

    #endregion

    #region 显现和消失的渐变方法
    public Color ToClear(Color color, float imageChangeSpeed)
    {
        //插值计算
        color = Color.Lerp(color, Color.clear, imageChangeSpeed * Time.deltaTime);
        if (color.a <= 0.05f)
        {
            color = Color.clear;
        }

        return color;
    }

    public Color ToWhite(Color color, float imageChangeSpeed)
    {
        color = Color.Lerp(color, Color.white, imageChangeSpeed * Time.deltaTime);
        if (color.a >= 0.95f)
        {
            color = Color.white;
        }

        return color;
    }
    #endregion

    #region 改变全局金币数量
    public void ChangeGold(int changeCount)
    {
        int tempGold = _gold;
        tempGold += changeCount;
        if (tempGold >= 999999)
        {
            _gold = 999999;
        }
        else if (tempGold>=0 && tempGold<999999)
        {
            _gold = tempGold;
        }
    }
    #endregion

    //返回金币数量
    public int GetGoldCount()
    {
        return _gold;
    }

    //取分数值
    public int GetScore()
    {
        return _score;
    }

    //设置分数值
    public void SetScore(int value)
    {
        _score += value;
    }

    //清空分数值
    public void ClearScore()
    {
        _score = 0;
    }
}
