using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAttack : Bolt.EntityBehaviour<ICustomUnitState>
{

    public float nextAttack = 0.0f;
    public float AttackCoolTime = 1.0f;
    public float AttackJudmentTime;
    public int AttackDamage = 20;
    public int manaStack;
    public bool isSkill;
    public UnitAI m_unitAI;
    public UnitHealth m_unitHealth;
    private UnitSkill m_unitSkill;
    private int collectCount = 0;

    public override void Attached()
    {
       
        state.AddCallback("AttackTrigger", AttackCallback);
        state.OnAttackTrigger += AttackCallback;
        m_unitHealth = GetComponent<UnitHealth>();
        m_unitSkill = GetComponent<UnitSkill>();
    }
 
    public override void SimulateOwner()
    {
        if (state.IsAttack)
        {

            if(Time.time >= nextAttack)
            {
                state.AttackTrigger();

                if(m_unitAI.m_unitDef == UnitDef.Collector)
                {
                    
                    if(m_unitAI.layerMask == 8)
                    {
                        ResourceManager.instance.TakeMoney(50);
                        StartCoroutine(CoolTimeAttackAnim(1f, 10));
                        collectCount = 0;
                    }
                    else if (collectCount >= 3)
                    {
                        StartCoroutine(CoolTimeAttackAnim(1f, 8));
                        collectCount = 0;
                    }
                    else
                    {
                        collectCount++;
                    }

                }
                else if(m_unitAI.m_unitDef == UnitDef.Dryad)
                {
                    StartCoroutine(ThrowingInterpolation(0.2f));

                }
                else
                {
                    StartCoroutine(AttackInterpolation(AttackJudmentTime));
                }

                nextAttack = Time.time + AttackCoolTime;
                StartCoroutine(OnSkill(AttackCoolTime));
            }   
        }
    }

    IEnumerator OnSkill(float time)
    {
        time -= 0.5f;
        isSkill = false;
        yield return new WaitForSeconds(time);
        isSkill = true;
    }

    IEnumerator AttackInterpolation(float time)
    {
        yield return new WaitForSeconds(time);
        if (m_unitAI.opponentObj != null)
        {
            m_unitAI.opponentObj.GetComponent<UnitHealth>().TakeDamage(AttackDamage);

            if(m_unitAI.m_unitDef == UnitDef.Dokkebi)
            {
                m_unitAI.opponentObj.GetComponent<UnitSkillEffect>().TakeEffect((int)SkillEfttectDef.DokkebiAttackEffect);
            }
            if (m_unitSkill != null)
                m_unitHealth.ManaInCrease(manaStack);
        }
    }

    IEnumerator ThrowingInterpolation(float time)
    {
        yield return new WaitForSeconds(time);

        Vector2 throwing = transform.position;

        if (!m_unitAI.sprite.flipX)
        {
            throwing.x += 0.3f;
        }
        else
        {
            throwing.x -= 0.3f;
        }
        GameObject go = BoltNetwork.Instantiate(BoltPrefabs.DryadThrowing, throwing, Quaternion.identity);
        go.GetComponent<Throwing>().TakeThrowing(1.0f, m_unitAI.opponentObj, gameObject);
    }

    IEnumerator CoolTimeAttackAnim(float time,int layerMask)
    {
        yield return new WaitForSeconds(time);
        m_unitAI.layerMask = layerMask;
    }

    void AttackCallback()
    {
        state.Animator.SetTrigger("IsAttack");

        switch (m_unitAI.m_unitDef)
        {
            case UnitDef.GoastKnight:
                m_unitAI.m_audio.Play("KnightAttack",0.3f);
                break;
            case UnitDef.Burserker:
                m_unitAI.m_audio.Play("BurserkerAttack",0.3f);
                break;
            case UnitDef.Dryad:
                m_unitAI.m_audio.Play("DryadAttack",0.3f);
                break;
            case UnitDef.Soldier:
                m_unitAI.m_audio.Play("SoldierAttack",0.1f);
                break;
            case UnitDef.Titan:
                m_unitAI.m_audio.Play("TitanAttack",0.5f);
                break;
            case UnitDef.Dwarf:
                m_unitAI.m_audio.Play("TitanAttack", 0.3f);
                break;
            case UnitDef.Crusader:
                m_unitAI.m_audio.Play("Crusader", AttackJudmentTime);
                break;
            case UnitDef.Dokkebi:
                m_unitAI.m_audio.Play("Dokkebi", AttackJudmentTime);
                break;
            case UnitDef.Nun:
                m_unitAI.m_audio.Play("KnightSkill", AttackJudmentTime);
                break;
            case UnitDef.Slime:
                m_unitAI.m_audio.Play("KnightSkill", AttackJudmentTime);
                break;
        }
    }
}
