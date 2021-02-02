using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAI : Bolt.EntityEventListener<ICustomUnitState>
{
    public enum State
    {
        PATROL,
        TRACE,
        ATTACK,
        SKILL,
        CC,
        IDLE,
        DIE,
    }

        
    [SerializeField]
    private Collider2D[] opponentColls;
    [SerializeField]
    private Vector3 direction;
    
    private Transform myTr;
    private WaitForSeconds ws;
    
    private UnitHealth m_unitHealth;
    private UnitSkill m_unitSkill;
    private UnitSpecialSkill m_unitSpecialSkill;
    private CCMessage m_CCMessage;
    private UnitCC m_unitCC;
    private UnitAttack m_unitAttack;
    private float m_force;
    private float m_time;
    private int m_index;
    private int tmp_count=0;
    private int count=0;

    public SpriteRenderer sprite;
    public UnitDef m_unitDef;
    public int layerMask ;
    public State m_state = State.PATROL;
    public Animator playerAnimator;
    public MoveAgent moveAgent;
    public float traceDist;
    public float attackDist;
    public bool isDie = false;
    public bool isAttack = false;
    public bool isCC = false;
    public GameObject opponentObj;
    public bool isHero = false;
    public bool isSuperArm = false;
    public UnitStat m_unitStat;
    public AudioManager m_audio;

    private void Awake()
    {
        m_audio = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        myTr = GetComponent<Transform>();
        m_unitCC = GetComponent<UnitCC>();
        m_unitHealth = GetComponent<UnitHealth>();
        m_unitSkill = GetComponent<UnitSkill>();
        m_unitSpecialSkill = GetComponent<UnitSpecialSkill>();
        m_unitAttack = GetComponent<UnitAttack>();
        ws = new WaitForSeconds(0.1f);
    }

    public override void Attached()
    {
        if (isHero)
            m_state = State.IDLE;

        sprite = GetComponent<SpriteRenderer>();
        if (BoltNetwork.IsServer && !entity.IsOwner)
        {
            sprite.flipX = true;
            
            if (m_unitDef == UnitDef.Collector)
            {
                gameObject.layer = 9;
            }
            else
            {
                gameObject.layer = layerMask;
            }
        }
        else if (BoltNetwork.IsClient && !entity.IsOwner)
        {
            gameObject.layer = layerMask;
            if (m_unitDef == UnitDef.Collector)
            {
                gameObject.layer = 9;
            }
            else
            {
                gameObject.layer = layerMask;
            }
        }

        state.AddCallback("IsFlip", FlipCallback);
        state.SetAnimator(playerAnimator);
        state.SetTransforms(state.UnitTransform, gameObject.transform);
        if (entity.IsOwner)
        {
            StartCoroutine(CheckState());
            StartCoroutine(Action());
            state.IsAttack = isAttack;
        }
            

    }

    IEnumerator CheckState()
    {

        while (!isDie)
        {
            if (m_state == State.DIE) yield break;

            if(opponentObj == null || m_unitDef == UnitDef.Collector)
            opponentColls = Physics2D.OverlapCircleAll(transform.position, traceDist, 1 << layerMask);

            if (opponentColls.Length > 0)
            {
               

                if ((m_unitDef == UnitDef.Collector && layerMask == 8 )||m_unitDef==UnitDef.Titan)
                {
                    foreach (var coll in opponentColls)
                    {
                        if (coll.tag == "Hero") opponentObj = coll.gameObject;
                    }
                    
                    if(opponentObj == null)
                    {
                        m_state = State.PATROL;
                        yield return ws;
                        continue;
                    }
                }
                else
                {
                    opponentObj = opponentColls[0].gameObject;
                }


                float dist = Vector2.Distance(myTr.position, opponentObj.transform.position);
                Vector3 vec = opponentObj.transform.position - myTr.position;
                direction = Vector3.Normalize(vec);

                if(!isHero)
                FlipFunction(direction.x);

                if (!isCC || isSuperArm)
                {
                    if ((m_unitSkill != null) && (m_unitHealth.localMana >= m_unitHealth.maxMana) && m_unitAttack.isSkill)
                    {
                        m_state = State.SKILL;
                    }
                    else if (dist <= attackDist) m_state = State.ATTACK;
                    else if (dist <= traceDist && !isHero) m_state = State.TRACE;
                    else if (isHero) m_state = State.IDLE;
                    else m_state = State.PATROL;
                }
                else m_state = State.CC;
               
            }
            else if(!isHero)
            {
               if (BoltNetwork.IsServer) state.IsFlip = 1;
               else state.IsFlip = -1;

                m_state = State.PATROL;
            }
            else if (isHero)
            {
                m_state = State.IDLE;
            }

            yield return ws;
        }
    }

    IEnumerator Action()
    {
        while (!isDie)
        {
            yield return ws;

            switch (m_state)
            {
                case State.PATROL:
                    opponentObj = null;
                    state.IsMove = true;
                    state.IsAttack = false;
                    moveAgent.patrolling = true;
                    break;
                case State.TRACE:
                    state.IsMove = true;
                    state.IsAttack = false;
                    moveAgent.patrolling = false;
                    moveAgent.traceTarget = opponentObj.transform.position;
                    break;
                case State.ATTACK:
                    moveAgent.Stop();
                    state.IsMove = false;
                    
                    if (state.IsAttack == false)
                        state.IsAttack = true;
                    break;
                case State.SKILL:
                    moveAgent.Stop();
                    state.IsMove = false;
                    state.IsAttack = false;
                    m_unitSkill.TakeSkill();
                    m_unitAttack.nextAttack = Time.time + m_unitAttack.AttackCoolTime;
                    break;
                case State.IDLE:
                    state.IsMove = false;
                    state.IsAttack = false;
                    moveAgent.Stop();
                    break;
                case State.CC:

                    if (!m_unitCC.isCor || count != tmp_count)
                    {
                        tmp_count = count;
                        moveAgent.Stop();
                        state.IsMove = false;
                        state.IsAttack = false;
                        moveAgent.agent.isCC = true;
                        m_unitCC.CCManager(m_index, m_time, m_force);
                    }
                    break;
                case State.DIE:
                    moveAgent.Stop();
                    break;

            }
        }
    }

    public void TakeCC(int index,float time,float force=0f)
    {
        m_CCMessage = CCMessage.Create(entity);
        m_CCMessage.force = force;
        m_CCMessage.time = time;
        m_CCMessage.index = index;
        m_CCMessage.Send();
    }

    public override void OnEvent(CCMessage evnt)
    {
        if (entity.IsOwner)
        {
            count++;
            isCC = true;
            m_force = evnt.force;
            m_index = evnt.index;
            m_time = evnt.time;
        }
    }
    private void FlipFunction(float key)
    {

        state.IsFlip = key;
    }
    private void FlipCallback()
    {
        if (state.IsFlip >= 0) sprite.flipX = false;
        else sprite.flipX = true;
    }
    private void Update()
    {
        if (state.IsAttack)
        {
            state.Animator.SetBool("IsMove", state.IsMove);
            return;
        }
        
        if (state.IsMove)
        {
            state.Animator.SetBool("IsMove",state.IsMove);
        }
        else
        {
            state.Animator.SetBool("IsMove",state.IsMove);
        }
    }

    public void OnUnitDie()
    {
        if (entity.IsOwner)
        {
            if (m_unitDef == UnitDef.Burserker) m_unitSpecialSkill.Berserker();
        }

    }

    private void OnDrawGizmos()
    {

        if (m_state == State.TRACE)
            Gizmos.color = Color.green;
        else
            Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(gameObject.transform.position, traceDist);

        if (m_state == State.ATTACK)
            Gizmos.color = Color.red;
        else
            Gizmos.color = Color.white;

        Gizmos.DrawWireSphere(gameObject.transform.position, attackDist);


        Gizmos.color = Color.white;

        Gizmos.DrawWireCube(new Vector3(gameObject.transform.position.x + 1f, gameObject.transform.position.y, gameObject.transform.position.z), new Vector3(1, 2, 1));

    }
}
