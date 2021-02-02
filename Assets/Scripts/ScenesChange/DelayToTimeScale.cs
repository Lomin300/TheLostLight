using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayToTimeScale : MonoBehaviour
{
    public float Timer;
    public int PauseOrResume; //0 = Pause, 1 = Resume;


    // Update is called once per frame
    void Update()
    {
        Timer -= Time.deltaTime;

        if(Timer<=0)
        {
            Time.timeScale = PauseOrResume;
            Destroy(this);
        }
    }
}
