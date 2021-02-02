using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSortingLayer : MonoBehaviour
{
    private UnitAI m_unitAI;
    private SpriteRenderer sprite;
    private WaitForSeconds ws;
    private int sortingNum;

    // Start is called before the first frame update
    void Start()
    {
        m_unitAI = GetComponent<UnitAI>();
        sprite = GetComponent<SpriteRenderer>();
        ws = new WaitForSeconds(0.3f);

        StartCoroutine(CheckLayer());
    }

    IEnumerator CheckLayer()
    {
        while (!m_unitAI.isDie)
        {
            sortingNum = (int)(-transform.position.y * 100) / 10 + 50;
            sprite.sortingOrder = sortingNum;
            yield return ws;
        }

    }
}
