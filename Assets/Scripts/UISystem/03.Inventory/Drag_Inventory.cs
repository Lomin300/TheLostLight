using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Drag_Inventory : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public Slot_Drag_Controller controller;
    Transform deckList;
    Transform slotListTr;
    CanvasGroup canvasGroup;

    Vector2 scrnToWldPos2D;
    public static GameObject draggingItem = null;
    public static bool isDragging;


    // Start is called before the first frame update
    void Start()
    {
        draggingItem = null;
        deckList = GameObject.Find("Inventory").GetComponent<Transform>();
        slotListTr = GameObject.Find("Content").GetComponent<Transform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("onDrag");
        scrnToWldPos2D = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = scrnToWldPos2D;
        isDragging = true;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("onBeginDrag");
        transform.SetParent(deckList);
        draggingItem = gameObject;

        canvasGroup.blocksRaycasts = false;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("onEndDrag");
        //transform.position = item_tsf;
        draggingItem = null;
        canvasGroup.blocksRaycasts = true;
        if (transform.parent == deckList)
        {

            UnitInventory.Instance.RemoveDeck(
                UnitInventory.Instance.FindGameObjectSR(
                    transform.GetChild(0).GetComponent<Image>().sprite));

            transform.SetParent(slotListTr);
        }
    }


}
