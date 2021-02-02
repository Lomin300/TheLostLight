 using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Mulligan : MonoBehaviour
{
    //현재 해상도를 좌우로 분할해주는 스크립트
    SearchWhereSide SideFinder;
    CheckInPosition CheckPosition;
    Rect StartBtn;
    Vector2 BtnPos;

    public GameObject[] Mulligan_front_face;
    List<GameObject> Mulligan_front_face_list = new List<GameObject>(0);
    public GameObject[] Mulligan_choice_effect;
    //GameObject[] MulliganCards;
    public Button button;
    public Text T_button;
    short isleft;
    bool isTouch;
    

    void Start()
    {
        //컴포넌트 부분은 해당 오브젝트가 해당 컴포넌트를 포함하지 않을 경우 에러가 발생할 수 있음.
        //따라서 try catch로 다른 파트에서 해당 컴포넌트 없이 사용하더라도 에러가 나지 않도록 설정.
        //이는 해당 컴포넌트가 오브젝트에 없더라도 부분적으로 기능을 수행할 수 있다.
        try
        {
            isTouch = true;
            //현재 해상도를 이용해 화면을 각각 좌우로 분할해주는 스크립트.
            SideFinder = GetComponent<SearchWhereSide>();
            CheckPosition = GameObject.Find("ProgrammerDefineMethods").GetComponent<CheckInPosition>();
            StartBtn = button.gameObject.GetComponent<RectTransform>().rect;

            SetBtnSizeWithScreen();
        }
        catch (Exception ex)
        {
            Debug.Log(ex);
        }
        

        Init(); //멀리건 이미지 생성
        Mulligan_front_face_list.AddRange(Mulligan_front_face);

    
    }

 
    void Update()
    {
        

        //터치가 입력된 경우
        if ((Input.GetMouseButtonDown(0)||Input.touchCount>0)&&isTouch)
        {
            if (CheckPosition.TouchToRect(StartBtn, Input.mousePosition)) ;

            else if (CheckPosition.TouchToRect(SideFinder.LeftSideScreen, Input.mousePosition))
            //if(CheckPosition.TouchToRect(SideFinder.LeftSideScreen, Input.GetTouch(0).position))
            {
                for (int i = 0; i < Mulligan_front_face.Length; i++)
                {
                    if (i < 5)
                        Mulligan_choice_effect[i].SetActive(true);

                    else
                        Mulligan_choice_effect[i].SetActive(false);

                }

                isleft = 1;
            }

            else
            {
                //Debug.Log("오른쪽 찾음");
                for (int i = 0; i < 10; i++)
                {
                    if (i < 5)
                        Mulligan_choice_effect[i].SetActive(false);

                    else
                        Mulligan_choice_effect[i].SetActive(true);
                }

                isleft = 2;

            }

            
        }
    }

    public void SetBtnSizeWithScreen() //버튼 사이즈를 스크린 좌표로 설정
    {
        BtnPos = button.gameObject.transform.localPosition;
        StartBtn.x = BtnPos.x + (Screen.width / 2) - (StartBtn.width / 2);
        StartBtn.y = BtnPos.y + (Screen.height / 2) - (StartBtn.height / 2);
        StartBtn.width = BtnPos.x + (Screen.width / 2) + (StartBtn.width / 2);
        StartBtn.height = BtnPos.y + (Screen.height / 2) + (StartBtn.height / 2);
    }

    void Init() //멀리건 이미지 생성
    {
        for (int i = 0; i < Mulligan_front_face.Length; i++)
        {
            Mulligan_front_face[i].GetComponent<Image>().sprite =
                UnitInventory.Instance.unitL[UnityEngine.Random.Range(0, UnitInventory.Instance.unitL.Count)].
                GetComponent<SpriteRenderer>().sprite;
        }
    }

    public void MulliganChoice()
    {
        

        if (isleft == 1 || isleft == 2)
        {
            isTouch = false;
            button.enabled = false;
            T_button.text = "상대 대기중";
            if(isleft == 1)
                BattleCardManagers.Instance.MulliganFinish(Mulligan_front_face_list.GetRange(0,5));
            else
                BattleCardManagers.Instance.MulliganFinish(Mulligan_front_face_list.GetRange(5, 5));
        }
            

        else
            return;

        
    }

}
