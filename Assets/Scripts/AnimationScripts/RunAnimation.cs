using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunAnimation : MonoBehaviour
{
    public Animator animator;

    private void Start()
    {
        //animator = GetComponent<Animator>();
    }

    public void Ani_Stop()
    {
        
    }
    public void Ani_Idle()
    {
        animator.SetBool("isCreated", true);
        animator.SetBool("isIdle", true);
        animator.SetBool("isMove", false);
    }
    public void Ani_Move()
    {
        animator.SetBool("isIdle", false);
        animator.SetBool("isMove", true);
    }
    public void Ani_Attack()
    {
        animator.SetTrigger("isAttackTrigger");
    }

    public void Ani_Skiil()
    {
        //animator.SetInteger("AniState", 4);
    }

    public void Ani_Damaged()
    {
        animator.SetTrigger("isDamagedTrigger");
    }
}
