using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UnitStat
{
    UnitDef unitDef; //유닛 식별 enum -> UnitDef스크립트 보면 이해할듯
    public string textName; //텍스트 상에 보여줄 이름
    public int damage; //공격력
    public float attackSpeed; //공격속도
    public int hp; //최대 체력
    public int manaStack; //공격마다 차오르는 마나량 (참고로 모든 유닛 최대마나는 120임
    public int skillDamage; //스킬 데미지
    public float traceRange; //탐색범위 -> 적 유닛을 인식하는 범위
    public float attackRange; // 공격범위 -> 적 유닛을 공격하는 범위
    public float speed;      //이동속도 -> 아직은 별로 의미없음

    public int exp; //경험치

    //더 필요한거 있으면 추가점

    public void SetLvUp()
    {
        damage += damage / 10;
        hp += hp / 10;
        manaStack += manaStack / 10;
        skillDamage += skillDamage / 10;
    }
    
}
