using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CardManager : MonoBehaviour
{

    public RectTransform[] hand_pos;

    public GameObject NextCard_prefab;
    public GameObject[] Hand;
    public NextCard[] Hand_Info;
    public List<int> Decks;
    private GameObject NextCard;
    private NextCard NextCard_Img;
    private float drawTime = 0f;
    public int handSize = 0;
    private bool isInit;
    public ResourceManager m_resource;
    private void Start()
    {
        m_resource = GetComponent<ResourceManager>();
        Hand = new GameObject[6];
        Hand_Info = new NextCard[6];
        NextCard = Instantiate(NextCard_prefab, this.transform);
        isInit = true;
        NextCard_Img = NextCard.GetComponent<NextCard>();
        NextCard_Img.unit_Img = NextCard.transform.GetChild(0).GetComponent<Image>();
        NextCard_Img.time_Img = NextCard.transform.GetChild(1).GetComponent<Image>();
        NextCard_Img.time_Img.fillAmount = 0;
        DrawHand(NextCard_Img);

    }

    void Update()
    {
        if(drawTime <=0f && handSize != 6)
        {
            
            for(int i = 0; i < Hand.Length; i++)
            {

                if(Hand[i] == null)
                {
                    Hand[i] = NextCard.gameObject;
                    Hand_Info[i] = NextCard_Img;
                    NextCard_Img.m_resource = m_resource;
                    NextCard_Img.cost = m_resource.unit_cost[NextCard_Img.cardNum];
                    NextCard_Img.isStart = true;
                    
                    StartCoroutine(DrawAnimation(i,Hand[i]));
                    break;
                }
            }

            NextCard = Instantiate(NextCard_prefab, this.transform);
            NextCard_Img = NextCard.GetComponent<NextCard>();
            NextCard_Img.unit_Img = NextCard.transform.GetChild(0).GetComponent<Image>();
            NextCard_Img.time_Img = NextCard.transform.GetChild(1).GetComponent<Image>();
            DrawHand(NextCard_Img);
            handSize++;

            if (!isInit)
                drawTime = 3.0f;
            else
            {
                NextCard_Img.time_Img.fillAmount = 0;
            }
               
        }
        else
        {
            if (isInit)
            {
                isInit = false;
                drawTime = 3.0f;
            }

            drawTime -= Time.deltaTime;
            NextCard_Img.time_Img.fillAmount = drawTime / 3.0f;
        }
    }

    IEnumerator DrawAnimation(int num,GameObject go)
    {
        RectTransform tr = go.GetComponent<RectTransform>();
        Vector2 pos = tr.anchoredPosition;
        Vector2 scale = tr.localScale;
        float time = 0f;
        while(time <=1)
        {
            time += 5 * Time.deltaTime;
            tr.anchoredPosition = Vector2.Lerp(pos, hand_pos[num].anchoredPosition, time);
            tr.localScale = Vector2.Lerp(scale, Vector2.one, time);
            yield return new WaitForEndOfFrame();
        }
       
    }

   private void DrawHand(NextCard card)
    {
        int index = Random.Range(0, Decks.Count);
        int num = Decks[index];
        Decks.RemoveAt(index);
        GameObject go = Resources.Load("Card/"+num.ToString()) as GameObject;
        card.unit_Img.sprite = go.GetComponent<SpriteRenderer>().sprite;
        card.cardNum = num;
        
    }
}
