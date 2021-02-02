using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSkillEffect : Bolt.EntityEventListener<ICustomUnitState>
{
    public GameObject[] effects;
    private Animator[] anims;
    public GameObject BotEffectPos;
    private SkillEfftectMessage skillEffect;

    private void Start()
    {
        anims = new Animator[effects.Length];

        for(int i = 0; i < anims.Length; i++)
        {
            anims[i] = effects[i].GetComponent<Animator>();
        }
    }


    public void TakeEffect(int index)
    {
        skillEffect = SkillEfftectMessage.Create(entity);
        skillEffect.Index = index;
        skillEffect.Send();
    }

    public override void OnEvent(SkillEfftectMessage evnt)
    {
        
        switch (evnt.Index)
        {
            case (int)SkillEfttectDef.CrusaderSkillEfftect:
                StartCoroutine(EffectDurationTime(BotEffectPos,
                    effects[200 - (int)SkillEfttectDef.CrusaderSkillEfftect],
                    anims[200 - (int)SkillEfttectDef.CrusaderSkillEfftect].runtimeAnimatorController));
                break;
            case (int)SkillEfttectDef.SlimeSkillEffect:
                StartCoroutine(EffectDurationTime(BotEffectPos, effects[200 - (int)SkillEfttectDef.SlimeSkillEffect],
                    anims[200 - (int)SkillEfttectDef.SlimeSkillEffect].runtimeAnimatorController));
                break;

            case (int)SkillEfttectDef.DokkebiAttackEffect:
                StartCoroutine(EffectDurationTime(BotEffectPos, effects[200 - (int)SkillEfttectDef.DokkebiAttackEffect],
                    anims[200 - (int)SkillEfttectDef.DokkebiAttackEffect].runtimeAnimatorController));
                break;
        }
    }

    private IEnumerator EffectDurationTime(GameObject pos,GameObject effect,RuntimeAnimatorController ac)
    {
        
        GameObject go = Instantiate(effect, pos.transform.position,Quaternion.identity,this.transform);
        float time = ac.animationClips[0].length;
        yield return new WaitForSeconds(time);

        Destroy(go);
    }
}
