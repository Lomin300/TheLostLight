using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardPackClick : MonoBehaviour
{
    public Transform cardPackHeadtf; //카드팩 머리
    public AnimationClip openAni; //여는 애니메이션 클립 -> 속도를 받아와서 코르틴으로 지연시킬때 사용
    public GameObject cardPack; //카드팩 오브젝트
    public GameObject ui; //상점 기본적인 UI. 카드팩을 만지는 도중엔 UI를 건드릴 수 없도록 하기 위해 사용
    public GameObject popUpUI; //카드팩 오픈에 관한 UI
    public GameObject suggestCardPackUI; //카드팩을 여시겠습니까? 물어보는 UI틀
    public GameObject backgroundPanel; //외부 배경을 검은색으로 처리하는 판넬
    public GameObject effectPanel; //오픈하는 이펙트 판넬
    public GameObject btn_OK; //카드팩 보상 확인 OK 버튼
    public GameObject cardObjectParent; //오픈으로 등장하는 카드 5장을 담고 있는 오브젝트
    public List<Animator> unitcardAni = new List<Animator>();
    public List<GameObject> unitCards = new List<GameObject>();
    public List<GameObject> heroCards = new List<GameObject>();

    public CardPackDB unitLottey;
    public CardPackDB heroLottey;
    
    public static bool isOpenning = false;

    Animator animator;

    private void Start()
    {

        animator = cardPackHeadtf.gameObject.GetComponent<Animator>();
    }

    public static bool IsOpen()
    {

        if (isOpenning)
            return true;
        else
            return false;
    }

    public void SuggestCardPack() //카드팩 확인 팝업
    {
        ui.SetActive(false);
        popUpUI.SetActive(true);
        suggestCardPackUI.SetActive(true);
        backgroundPanel.SetActive(true);
    }

    public void NewCardPack() //카드팩 생성!
    {
        if(UnitInventory.Instance.rube>=300)
        {
            Debug.Log("카드팩 생성!");

            suggestCardPackUI.SetActive(false);
            cardPack.SetActive(true);

            isOpenning = true;
        }
        
    }

    public void CancelSuggestCardPack() //카드팩 확인 팝업 이후 구매 캔슬
    {
        ui.SetActive(true);
        popUpUI.SetActive(false);
        suggestCardPackUI.SetActive(false);
        backgroundPanel.SetActive(false);
    }

    public void OpenCardPack() // 카드팩 오픈
    {
        animator.SetTrigger("Open");
        for (int i = 0; i < unitcardAni.Count; i++)
        {
            unitcardAni[i].gameObject.SetActive(true);
        }
        StartCoroutine(WaitForAnimation());
    }

    IEnumerator WaitForAnimation() //카드팩 오픈 > 애니메이션 이후 처리
    {
        yield return new WaitForSeconds(openAni.length);
        effectPanel.SetActive(true);
        cardPack.SetActive(false);
        btn_OK.SetActive(true);
        cardObjectParent.SetActive(true);

    }

    public void EndCardPack() //카드팩 종료
    {
        isOpenning = false;

        for (int i = 0; i < unitCards.Count; i++)
        {
            unitcardAni[i].SetBool("IsTurn", false);
            unitCards[i].transform.GetChild(0).gameObject.SetActive(false);
            unitCards[i].gameObject.SetActive(false);
            Debug.Log("Card");
        }

        ui.SetActive(true);
        btn_OK.SetActive(false);
        backgroundPanel.SetActive(false);
        
        
        effectPanel.SetActive(false);
        Debug.Log("End");

        
        cardObjectParent.SetActive(false);

        popUpUI.SetActive(false);
        Debug.Log("Pack");
    }

    public void SetUnitRandomBox()
    {
        GameObject foundObject;
        Image cardCharImg;

        UnitInventory.Instance.rube -= 300;
        for (int i = 0; i < unitCards.Count; i++)
        {
            foundObject = PickRandomUnitObject();

            if (foundObject != null)
            {
                UnitInventory.Instance.AddInventory(foundObject, (int)CATEGORY.Unit);

                cardCharImg = unitCards[i].transform.GetChild(0).GetComponent<Image>();
                cardCharImg.sprite = foundObject.GetComponent<SpriteRenderer>().sprite;
                cardCharImg.SetNativeSize();
                cardCharImg.transform.localScale = new Vector3(0.7f, 0.7f, 1);
            }
            else
                Debug.Log("PickRandomUnitObject()에서 null 값이 넘어옴. 확인요망");
        }
            


    }

    public void SetHeroRandomBox()
    {
        GameObject foundObject;
        Image cardCharImg;


        UnitInventory.Instance.rube -= 300;
        for (int i = 0; i < heroCards.Count; i++)
        {
            foundObject = PickRandomHeroObject();

            if (foundObject != null)
            {
                UnitInventory.Instance.AddInventory(foundObject, (int)CATEGORY.Hero);

                cardCharImg = heroCards[i].transform.GetChild(0).GetComponent<Image>();
                cardCharImg.sprite = foundObject.GetComponent<SpriteRenderer>().sprite;
                cardCharImg.SetNativeSize();
            }
            else
                Debug.Log("PickRandomHeroObject()에서 null 값이 넘어옴. 확인요망");
        }
    }

    private GameObject PickRandomUnitObject() // DB에 있는 추첨권 개수에 따라 랜덤으로 오브젝트 하나 추출
    {
        int allTicket = 0;

        for (int i = 0; i < unitLottey.unitPrefab.Count; i++)
        {
            allTicket += unitLottey.chance[i];
        }

        int randomEncounter = Random.Range(0, allTicket);

        for (int i = 0; i < unitLottey.unitPrefab.Count; i++)
        {
            if (randomEncounter >= unitLottey.chance[i])
            {
                randomEncounter -= unitLottey.chance[i];
            }
            else
                return unitLottey.unitPrefab[i];

        }

        return null;
    }

    private GameObject PickRandomHeroObject() // DB에 있는 추첨권 개수에 따라 랜덤으로 오브젝트 하나 추출
    {
        int allTicket = 0;

        for (int i = 0; i < heroLottey.unitPrefab.Count; i++)
        {
            allTicket += heroLottey.chance[i];
        }

        int randomEncounter = Random.Range(0, allTicket);

        for (int i = 0; i < heroLottey.unitPrefab.Count; i++)
        {
            if (randomEncounter >= heroLottey.chance[i])
            {
                randomEncounter -= heroLottey.chance[i];
            }
            else
                return heroLottey.unitPrefab[i];

        }

        return null;
    }


}
