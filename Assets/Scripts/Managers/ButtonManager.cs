using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

enum BtnID
{
    HandCard = 0
}

public class ButtonManager : MonoBehaviour
{
    CheckInPosition checkInPosition;

    public GameObject[] btnHandCard;
    List<GameObject> btnHandCardList = new List<GameObject>();
    IsDrag isDrag;
    bool currentBtn;

    int findBtn;
    /// <summary>
    /// 어떤 버튼이 눌렸는지에 대한 정의
    /// -1 눌린 버튼 없음
    /// 0번 핸드 카드 0번
    /// 9번 핸드 카드 9번
    /// 
    /// </summary>
    

    void Start()
    {
        checkInPosition = GameObject.Find("ProgrammerDefineMethods").GetComponent<CheckInPosition>();
        isDrag = GameObject.Find("ProgrammerDefineMethods").GetComponent<IsDrag>();
        btnHandCardList.AddRange(btnHandCard);
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButton(0))
        {
            if(currentBtn==false)
                findBtn = HandCardButtons();
            currentBtn = true;
            //Debug.Log("클릭중");
        }

        else if(Input.GetMouseButtonUp(0))
        {
            //마우스를 클릭했다가 뗀 경우
            if(currentBtn&&BattleCardManagers.Instance.isActive)
            {
                BtnSwitchCase();
                findBtn = -1;
            }
            //그냥 안누르고 있는 경우
            else
            {

            }
            currentBtn = false;
        }
    }

    public bool ObjectClickCheck(GameObject gameobject)
    {
        Rect objectScreenRect = BattleCardManagers.Instance.PosWithRect(gameobject);
        //Debug.Log(objectScreenRect);
        bool isIn = checkInPosition.TouchToRect(objectScreenRect, (Vector2)Input.mousePosition);

        return isIn;
    }

    public int HandCardButtons()
    {
        for (int i = 0; i < btnHandCardList.Count; i++)
        {
            if (ObjectClickCheck(btnHandCardList[i])==true)
            {
                isDrag.Objectclick(BattleCardManagers.Instance.AllFrontFaceList[i].GetComponent<Image>().sprite);

                return i;
            }

            else
            {
                //Debug.Log("버튼 범위에 안들어옴");
            }
        }

        return -1;
    }
    
    public void BtnSwitchCase()
    {
        if(findBtn >= 0 && findBtn <= btnHandCardList.Count)
        {
            Debug.Log(findBtn + "버튼누름");
            isDrag.Objectclickup();
            BattleCardManagers.Instance.SummonUnit(findBtn,"Player");
            //GameObject tempObject = Instantiate(unitL, new Vector3(SpawnPos.x, transform.position.y, SpawnPos.z), unitL.transform.rotation);
        }
    }
}
