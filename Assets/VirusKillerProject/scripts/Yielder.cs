using UnityEngine;

//全局复用Yield类
public static class Yielder
{
    private static WaitForSeconds _waitForLong = new WaitForSeconds(1f);
    private static WaitForSeconds _waitForShort = new WaitForSeconds(0.7f);
    private static WaitForSeconds _waitForVeryShort = new WaitForSeconds(0.05f);
    private static WaitForSeconds _waitForVeryLong = new WaitForSeconds(3f);
    private static WaitForEndOfFrame _waitForEndOfFrame = new WaitForEndOfFrame();

    public static WaitForSeconds WaitForLong()
    {
        return _waitForLong;
    }

    public static WaitForSeconds WaitForShort()
    {
        return _waitForShort;
    }

    public static WaitForSeconds WaitForVeryShort()
    {
        return _waitForVeryShort;
    }

    public static WaitForSeconds WaitForVeryLong()
    {
        return _waitForVeryLong;
    }

    public static WaitForEndOfFrame WaitForEndOfFrame()
    {
        return _waitForEndOfFrame;
    }
}