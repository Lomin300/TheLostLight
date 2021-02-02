using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;





public class BattleCardManagers : MonoBehaviour
{

    //__________________________________________________________________________________
    //                              <카드 속성 정의>
    public List<string> Cardname = new List<string>();
    //현재 덱에서 무작위로 카드를 10장 섞인 카드를 담고 있는 리스트
    public GameObject[] AllCard;
    //엔진에서 넣어주는 핸드 카드 6개의 오브젝트를 담고 있음.
    public List<GameObject> AllCardList = new List<GameObject>();
    //엔진에서 넣어주는 핸드 카드를 리스트로 담아줄 예정
    public List<GameObject> AllFrontFaceList = new List<GameObject>();
    //핸드 카드 6개의 앞면 이미지를 리스트로 담고 있음.
    public List<GameObject> AllCostTextList = new List<GameObject>();
    //핸드 카드 6개의 코스트 텍스트를 리스트로 담고 있음.
    public List<GameObject> UndrawCardList = new List<GameObject>();
    public List<GameObject> handUnitList = new List<GameObject>();
    public GameObject UnitObjectsField;
    //내 핸드에 할당된 유닛 오브젝트
    //__________________________________________________________________________________

    //__________________________________________________________________________________
    //                              <필드 오브젝트 정의>
    public Canvas battleObjectCanvas;
    public List<GameObject> AllUnitObjectList = new List<GameObject>();
    public int inCrease=12; //초당 수입
    public int firefly=0; //자원 
    public bool isActive;
    float deltaSec;
    //__________________________________________________________________________________


    private static BattleCardManagers _instance = null;
    //싱글턴

    public static BattleCardManagers Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType(typeof(BattleCardManagers)) as BattleCardManagers;

                if (_instance == null)
                {
                    Debug.LogError("There's no active ManagerClass object");
                }
            }
            
            return _instance;
        }
    }

    // Start is called before the first frame update
    void Awake()
    {
        AllCardList.AddRange(AllCard);
        isActive = false;
        for (int i = 0; i < AllCardList.Count; i++)
        {
            
            AllFrontFaceList.Add(AllCardList[i].transform.GetChild(0).gameObject); //프론트 페이스
            AllCostTextList.Add(AllCardList[i].transform.GetChild(1).gameObject); //자원 텍스트
        }

    }
    
    // Update is called once per frame
    void FixedUpdate()
    {
        ActiveCards();
        deltaSec += Time.deltaTime;
        if (deltaSec >= 1)
        {
            firefly = firefly + inCrease;
            deltaSec = 0;
        }

        Debug.Log(Input.mousePosition);

        //카드 드로우 타이머를 돌림
        //카드 언드로우 리스트
        //카드 핸드 리스트
        //랜덤 레인지로 값 뽑아서 핸드로 추가
        //카드 사용하면 언드로우 리스트에 추가하고
        //핸드 리스트에서 제거

        //드로우 애니메이션
        //첫 카드 드로우 좌표 x + 120
        //사용하면 리스트에서 remove()하고 이미지 딜레이(추가) 갱신
        //카드 드로우는 랜덤으로 사용
    }

    public void MulliganFinish(List<GameObject> mulligan_front_face) //멀리건이 끝나면
    {
        isActive = true;
        //HandCardInfo cardinfo = new HandCardInfo();
        //HandCardInfo cardinfo2 = new HandCardInfo();  

        //cardinfo.imagename = UnitInventory.Instance.UserCards[0].charname;
        //cardinfo.cost = UnitInventory.Instance.UserCards[0].cost;
        //cardinfo2.imagename = UnitInventory.Instance.UserCards[1].charname;
        //cardinfo2.cost = UnitInventory.Instance.UserCards[1].cost;
        //PKT_REQMULLIGANFINISH mulligan = new PKT_REQMULLIGANFINISH();
        //mulligan.Init();
        //string message;
        for (int i = 0; i < mulligan_front_face.Count; i++)
        {
            AllFrontFaceList[i].GetComponent<Image>().sprite = mulligan_front_face[i].GetComponent<Image>().sprite;

            for(int j=0; j<UnitInventory.Instance.unitL.Count; j++)
            {
                if(UnitInventory.Instance.unitL[j].GetComponent<SpriteRenderer>().sprite == AllFrontFaceList[i].GetComponent<Image>().sprite)
                { //이미지 이름이 같다면 = 오브젝트를 찾았다면!
                    handUnitList.Add(UnitInventory.Instance.unitL[j]);
                    //핸드 유닛 리스트에 찾은 유닛 오브젝트를 add해주고
                    AllCostTextList[i].GetComponent<TextMeshProUGUI>().text = UnitInventory.Instance.unitL[j].GetComponent<UnitState>().cost.ToString();
                    //핸드 카드 코스트 텍스트에 해당 유닛이 가진 코스트를 입력해준다.
                    break;
                }
            }
        }
            

        /*message = JsonUtility.ToJson(mulligan);
        mulligan.nSize = message.Length;
        message = JsonUtility.ToJson(mulligan);
        NetManager.instance.Send(message);*/
    }

    public Rect PosWithRect(GameObject button)//사각형 크기를 중점 피벗을 기준으로 화면 좌표에 맞춰준다.
    {
        Rect tmpRect = button.GetComponent<RectTransform>().rect;
        Vector2 tmpPos = button.transform.localPosition;

        tmpRect.x = tmpPos.x + (Screen.width / 2) - (tmpRect.width / 2);
        tmpRect.y = tmpPos.y + (Screen.height / 2) - (tmpRect.height / 2);
        tmpRect.width = tmpPos.x + (Screen.width / 2) + (tmpRect.width / 2);
        tmpRect.height = tmpPos.y + (Screen.height / 2) + (tmpRect.height / 2);

        return tmpRect;
    }

    public Vector2 PosWithVector(Vector2 button)
    {
        Vector2 tempVec = button;

        tempVec.x -= Screen.width / 2;
        tempVec.y -= Screen.height / 2;

        return tempVec;
    }

    public void ActiveCards()//손에 있는 카드 활성화 및 비활성화
    {
        if (isActive)
        {
            for (int i = 0; i < AllCardList.Count; i++)
            {
                if (i < handUnitList.Count) //전체 카드 중에 내 핸드 카드의 개수만큼 활성화
                {
                    AllCardList[i].SetActive(true);
                }

                else //내 핸드카드보다 초과된 카드들은 비활성화
                {
                    AllCardList[i].SetActive(false);
                }
            }
        }
        
    }

    public void SummonUnit(int i,string team) //유닛 소환
    {
        

        if (team.Equals("Player"))
        {
            UnitState tempState;
            tempState = handUnitList[i].GetComponent<UnitState>();

            if (firefly >= tempState.cost)
            {
                firefly -= tempState.cost;

                handUnitList[i].layer = (int)layers.Player;
                handUnitList[i].GetComponent<MoveAndSearch>().attackTargetLayerName = "Enemy";
                Vector3 tempPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                tempPos.z = 0;
                GameObject tempObject = Instantiate(handUnitList[i],tempPos
                            , Quaternion.identity) as GameObject;
                tempObject.transform.parent = UnitObjectsField.transform;
                
                tempObject.GetComponent<UnitState>().enabled = true;
                tempObject.GetComponent<MoveAndSearch>().enabled = true;

                AllUnitObjectList.Add(tempObject);
            }
            //////백정욱
            
            /*
            PKT_REQSUMMONSPAWN SendPkt = new PKT_REQSUMMONSPAWN();
            SendPkt.Init();

            SendPkt.name = MyHandCardList[i].name;
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.x=Mathf.Round(mousePosition.x * 10) * 0.1f;
            mousePosition.y = Mathf.Round(mousePosition.y * 10) * 0.1f;

            SendPkt.x = (int)mousePosition.x * 100;
            SendPkt.y = (int)mousePosition.y * 100;
            string message = JsonUtility.ToJson(SendPkt);
            SendPkt.nSize = message.Length;
            message = JsonUtility.ToJson(SendPkt);
            NetManager.instance.Send(message);*/
        }

        else if(team.Equals("Enemy"))
        {
            UnitInventory.Instance.unitL[i].layer = (int)layers.Enemy;
            UnitInventory.Instance.unitL[i].GetComponent<MoveAndSearch>().attackTargetLayerName = "Player";

            Vector3 tempPos = new Vector3(5, Random.Range(-1.5f, 1.5f), 0);
            Quaternion tempQuaternion = Quaternion.identity;

            GameObject tempObject = Instantiate(UnitInventory.Instance.unitL[i], tempPos
                        , tempQuaternion) as GameObject;
            tempObject.transform.localScale = new Vector3(-1, 1, 1);
            tempObject.transform.parent = UnitObjectsField.transform;
            tempObject.GetComponent<UnitState>().enabled = true;
            tempObject.GetComponent<MoveAndSearch>().enabled = true;

            AllUnitObjectList.Add(tempObject);
        }
        
    }

    

    private void OnDestroy()
    {
        Destroy(_instance);
    }
}

