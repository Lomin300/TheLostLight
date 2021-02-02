using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class DefalutTimer : MonoBehaviour
{
    public Text time;
    float timer = 0;
    public float TotalTimer;
    int min = 0;
    public bool isFlag;
    public bool isStop=false;
    private InGameManager m_InGame;
    private void Start()
    {
        m_InGame = GetComponent<InGameManager>();
        min = (int)TotalTimer / 60;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isFlag)
        {
            timer += Time.deltaTime;
            if (timer >= 60)
            {
                min++;
                timer -= 60;
            }
        }
        else if(!isStop)
        {
           
                TotalTimer -= Time.deltaTime;
                min = (int)TotalTimer / 60;
                timer = TotalTimer - (min * 60);
            if (TotalTimer <= 0)
            {
                time.text = "";
                isStop = true;
                m_InGame.go.GetComponent<UnitAI>().isHero = false;
            }
            else if (TotalTimer <= 30)
             {
                    time.color = new Color(1, 0.1f, 0.1f, 1);
             }
               
            
        }

        if(!isStop)
        time.text = string.Format("{0:D2} : {1:D2}", min, (int)timer);

    }

    private void OnDisable()
    {
        time.text = "00 : 00";
        timer = 0;
        min = 0;
    }
}
