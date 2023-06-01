using UnityEngine;

public class BackGroundScoll : MonoBehaviour
{
    private GameObject _baseBackground;
    private GameObject _medialBackground;
    private GameObject _topBackground;

    private void Awake()
    {
        _baseBackground = GameObject.Find("BackGround/BaseBackground");
        _medialBackground = GameObject.Find("BackGround/MedialBackground");
        _topBackground = GameObject.Find("BackGround/TopBackground");
    }

    private void Update()
    {
        BackgroundScoll();
    }

    private void BackgroundScoll()
    {
        Scroll(_baseBackground, 0.006f, -15.18f, -0.5f);
        Scroll(_medialBackground, 0.007f, -10f, 2.3f);
        Scroll(_topBackground, 0.004f, -12f, 1.1f);
    }

    private void Scroll(GameObject backGround, float moveSpeed, float buttomPoint, float topPoint)
    {
        backGround.transform.Translate(new Vector3(0, -moveSpeed));
        if (backGround.transform.position.y <= buttomPoint)
        {
            Vector3 temp = backGround.transform.position;
            temp.y = topPoint;
            backGround.transform.position = temp;
        }
    }
}
