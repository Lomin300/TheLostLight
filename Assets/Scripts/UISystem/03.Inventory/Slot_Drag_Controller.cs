using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slot_Drag_Controller : MonoBehaviour
{
    public GameObject upgradeUI;


    float clickTime, time;
    Rect size;

    // Start is called before the first frame update
    void Start()
    {
        upgradeUI = GameObject.Find("UIUpgrade");
    }

    // Update is called once per frame
    void Update()
    {
        /*
        time += Time.deltaTime;



        if (Input.GetMouseButtonDown(0))
        {
            Drag_Inventory.isDragging = false;
            //RaycastHit2D ray;
            //ray = Physics2D.Raycast(Input.mousePosition, Vector3.forward, 100, LayerMask.GetMask(""));

            clickTime = time;
        }


        if (Input.GetMouseButtonUp(0) && !Drag_Inventory.isDragging && IsRectIn())
        {
            if (clickTime <= time + 0.7)
            {
                if (!upgradeUI.transform.GetChild(0).gameObject.activeSelf)
                    upgradeUI.transform.GetChild(0).gameObject.SetActive(true); //0.UIBackSprite true
                else
                    upgradeUI.transform.GetChild(0).gameObject.SetActive(false); //0.UIBackSprite flase
            }
        }*/
    }

    //콜라이더를 넣는 방식으로 가자

    private void OnMouseDown()
    {
        Debug.Log("마우스 클릭됨");
        Drag_Inventory.isDragging = false;
        clickTime = time;
    }

    private void OnMouseUp()
    {
        Debug.Log("마우스 올라옴");
        if (clickTime <= time + 0.7 && !Drag_Inventory.isDragging)
        {
            if (!upgradeUI.transform.GetChild(1).gameObject.activeSelf) //열기
            {
                OpenUpgradeUI();
            }
                
            else //닫기
            {
                CloseUpgradUI();
            }
                
        }
    }



    public void OpenUpgradeUI()
    {
        upgradeUI.transform.GetChild(0).gameObject.SetActive(true); //백 판넬 true
        upgradeUI.transform.GetChild(1).gameObject.SetActive(true); //0.UIBackSprite true

        UIUpgrade_Init.Init(UnitInventory.Instance.FindGameObjectAll(transform.GetChild(0).GetComponent<Image>().sprite));
    }

    public void CloseUpgradUI()
    {
        upgradeUI.transform.GetChild(0).gameObject.SetActive(false); //백 판넬 false
        upgradeUI.transform.GetChild(1).gameObject.SetActive(false); //0.UIBackSprite flase
        UIUpgrade_Init.Guillotine();
    }


    string DebugString()
    {
        return string.Format("x : {0}, y : {1}, width : {2}, height : {3} mx : {4}, my : {5}", size.x, size.y, size.width, size.height, Input.mousePosition.x, Input.mousePosition.y);
    }
}
