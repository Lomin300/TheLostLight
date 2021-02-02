using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitState : MonoBehaviour
{
    public int cost;
    //유닛 가격
    public string CharName;
    //유닛 이름
    public float MaxHP;
    //최대 체력
    public float CurrentHP;
    //현재 체력
    public float MP;
    //최대 마나
    public float MP_increase;
    //마나 회복치
    public float MoveSpeed;
    //이동 속도
    public float AttackSpeed;
    //공격 속도
    public float Damage;
    //데미지
    public float Armor;
    //방어력
    public float Range;
    //사거리
    public float Recognition;
    //인식 범위
    public string RecogOrder;

    public List<string> order = new List<string> { "Unit", "Miner", "Hero" };


    void Start()
    {
        BattleCardManagers.Instance.AllUnitObjectList.Add(this.gameObject);
        order.Remove(RecogOrder);
        order.Insert(0, RecogOrder);
    }

    void Update()
    {
        if (CurrentHP <= 0)
        {
            //Destroy(parent);
            BattleCardManagers.Instance.AllUnitObjectList.Remove(this.gameObject);
            this.gameObject.SetActive(false);

        }
            
    }

    public void Setting()
    {
        //스텟 정의

        
    }
}
