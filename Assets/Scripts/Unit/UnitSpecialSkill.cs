using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSpecialSkill : Bolt.EntityBehaviour<ICustomUnitState>
{

    private UnitHealth m_unitHealth;
    private UnitAttack m_unitAttack;
    private UnitSkill m_unitSkill;
    public GameObject skillParticle;

    public override void Attached()
    {
        m_unitHealth = GetComponent<UnitHealth>();
        m_unitAttack = GetComponent<UnitAttack>();
        m_unitSkill = GetComponent<UnitSkill>();
    }

    public void Berserker()
    {
        if (entity.IsOwner)
        {
            if (m_unitHealth.localMana != m_unitHealth.maxMana)
            {
                m_unitAttack.AttackDamage += 10;
                m_unitHealth.ManaInCrease(24);
            }

        }

    }

    public void Dryad(GameObject target)
    {
        if (entity.IsOwner)
        {
            target.GetComponent<UnitHealth>().TakeDamage(10);
            m_unitHealth.ManaInCrease(24);
            m_unitSkill.Transition(target, 3.0f);
        }
    }
}
