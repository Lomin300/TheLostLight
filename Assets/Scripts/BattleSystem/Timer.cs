using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public Text TimerText;
    public float TimeLimit=0;
    float time;
    public Mulligan mulligan;
    public GameObject inGameTimer;

    // Start is called before the first frame update
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
            mulligan.MulliganChoice();
            inGameTimer.SetActive(true);
            SelectDestroy.instance.DestroySelectObject();
            time = 0;
            Debug.Log("멀리건 타이머 들어옴!!");
        }
            
        TimerText.text = time.ToString();
    }
}
