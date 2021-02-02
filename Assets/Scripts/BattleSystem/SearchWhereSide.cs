using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchWhereSide : MonoBehaviour
{
    //현재 해상도를 담을 변수
    float _Screen_Height;
    float _Screen_Width;

    //화면을 반으로 나누어 왼쪽과 오른쪽 화면으로 나눔
    public Rect LeftSideScreen;
    public Rect RightSideScreen;

    void Start()
    {
        // 현재 해상도를 가져옴
        _Screen_Height = Screen.height;
        _Screen_Width = Screen.width;

        //화면을 반으로 나누어 왼쪽과 오른쪽 화면으로 나눔
        LeftSideScreen = new Rect(0, 0, _Screen_Width / 2, _Screen_Height);
        RightSideScreen = new Rect((_Screen_Width / 2) + 1, 0, _Screen_Width, _Screen_Height);
    }

}
