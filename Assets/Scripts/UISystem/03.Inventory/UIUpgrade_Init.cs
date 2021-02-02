using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIUpgrade_Init : MonoBehaviour
{
    public Upgrade_Profile profileFinder;
    public Text unitNameText;
    public Text profileText;
    public GameObject profileList;
    public Image unitImg;
    public GameObject stateBar;
    public Text lvText;
    public Image fillexpBar;

    UnitStat state;
    static GameObject ClickedCardObject = null;

    static bool isInit = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(ClickedCardObject && !isInit)
        {
            state = ClickedCardObject.GetComponent<UnitAI>().m_unitStat;
            Init();
            isInit = true;
            Debug.Log("Init 호출됨. 한번만 호출되야 정상인 텍스트입니다.");
        }
    }


    public static void Init(GameObject unit)
    {
        ClickedCardObject = unit;
    }

    public static void Guillotine()
    {
        ClickedCardObject = null;
        isInit = false;
    }

    public void Init()
    {
        SetNameText();
        SetProfileText();
        SetCharImg();
        SetStateText();
        SetLvText();
        SetExpBar();
    }

    public void SetNameText()
    {
        unitNameText.text = state.textName;
    }
    
    public void SetProfileText()
    {
        GameObject prefab;
        Text listText;

        for(int i=0; i<profileList.transform.childCount; i++)
        {
            prefab = profileList.transform.GetChild(i).GetComponent<RememberPrefab>().Prefab;
            listText = profileList.transform.GetChild(i).GetComponent<Text>();

            if(ClickedCardObject == prefab)
            {
                profileText.text = listText.text;
            }
        }


        //profileText.text = profileFinder.FindProfile(state.textName);




    }

    public void SetCharImg()
    {
        unitImg.sprite = ClickedCardObject.GetComponent<SpriteRenderer>().sprite;
        unitImg.SetNativeSize();
        unitImg.transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
    }



    public void SetStateText()
    {
        stateBar.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = state.hp.ToString(); //Health
        stateBar.transform.GetChild(1).GetChild(0).GetComponent<Text>().text = state.manaStack.ToString(); //Health
        stateBar.transform.GetChild(2).GetChild(0).GetComponent<Text>().text = state.damage.ToString(); //Health
        stateBar.transform.GetChild(3).GetChild(0).GetComponent<Text>().text = state.attackSpeed.ToString(); //Health
        stateBar.transform.GetChild(4).GetChild(0).GetComponent<Text>().text = state.attackRange.ToString(); //Health
        stateBar.transform.GetChild(5).GetChild(0).GetComponent<Text>().text = state.skillDamage.ToString(); //Health
        stateBar.transform.GetChild(6).GetChild(0).GetComponent<Text>().text = state.speed.ToString(); //Health
        
    }

    public void SetLvText()
    {
        int lv;
        lv = state.exp / 100;
        lvText.text = lv.ToString();
    }
        

    public void SetExpBar()
    {
        float tExp;
        tExp = state.exp % 100;
        fillexpBar.fillAmount = tExp / 100;
    }
}
