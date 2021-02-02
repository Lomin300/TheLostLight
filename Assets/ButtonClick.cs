using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class ButtonClick : MonoBehaviour,IPointerDownHandler,IPointerUpHandler
{
    public Spawn m_Spawn;
    public int num;
    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
        m_Spawn.TakeClick(num);
    }

    void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
    {
        m_Spawn.TakeClickUp();
    }
}
