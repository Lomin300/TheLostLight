using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NextCard : MonoBehaviour
{

    public Image unit_Img;
    public Image time_Img;
    public int cardNum;
    public int cost;
    public bool isStart;
    public bool isSpawn;
    public ResourceManager m_resource;


    private void Update()
    {
        if (isStart)
        {
            if(m_resource.money >= cost)
            {
                time_Img.fillAmount = 0;
                isSpawn = true;
                Debug.Log("필어마운트");
            }
            else
            {
                time_Img.fillAmount = 1;
                isSpawn = false;
            }
        }
    }
}
