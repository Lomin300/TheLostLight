using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSkill : Bolt.EntityEventListener<ICustomUnitState>
{
    private UnitAI m_unitAI;
    private UnitAttack m_unitAttack;
    private UnitHealth m_unitHealth;
    private UnitSkillEffect m_unitEffect;
    private SkillMessage skill;
    public GameObject skillParticle;
    public float SkillJudgmentTime;
    private float direction;
    public int skillDamage;
    public override void Attached()
    {
        m_unitAI = GetComponent<UnitAI>();
        m_unitAttack = GetComponent<UnitAttack>();
        m_unitHealth = GetComponent<UnitHealth>();

    }
    public void TakeSkill()
    {
        m_unitHealth.ManaInit();
        skill = SkillMessage.Create(entity);
        skill.Index = (int)m_unitAI.m_unitDef;
        skill.Send();
        switch (m_unitAI.m_unitDef)
        {
            case UnitDef.Burserker:
                break;
            case UnitDef.Nun:
                StartCoroutine(SkillInterpolation_Heal(SkillJudgmentTime, skillDamage));
                break;
            case UnitDef.GoastKnight:
                Vector2 vec = transform.position;

                if (m_unitAI.sprite.flipX)
                {
                    direction = -1;
                    vec.x += -1;
                }
                else
                {
                    direction = 1;
                    vec.x += 1;
                }

                StartCoroutine(SkillInterpolation_DamageBox(SkillJudgmentTime, skillDamage, direction, vec));
                break;
            case UnitDef.Crusader:
              
                StartCoroutine(SkillInterpolation_DamageCircle(SkillJudgmentTime,skillDamage,2f,transform.position));
                break;
            case UnitDef.Dryad:
                StartCoroutine(SkillInterpolation_DamageCircle(SkillJudgmentTime, skillDamage, 2.5f, transform.position));
                break;
            case UnitDef.Slime:
                StartCoroutine(SkillInterpolation_DamageCircle(SkillJudgmentTime, skillDamage, 2f, transform.position));
                break;
        }
    }

    private void Damage_Box(int damage, float direction, Vector2 center)
    {
        Collider2D[] enemyunits = Physics2D.OverlapBoxAll(center, new Vector2(1,2), 0, 1 << LayerMask.NameToLayer("Enemy"));

        foreach (var unit in enemyunits)
        {
            unit.GetComponent<UnitHealth>().TakeDamage(damage);
            if (m_unitAI.m_unitDef == UnitDef.GoastKnight)
                KnockBack(unit, direction, 3f);
        }
    }

    private void Damage_Circle(int damage,Vector2 point,float range)
    {
        Collider2D[] enemyunits = Physics2D.OverlapCircleAll(point, range, 1 << LayerMask.NameToLayer("Enemy"));

        foreach (var unit in enemyunits)
        {
            unit.GetComponent<UnitHealth>().TakeDamage(damage);
            if (m_unitAI.m_unitDef == UnitDef.Crusader)
                unit.GetComponent<UnitSkillEffect>().TakeEffect((int)SkillEfttectDef.CrusaderSkillEfftect);
            else if (m_unitAI.m_unitDef == UnitDef.Slime)
                unit.GetComponent<UnitSkillEffect>().TakeEffect((int)SkillEfttectDef.SlimeSkillEffect);
            else if (m_unitAI.m_unitDef == UnitDef.Dryad)
                Transition(unit.gameObject, 2.0f);
            
        }
    }
    private void KnockBack(Collider2D coll, float direction, float force)
    {
        force *= direction;
        coll.GetComponent<UnitAI>().TakeCC((int)CCDef.KnockBack, 0.7f, force);
    }

    public void Transition(GameObject target,float time)
    {
        target.GetComponent<UnitAI>().TakeCC((int)CCDef.Transition, time);
    }
    private void Heal(int heal)
    {
        Collider2D[] myunits = Physics2D.OverlapCircleAll(transform.position, 2.0f, 1 << LayerMask.NameToLayer("Player"));

        foreach (var unit in myunits)
        {
            unit.GetComponent<UnitHealth>().TakeHeal(heal);
        }
    }
    public override void OnEvent(SkillMessage evnt)
    {
        switch (evnt.Index)
        {
            case (int)UnitDef.Burserker:
                skillParticle.SetActive(true);
                break;
            case (int)UnitDef.Nun:
                state.Animator.SetTrigger("SkillTrigger");
                StartCoroutine(ParticleDuration(skillParticle, 2.0f));
                break;
            case (int)UnitDef.GoastKnight:
                state.Animator.SetTrigger("SkillTrigger");
                m_unitAI.m_audio.Play("KnightSkill",0.3f);
                break;
            case (int)UnitDef.Crusader:
                state.Animator.SetTrigger("SkillTrigger");
                m_unitAI.m_audio.Play("TitanAttack", SkillJudgmentTime);
                break;
            case (int)UnitDef.Dryad:
                state.Animator.SetTrigger("SkillTrigger");
                m_unitAI.m_audio.Play("TitanAttack", SkillJudgmentTime);
                break;
            case (int)UnitDef.Slime:
                state.Animator.SetTrigger("SkillTrigger");
                m_unitAI.m_audio.Play("SlimeSkill",0.3f);
                break;
        }
    }

    private IEnumerator SkillInterpolation_Heal(float time, int heal)
    {
        yield return new WaitForSeconds(time);
        Heal(heal);
    }

    private IEnumerator SkillInterpolation_DamageBox(float time, int damage, float direction, Vector2 vec)
    {
        yield return new WaitForSeconds(time);
        Damage_Box(damage, direction, vec);
    }

    private IEnumerator SkillInterpolation_DamageCircle(float time, int damage,float range,Vector2 point)
    {
        yield return new WaitForSeconds(time);
        
        Damage_Circle(damage,point,range);
    }

    private IEnumerator ParticleDuration(GameObject Particle,float time)
    {
        Particle.SetActive(true);
        yield return new WaitForSeconds(time);
        Particle.SetActive(false);
    }
}
