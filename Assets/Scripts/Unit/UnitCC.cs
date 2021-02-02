using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitCC : Bolt.EntityBehaviour<ICustomUnitState>
{

    public UnitAI m_uintAI;
    public MoveAgent m_moveAgent;
    public bool isCor;
    private Coroutine coroutine;
    public void CCManager(int index,float time,float force)
    {
        
        if (isCor)
        {
            StopCoroutine(coroutine);
        }

        
        coroutine = StartCoroutine(DurationTimeCC(time));

        switch (index)
        {
            case (int)CCDef.KnockBack:
                StartCoroutine(StartKnockBack(force,time));       
                break;
            case (int)CCDef.Transition:
                break;
        }
    }

    IEnumerator DurationTimeCC(float time)
    {
        isCor = true;
        yield return new WaitForSeconds(time);
        m_moveAgent.agent.UpdatePosition();
        m_uintAI.isCC = false;
        m_moveAgent.agent.isCC = false;
        isCor = false;
    }

    private IEnumerator StartKnockBack(float force,float time)
    {
        gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(force, 0);
        yield return new WaitForSeconds(time-0.1f);
        gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;

    }
}
