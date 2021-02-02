using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Drag_Volume : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    Vector3 originPos, pos;
    public Image barImage;
    public Transform buttonTF;
    Vector2 newBtnPos;

    Vector3 endBtnPos;

    

    // Start is called before the first frame update
    void Awake()
    {
        //barImage = GetComponent<Image>();
        originPos = Camera.main.WorldToScreenPoint(transform.position);
        pos.x = originPos.x - 200;
        endBtnPos = transform.GetChild(0).GetChild(0).position;
        //pos.x = GetComponent<RectTransform>().rect.width;
        //pos.x = originPos.x - transform.GetChild(1).position.x;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(Input.mousePosition);
        //Debug.Log(transform.position);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        CalculatefillAmount();
    }

    public void OnDrag(PointerEventData eventData)
    {
        //pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        CalculatefillAmount();

        //lpos.Set(transform.localPosition.x, transform.localPosition.y, 0);
        //transform.localPosition = lpos; // 로컬 z값을 0으로 안만들면 카메라 앞에 설치됨.
        //왜 그런지 모르겠음
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        CalculatefillAmount();
    }

    void CalculatefillAmount()
    {
        string debug;
        
        
        //barImage.fillAmount = (Camera.main.ScreenToWorldPoint(Input.mousePosition).x - pos.x) / (originPos.x - pos.x);
        barImage.fillAmount = (Input.mousePosition.x - pos.x) / (originPos.x - pos.x);
        newBtnPos.x = Camera.main.ScreenToWorldPoint(Input.mousePosition).x;
        //newBtnPos.x = Input.mousePosition.x;
        newBtnPos.y = buttonTF.position.y;

        debug = string.Format("endBtnPos.x : {0}, newBtnPos.x : {1} ", endBtnPos.x, newBtnPos.x);
        Debug.Log(debug);

        if (endBtnPos.x < newBtnPos.x)
            buttonTF.position = endBtnPos;
        else
            buttonTF.position = newBtnPos;

        if (barImage.fillAmount > 1)
            AudioListener.volume = 1;
        else if (barImage.fillAmount < 0)
            AudioListener.volume = 0;
        else
            AudioListener.volume = barImage.fillAmount;

    }

    public void ClickfillAmount()
    {
        if(barImage.fillAmount>0)
        {
            AudioListener.volume = barImage.fillAmount = 0;
        }

        else
        {
            AudioListener.volume = barImage.fillAmount = 1;
        }
    }
    
}
