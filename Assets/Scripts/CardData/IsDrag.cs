using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class IsDrag : MonoBehaviour
{
    Vector2 defaultPosition;
    Vector2 mPos;
    Vector2 screenPos;
    public GameObject dummy;
    bool isClick;
    bool isObjectClick;

    

    private void Start()
    {
        defaultPosition = dummy.transform.localPosition;
        screenPos.x = Screen.width / 2;
        screenPos.y = Screen.height / 2;
    }

    private void Update()
    {
        
        if (Input.GetMouseButton(0) && isObjectClick)
        {
            isClick = true;
            
        }
            
            

        else
            isClick = false;

        if (isClick)
        {
            //Debug.Log("드래그 오브젝트 클릭됨");
            mPos = Input.mousePosition;
            dummy.transform.localPosition = mPos - screenPos;

        }


    }

    public void Objectclick(Sprite sprite)
    {
        isObjectClick = true;
        dummy.GetComponent<Image>().sprite = sprite;
        dummy.GetComponent<Image>().SetNativeSize();
        dummy.GetComponent<Image>().color = new Color(1f, 1, 1f, 0.3f);
    }

    public void Objectclickup()
    {
        isObjectClick = false;
        dummy.GetComponent<Image>().color = new Color(1, 1, 1, 0); //투명
        dummy.transform.localPosition = defaultPosition;
    }


}
