using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndGameTimer : MonoBehaviour
{
    public Text TimerText;
    public float TimeLimit = 0;
    float time;

    // Update is called once per frame
    void Start()
    {
        TimerText = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        TimeLimit -= Time.deltaTime;
        if (TimeLimit > 0)
            time = Mathf.Round(TimeLimit);

        else
        {
            BattleCardManagers.Instance.inCrease *= 2;
            for(int i=0; i<BattleCardManagers.Instance.AllUnitObjectList.Count; i++)
            {
                UnitState tempState = BattleCardManagers.Instance.AllUnitObjectList[i].GetComponent<UnitState>();

                tempState.MoveSpeed *= 2;
                tempState.AttackSpeed *= 0.5f;
                if(BattleCardManagers.Instance.AllUnitObjectList[i].tag.Equals("Hero"))
                {
                    tempState.MoveSpeed = 5;
                    tempState.Recognition = 15;
                    tempState.order.Remove("Unit");
                    tempState.order.Remove("Miner");
                }
                    
            }
            this.gameObject.SetActive(false);
        }

        TimerText.text = time.ToString();
    }
}
