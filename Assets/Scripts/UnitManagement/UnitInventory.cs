using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

enum CATEGORY
{
    Unit,
    Hero
}

enum DEVELOPER
{
    SLOTSIZE = 18
}

public class UnitInventory : MonoBehaviour
{
    //싱글턴 패턴
    public static UnitInventory Instance;

    public List<GameObject> unitL = new List<GameObject>();
    public List<GameObject> heroL = new List<GameObject>();

    public List<GameObject> invenUnitL = new List<GameObject>();
    public List<GameObject> invenHeroL = new List<GameObject>();

    public List<GameObject> allUnitL = new List<GameObject>();
    public List<GameObject> allHeroL = new List<GameObject>();

    public GameObject inventory;
    public GameObject Deck;
    public GameObject heroDeck;

    public int nowCategory;

    /// <summary>
    /// 자원에 대한 정의
    /// </summary>

    public int gold, rube;


    public GameObject leader; //대표 캐릭터 이미지
    public List<GameObject> leaders = new List<GameObject>();
    public GameObject illust;
    public GameObject nickNameText;


    // Start is called before the first frame update
    //public List<GAMECARDINFO> UserCardList = new List<GAMECARDINFO>();

    //public static UnitInventory Instance
    //{
    //    get
    //    {
    //        if (_instance == null)
    //        {
    //            _instance = FindObjectOfType(typeof(UnitInventory)) as UnitInventory;

    //            if (_instance == null)
    //            {
    //                Debug.LogError("There's no active ManagerClass object");
    //            }
    //        }

    //        return _instance;
    //    }
    //}

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this.gameObject);
        }

        else
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }
    private void Start()
    {
        

        invenUnitL.AddRange(allUnitL);
        invenHeroL.AddRange(allHeroL);
        UnitInventory.Instance.Inventory_Unit();

        gold = 1000;
        rube = 5100;
    }


    /*void Update()
    {
        
    }*/

    public void Deck_Show() //덱을 화면에 불러온다.
    {
        Vector3 tempV = new Vector3(0,0,0);
        GameObject tmpGO;
        GameObject justImgObject = GameObject.Find("TempSlot");
        

        for(int i=0; i<unitL.Count; i++)
        {
            if (Deck.transform.GetChild(i).childCount == 0)
            {
                justImgObject.transform.GetChild(0).GetComponent<Image>().sprite = unitL[i].GetComponent<SpriteRenderer>().sprite;

                tmpGO = Instantiate(justImgObject, tempV
                        , Quaternion.identity) as GameObject;

                tmpGO.transform.SetParent(Deck.transform.GetChild(i));
                tmpGO.transform.localScale = new Vector3(1, 1, 1);
                tmpGO.transform.GetChild(0).GetComponent<Image>().SetNativeSize();
            }
        }

        if(heroL.Count != 0)
        {
            justImgObject.transform.GetChild(0).GetComponent<Image>().sprite = heroL[0].GetComponent<SpriteRenderer>().sprite;

            tmpGO = Instantiate(justImgObject, tempV
                        , Quaternion.identity) as GameObject;

            tmpGO.transform.SetParent(heroDeck.transform);
            tmpGO.transform.localScale = new Vector3(1, 1, 1);
            tmpGO.transform.GetChild(0).GetComponent<Image>().SetNativeSize();
        }
    }

    public void Inventory_Unit() //유닛 탭 보여주기
    {
        if(inventory!=null)
        {
            Image objectImg;

            Inventory_Clear(); //기존 인벤토리 카드들을 false
            nowCategory = (int)CATEGORY.Unit; //현재 참조 중인 탭이 유닛이라고 알려줌



            for (int i = 0; i < invenUnitL.Count; i++) //인벤토리 유닛 카드 개수만큼 정보를 넣겠다
            {
                inventory.transform.GetChild(i).gameObject.SetActive(true);
                objectImg = inventory.transform.GetChild(i).GetChild(0).GetComponent<Image>();
                objectImg.sprite = invenUnitL[i].GetComponent<SpriteRenderer>().sprite;
                objectImg.SetNativeSize();
            }

            //for (int i = invenUnitL.Count; i < inventory.transform.childCount; i++) //정보가 들어가지 않은 슬롯들은 false 처리
            //{ //unitL.Count를 빼주는 이유? 덱으로 옮겨간 슬롯들 개수를 빼준다.
            //    inventory.transform.GetChild(i).gameObject.SetActive(false);
            //}
        }



    }

    public void Inventory_Hero() //영웅 탭 보여주기
    {
        if (inventory != null)
        {
            Image objectImg;

            Inventory_Clear();
            nowCategory = (int)CATEGORY.Hero;

            for (int i = 0; i < invenHeroL.Count; i++)
            {
                inventory.transform.GetChild(i).gameObject.SetActive(true);
                objectImg = inventory.transform.GetChild(i).GetChild(0).GetComponent<Image>();
                objectImg.sprite = invenHeroL[i].GetComponent<SpriteRenderer>().sprite;
                objectImg.SetNativeSize();
            }

            //for (int i = invenHeroL.Count; i < inventory.transform.childCount; i++)
            //{
            //    inventory.transform.GetChild(i).gameObject.SetActive(false);
            //}
        }
    }

    public void Inventory_Clear()
    {
            for (int i = 0; i < inventory.transform.childCount; i++)
            {
                inventory.transform.GetChild(i).gameObject.SetActive(false);
            }
        
    }

    

    public void AddDeck(GameObject tempObject)
    {
        if (tempObject == null)
            return;
        
            if (tempObject.tag == "Unit" || tempObject.tag == "Miner")
            {
                if(invenUnitL.Remove(tempObject))
                    unitL.Add(tempObject);
                
            }

            else if (tempObject.tag == "Hero")
            {
                if(invenHeroL.Remove(tempObject))
                    heroL.Add(tempObject);
            }
    }

    public void RemoveDeck(GameObject tempObject)
    {
        if (tempObject == null)
            return;

        if (tempObject.tag == "Unit" || tempObject.tag == "Miner")
        {
            if(unitL.Remove(tempObject))
                invenUnitL.Add(tempObject);
        }
        else if (tempObject.tag == "Hero")
        {
            if(heroL.Remove(tempObject))
                invenHeroL.Add(tempObject);
        }
    }
    
    public void AddInventory(GameObject tempObject, int typeCase)
    {
        UnitAI tmpState;



        switch(typeCase)
        {
            case (int)CATEGORY.Unit:
                for(int i=0; i<allUnitL.Count; i++)
                {
                    if(allUnitL[i] == tempObject)
                    {
                        tmpState = allUnitL[i].GetComponent<UnitAI>();
                        tmpState.m_unitStat.exp += 120;
                        tmpState.m_unitStat.SetLvUp();
                        return;
                    }
                }

                allUnitL.Add(tempObject);
                invenUnitL.Add(tempObject);
                return;

            case (int)CATEGORY.Hero:
                for (int i = 0; i < allHeroL.Count; i++)
                {
                    if (allHeroL[i] == tempObject)
                    {
                        tmpState = allHeroL[i].GetComponent<UnitAI>();
                        tmpState.m_unitStat.exp += 120;
                        tmpState.m_unitStat.SetLvUp();
                        return;
                    }
                }

                allHeroL.Add(tempObject);
                invenHeroL.Add(tempObject);
                return;

            default:
                Debug.Log("잘못된 typeCase 입력. CATEGORY 이용바람.");
                return;
        }
    }

    public GameObject FindGameObjectSR(Sprite tempSR) //스프라이트를 단서로 게임 오브젝트를 찾아준다.
    {
        if (UnitInventory.Instance.nowCategory == (int)CATEGORY.Unit)
            for (int i=0; i < allUnitL.Count; i++)
            {
                if(allUnitL[i].GetComponent<SpriteRenderer>().sprite == tempSR)
                    return allUnitL[i];
            }

        else if (UnitInventory.Instance.nowCategory == (int)CATEGORY.Hero)
            for (int i = 0; i < allHeroL.Count; i++)
            {
                if (allHeroL[i].GetComponent<SpriteRenderer>().sprite == tempSR)
                    return allHeroL[i];
            }


        return null;
    }

    public GameObject FindGameObjectAll(Sprite tempSR) //모든 스프라이트를 단서로 게임 오브젝트를 찾아준다.
    {
            for (int i = 0; i < allUnitL.Count; i++)
            {
                if (allUnitL[i].GetComponent<SpriteRenderer>().sprite == tempSR)
                    return allUnitL[i];
            }

            for (int i = 0; i < allHeroL.Count; i++)
            {
                if (allHeroL[i].GetComponent<SpriteRenderer>().sprite == tempSR)
                    return allHeroL[i];
            }


        return null;
    }

    public void SetLeader() //대표 캐릭터로 설정(illust가 할당되어 있는 상태여야함)
    {
        if (leader != null)
        {
            for (int i = 0; i < illust.transform.childCount; i++)
            {
                if (illust.transform.GetChild(i).gameObject.name == leader.name)
                    illust.transform.GetChild(i).gameObject.SetActive(true);
                else
                    illust.transform.GetChild(i).gameObject.SetActive(false);
            }
                
            //leader.SetActive(true);
            //leader = Instantiate(leader);
        }
    }

    public void SetNickname(GameObject nicknameObject)
    {
        nicknameObject.GetComponent<Text>().text = nickNameText.GetComponent<Text>().text;
    }
}
