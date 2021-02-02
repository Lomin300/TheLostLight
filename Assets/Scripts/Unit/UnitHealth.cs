using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UnitHealth : Bolt.EntityEventListener<ICustomUnitState>
{
    public int localHealth = 100;
    public int maxHealth = 100;
    public int localMana = 0;
    public int maxMana = 120;
    public GameObject hpBar;
    public GameObject mpBar;
    public GameObject BackGroundhpBar;
    public Color HitCharColor;
    public Color HealCharColor;
    

    [SerializeField]
    private Image hpBar_Img;
    [SerializeField]
    private Image mpBar_Img;
    private int backupHelth;
    private bool isDamage = false;
    private bool aniFirst = true;
    private Color originalManaColor;
    private Color originalColor;
    private Color HitColor = Color.white;
    private Color originalCharColor;
    private WaitForSeconds ws;
    private AttackMessage attack;
    [SerializeField]
    private SpriteRenderer sprite;
    public GameObject endPanel;
    private DieMessage die;
    private UnitAI m_unitAI;
    private DefalutTimer m_timer;
    private void Awake()
    {
        m_timer = GameObject.Find("UI").GetComponent<DefalutTimer>();
        endPanel = GameObject.Find("UI").transform.Find("EndPanel").gameObject;
        sprite = gameObject.GetComponent<SpriteRenderer>();
        hpBar = transform.GetChild(0).GetChild(0).GetChild(0).gameObject;
        BackGroundhpBar = transform.GetChild(0).GetChild(0).gameObject;
        mpBar = transform.GetChild(0).GetChild(1).gameObject;
        hpBar_Img = hpBar.GetComponent<Image>();
        mpBar_Img = mpBar.GetComponent<Image>();
        m_unitAI = GetComponent<UnitAI>();
        
    }

    public override void Attached()
    {
        if (entity.IsOwner)
        {
            //state.Health = localHealth;
            state.Mana = localMana;
            
        }
       
        state.AddCallback("Health", HealthCallback);
        state.AddCallback("Mana", ManaCallback);
    }

    
    private void Start()
    {
        if (!entity.IsOwner) hpBar_Img.color = Color.red;


       
        backupHelth = localHealth;
        originalColor = hpBar_Img.color;
        originalManaColor = mpBar_Img.color;
        originalCharColor = sprite.color;
        ws = new WaitForSeconds(0.01f);
    }

    private void HealthCallback()
    {
        localHealth = state.Health;

        if (localHealth != maxHealth && aniFirst)
        {
            BackGroundhpBar.SetActive(true);
            aniFirst = false;
        }

        StartCoroutine(HitAnimationCoroutine(HitColor,originalColor,hpBar_Img));
        if(localHealth<backupHelth)
        StartCoroutine(HitAnimationCoroutine(HitCharColor, originalCharColor, sprite));
        else
        StartCoroutine(HitAnimationCoroutine(HealCharColor, originalCharColor, sprite));

        hpBar_Img.fillAmount = (float)localHealth / maxHealth;
        if (localHealth <= 0)
        {
            UnitDie();
        }

        isDamage = false;
        backupHelth = localHealth;
    }

    private void UnitDie()
    {
        if (entity.IsOwner)
        {

            Collider2D[] myunits = Physics2D.OverlapCircleAll(transform.position, 2.0f, 1 << LayerMask.NameToLayer("Player"));

            if (myunits.Length > 0)
            {
                foreach (Collider2D coll in myunits)
                {
                    coll.gameObject.SendMessage("OnUnitDie", SendMessageOptions.DontRequireReceiver);
                }
            }
        }

        BoltNetwork.Destroy(gameObject);
    }

    private void ManaCallback()
    {
        localMana = state.Mana;

        if (localMana <= 0)
        {
            HitAnimationCoroutine(HitColor, originalManaColor, mpBar_Img);
        }

        mpBar_Img.fillAmount = (float)localMana/ maxMana;
    }

    public void ManaInCrease(int mana)
    {
        state.Mana += mana;
    }

    public void ManaInit()
    {
        state.Mana = 0;
    }
    public void TakeDamage(int damage)
    {
        attack = AttackMessage.Create(entity);
        attack.AttackDamage = -damage;
       
        attack.Send();
    }
    public void TakeHeal(int heal)
    {
        attack = AttackMessage.Create(entity);
        attack.AttackDamage = heal;
       
        attack.Send();
    }
    public override void OnEvent(AttackMessage evnt)
    {
        if (entity.IsOwner)
        {
            localHealth += evnt.AttackDamage;
            if (localHealth > maxHealth)
                localHealth = maxHealth;
            isDamage = true;
        }
        
    }

    public override void SimulateOwner()
    {
        if (isDamage)
            state.Health = localHealth;

        if(m_timer.TotalTimer <= 0 && m_unitAI.m_unitDef == UnitDef.Collector)
        {
            ResourceManager.instance.TakeMoney(25);
            BoltNetwork.Destroy(gameObject);
        }
    }

    private IEnumerator HitAnimationCoroutine(Color hit,Color original,Image img, int count = 10)
    {
        
        Color ratio = (hit - original) / count;
        int tmp_count = count;

        while(count!=0)
        {
            img.color += ratio;
            count--;
            yield return ws;
        }

        while(count!=tmp_count)
        {
            img.color -= ratio;
            count++;
            yield return ws;
        }

        img.color = original;
    }

    private IEnumerator HitAnimationCoroutine(Color hit, Color original, SpriteRenderer sprite, int count = 10)
    {
        
        Color ratio = (hit - original) / count;
        int tmp_count = count;

        while (count != 0)
        {
            sprite.color += ratio;
            count--;
            yield return ws;
        }

        while (count != tmp_count)
        {
            sprite.color -= ratio;
            count++;
            yield return ws;
        }

        sprite.color = original;
    }

    private void OnDestroy()
    {
        if (gameObject.tag == "Hero" && gameObject.layer == 8)
        {
            endPanel.SetActive(true);
            endPanel.transform.GetChild(0).GetComponent<Text>().text = "승리!";
            m_unitAI.m_audio.Play("Win", 0);
            BoltLauncher.Shutdown();

            
        }
        else if(gameObject.tag == "Hero" && gameObject.layer == 9)
        {
            endPanel.SetActive(true);
            endPanel.transform.GetChild(0).GetComponent<Text>().text = "패배!";
            m_unitAI.m_audio.Play("Defeat", 0);
            BoltLauncher.Shutdown();
        }
       
    }
}
