using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.UI;

public class MulliganAnimation : MonoBehaviour
{
    public float DelayTime;
    Animator animator;
    public GameObject firstchild;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        StartCoroutine(AnimationDelayCoroutine());
        
    }

    IEnumerator AnimationDelayCoroutine()
    {
        yield return new WaitForSeconds(DelayTime);
        animator.SetBool("IsDelay", true);
        //Destroy(this);
        StartCoroutine(CardImageCoroutine());

    }

    IEnumerator CardImageCoroutine()
    {
        yield return new WaitForSeconds(2f);
        firstchild.GetComponent<Image>().color = Color.white;
    }
}
