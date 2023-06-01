public class BuffData
{
    private float _buffTime;
    private float _tempBuffTime;    //buff的当前持续时间
    private object _buffActivateConst;
    private object _buffUnactivateConst;
    private object _buffConst;
    private bool _isActivate;

    public BuffData(float buffTime, object bufActivateConst, object buffUnactivateConst)
    {
        _buffTime = buffTime;
        _buffActivateConst = bufActivateConst;
        _buffUnactivateConst = buffUnactivateConst;

        _tempBuffTime = _buffTime;
        _buffConst = _buffUnactivateConst;
        _isActivate = false;
    }
    /// <summary>
    /// 修改buff的当前持续时间（赋值）
    /// </summary>
    /// <param name="time">直接对当前持续时间赋予该值</param>
    public void SetBuffTime(float time)
    {
        _tempBuffTime = time;
    }

    /// <summary>
    /// 修改buff的当前持续时间（减法）
    /// </summary>
    /// <param name="time">将当前持续时间减去该值</param>
    /// <param name="subFlag">传入任意int类型数据表示开启减法计算</param>
    public void SetBuffTime(float time, int subFlag)
    {
        _tempBuffTime -= time;
    }

    /// <summary>
    /// 对buff的当前持续时间进行初始化
    /// </summary>
    public void ResetBuffTime()
    {
        _tempBuffTime = _buffTime;
    }

    public float GetBuffTime()
    {
        return _tempBuffTime;
    }

    /// <summary>
    /// 激活或隐藏buff的效果属性
    /// </summary>
    /// <param name="index">true激活buff属性,false隐藏buff属性</param>
    /// <returns></returns>
    public void SetBuffConst(bool index)
    {
        if (index)
        {
            _buffConst = _buffActivateConst;
            _isActivate = true;
        }
        else
        {
            _buffConst = _buffUnactivateConst;
            _isActivate = false;
        }
    }

    public object GetBuffConst()
    {
        return _buffConst;
    }

    public bool GetBuffConstState()
    {
        return _isActivate;
    }
}