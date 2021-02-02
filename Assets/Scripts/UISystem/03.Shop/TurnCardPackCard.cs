using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnCardPackCard : MonoBehaviour
{
    public Animator animator;
    public AnimationClip turnClip;
    float animationTime;

    bool isTurnning;

    private void Update()
    {
        if(isTurnning)
        {
            animationTime += Time.deltaTime;
            if (animationTime >= turnClip.length / 2)
            {
                transform.GetChild(0).gameObject.SetActive(true);
                isTurnning = false;
                animationTime = 0;
            }
                
        }
    }

    public void Turnnig()
    {
        Debug.Log("회전 실행!!!");
        animator.SetBool("IsTurn", true);

        isTurnning = true;
    }

}
