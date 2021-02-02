using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnyTimer : MonoBehaviour
{
    public Text time;
    float timer = 0;
    int min = 0;

    private void Start()
    {
        time = GetComponent<Text>();
    }
    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= 60)
        {
            min++;
            timer -= 60;
        }

        time.text = string.Format("{0:D2} : {1:D2}", min, (int)timer);

    }

    private void OnDisable()
    {
        time.text = "00 : 00";
        timer = 0;
        min = 0;
    }
}
