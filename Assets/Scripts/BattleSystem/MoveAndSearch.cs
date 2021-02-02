using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum MOVE
{
    walk,
    recognition,
    skill,
    attack
}

public class MoveAndSearch : MonoBehaviour
{
    public string attackTargetLayerName;


    Rigidbody2D rigid;
    //리기드바디 컴포넌트

    RunAnimation runAni;

    Vector2 pos;
    Vector2 deltaPos;
    //해당 오브젝트의 좌표

    UnitState stat;
    //해당 오브젝트의 상태 값

    SearchUnit searchunit;

    GameObject Movetarget;
    //각자 서칭되어 레이어가 지정된 콜라이더들 집합.

    ushort MoveType;
    //일반적인 걸음인지, 적을 찾은 걸음인지, 스킬에 의한 걸음인지

    //================= 여기서부턴 충돌 및 충돌 코르틴 정의 ====================//
    //
    [SerializeField]
    Collider2D[] FoundCollider;


    

    Vector2 temppos;

    bool Iscolliding=false;
    bool IsAttack=false;

    bool IsSearch;
    bool IsSkill;

    float distance;
    //SortedColliders inrange;
    UnitState EnemyStat;

    //=======================================================================//

    // Start is called before the first frame update
    void Start()
    {

        pos = transform.position;
        rigid = GetComponent<Rigidbody2D>();

        stat = GetComponent<UnitState>();

        runAni = GetComponent<RunAnimation>();
        //runAni.Ani_Idle();
        runAni.Ani_Idle();
        runAni.Ani_Move();


        Debug.Log("스타트 들어왔음 스타트 코루틴 직전");
        Brain();

        StartCoroutine(ColliderCheckCoroutine());

        StartCoroutine(AttackWithSpeedCoroutine());
    }

    // Update is called once per frame
    void Update()
    {
        Brain();
        Move();
        Attack();
    }

    IEnumerator ColliderCheckCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.5f);

            FoundCollider = Physics2D.OverlapCircleAll(pos, stat.Recognition, 1 << LayerMask.NameToLayer(attackTargetLayerName));
            if (FoundCollider.Length != 0) //찾은 콜라이더가 있는 경우
            {
                Movetarget = SortObject(FoundCollider, stat.order);
                Iscolliding = true;
                Debug.Log("콜라이더 찾음!");
            }
            else
            {
                Iscolliding = false;
                IsAttack = false;
            }
            
        }

    }

    void Brain()
    {
        if (Iscolliding)
        {
            if (IsAttack)
                MoveType = (ushort)MOVE.attack;
            else
                MoveType = (ushort)MOVE.recognition;      
        }
        else
            MoveType = (ushort)MOVE.walk;

    }

    void Move()
    {
        switch (MoveType)
        {
            case (ushort)MOVE.walk:
                if(attackTargetLayerName.Equals("Player"))
                    pos.x = pos.x + (-stat.MoveSpeed * Time.deltaTime);
                else
                    pos.x = pos.x + (stat.MoveSpeed * Time.deltaTime);

                runAni.Ani_Move();
                break;

            case (ushort)MOVE.recognition:
                if (attackTargetLayerName.Equals("Player"))
                {
                    temppos = pos - (Vector2)Movetarget.transform.position;
                    temppos.Normalize();
                    temppos = temppos * -stat.MoveSpeed * Time.deltaTime;
                    pos = pos + temppos;
                }

                else
                {
                    temppos = (Vector2)Movetarget.transform.position - pos;
                    temppos.Normalize();
                    temppos = temppos * stat.MoveSpeed * Time.deltaTime;
                    pos = pos + temppos;
                }

                    runAni.Ani_Move();

                break;

            case (ushort)MOVE.skill:
                    runAni.Ani_Skiil();
                break;

            case (ushort)MOVE.attack:
                //움직이지 않음
                break;
        }

        /*if(deltaPos.x - pos.x > 0)
            transform.localScale = new Vector3(-1, 1, 1);
        else
            transform.localScale = new Vector3(1, 1, 1);

        if(MoveType != (ushort)MOVE.attack)
            deltaPos = pos;*/

        rigid.MovePosition(pos);
        //transform.position = pos;
        //이동
    }
    void Attack()
    {
    }

    IEnumerator AttackWithSpeedCoroutine()
    {
        yield return new WaitForSeconds(0.2f);
        while (true)
        {
                if (Movetarget != null)
                {
                    
                    distance = Vector2.Distance(Movetarget.transform.position, pos);

                    if (stat.Range >= distance)
                    {           
                        GameObject hpbar;
                        hpbar = Movetarget.transform.GetChild(0).GetChild(0).gameObject;
                        hpbar.SetActive(true);

                        Movetarget.GetComponent<RunAnimation>().Ani_Damaged();
                        EnemyStat = Movetarget.GetComponent<UnitState>();

                    if (EnemyStat.CurrentHP<=0)
                        {
                            IsAttack = false;
                        }

                        else
                        {

                        runAni.Ani_Attack();

                        
                        EnemyStat.CurrentHP -= stat.Damage;

                        IsAttack = true;
                        }
                            
                        
                        
                    }

                    else
                    {
                        IsAttack = false;
                    }
                }
                else
                    IsAttack = false;
            
            yield return new WaitForSeconds(stat.AttackSpeed);
        }
    }

    public GameObject SortObject(Collider2D[] colliders, List<string> order)
    {
        List<GameObject> SortedgameObjects = new List<GameObject>();

        //찾을 서칭 우선순위 개수를 할당해. 만약에 "에너미" "마이너" "히어로" = 3개를 할당해.


        for (int i = 0; i < order.Count; i++)
        {
            foreach (Collider2D find in colliders)
            {
                GameObject temp = find.gameObject;

                if (temp.tag.Equals(order[i]))
                {
                    SortedgameObjects.Add(temp);
                }
            }
        }
        if(colliders.Length>0)
            return SortedgameObjects[SortedgameObjects.Count-1];

        return null;
    }
}
  

