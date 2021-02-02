using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Drop_Inventory : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        if(transform.childCount == 0 && Drag_Inventory.draggingItem!=null)
        {
            Image objectImg = Drag_Inventory.draggingItem.transform.GetChild(0).GetComponent<Image>();

            objectImg.SetNativeSize();
            objectImg.transform.localScale = new Vector3(0.5f, 0.5f, 0);

            UnitInventory.Instance.AddDeck(UnitInventory.Instance.FindGameObjectSR(objectImg.sprite));
            

            Drag_Inventory.draggingItem.transform.SetParent(transform);
        }
            
    }

}
