using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwoRendomDelayAnimation : MonoBehaviour
{
    public int min;
    public int max;
    public Animator animator;

    float delay;

    private void Start()
    {
        SetRandom();
    }

    void FixedUpdate()
    {
        delay -= Time.deltaTime;

        if(delay<=0)
        {
            animator.SetTrigger("Active");
            SetRandom();
        }

    }

    void SetRandom()
    {
        delay = Random.Range(min, max);
    }
}
