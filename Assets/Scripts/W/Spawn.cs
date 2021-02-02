using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class Spawn : Bolt.EntityBehaviour<IICustomCanvas>
{
    // Start is called before the first frame update

    public RectTransform go_rect;
    private Vector2 go_pos;
    private NextCard go_info;
    public RectTransform rect;
    public GameObject dummyUnit;
    public SpriteRenderer dummySprite;
    public Color dummyColor;
    public bool isClick;
    public Vector2 vec;
    public Vector2 lp;
    public Vector2 mousePosition;
    private int btn_num;
    private bool isOnClick;
    private CardManager card;
    private ResourceManager m_resource;
    private GameObject multiUnit;
    private void Start()
    {
        card = GetComponent<CardManager>();
        rect = GetComponent<RectTransform>();
        m_resource = GetComponent<ResourceManager>();
        dummySprite = dummyUnit.GetComponent<SpriteRenderer>();
        dummyColor = dummySprite.color;
        if (!BoltNetwork.IsServer)
        {
            dummySprite.flipX = true;
        }
    }

    private void Update()
    {
        
        if (isClick)
        {
            vec = Input.mousePosition;

            RectTransformUtility.ScreenPointToLocalPointInRectangle(rect, vec, Camera.main,out lp);
            mousePosition = Camera.main.ScreenToWorldPoint(vec);

            if(lp.y <= -165)
            {
                go_rect.anchoredPosition = lp;
                go_rect.localScale = Vector2.one;
                if (go_info.cardNum != 7)
                    dummySprite.color = Color.clear;
                else
                    multiUnit.SetActive(false);
            }
            else
            {
                go_rect.localScale = Vector2.zero;
                dummyUnit.transform.position = mousePosition;

                if (go_info.cardNum != 7)
                    dummySprite.color = dummyColor;
                else
                    multiUnit.SetActive(true);

            }
           

        }
    }


    public void TakeClick(int num)
    {
        if(card.Hand[num] != null && card.Hand_Info[num].isSpawn)
        {
            isOnClick = true;
            btn_num = num;
            isClick = true;
            go_rect = card.Hand[num].GetComponent<RectTransform>();
            go_pos = go_rect.anchoredPosition;
            go_info = card.Hand_Info[num];
            GameObject go =Resources.Load(go_info.cardNum.ToString()) as GameObject;

            if (go_info.cardNum == 7)
            {
                multiUnit = Instantiate(go, dummyUnit.transform);
                if (BoltNetwork.IsClient)
                {
                    SpriteRenderer[] sprite = multiUnit.GetComponentsInChildren<SpriteRenderer>();
                    for (int i = 0; i < sprite.Length; i++)
                    {
                        sprite[i].flipX = true;
                    }
                }
            }
            else
            dummySprite.sprite = go.GetComponent<SpriteRenderer>().sprite;
        }
    }

    public void TakeClickUp()
    {
        if (isOnClick)
        {
            isClick = false;

            if (lp.y > -165)
            {
                SpawnUnit(go_info.cardNum, mousePosition);
                card.Decks.Add(go_info.cardNum);
                m_resource.money -= go_info.cost;
                if (go_info.cardNum == 7)
                {
                    Destroy(multiUnit);
                }
                else
                    dummySprite.sprite = null;
                Destroy(card.Hand[btn_num]);
                card.Hand[btn_num] = null;
                card.handSize--;
            }
            else
            {
                go_rect.anchoredPosition = go_pos;
            }
        }
        isOnClick = false;
    }

    public void SpawnUnit(int spawnNum,Vector2 spawnPosition)
    {
        GameObject go = null;
        GameObject[] gos = new GameObject[3];
        switch (spawnNum)
        {

            case 0:
                go = BoltNetwork.Instantiate(BoltPrefabs.Burserker, spawnPosition, Quaternion.identity);
                break;
            case 1:
                go = BoltNetwork.Instantiate(BoltPrefabs.Nun, spawnPosition, Quaternion.identity);
                break;
            case 2:
                go = BoltNetwork.Instantiate(BoltPrefabs.GoastKnight, spawnPosition, Quaternion.identity);
                break;
            case 3:
                go = BoltNetwork.Instantiate(BoltPrefabs.Collector, spawnPosition, Quaternion.identity);
                break;
            case 4:
                go = BoltNetwork.Instantiate(BoltPrefabs.Crusader, spawnPosition, Quaternion.identity);
                break;
            case 5:
                go = BoltNetwork.Instantiate(BoltPrefabs.Dryad, spawnPosition, Quaternion.identity);
                break;
            case 6:
                go = BoltNetwork.Instantiate(BoltPrefabs.Titan, spawnPosition, Quaternion.identity);
                break;
            case 7:
                gos[0] = BoltNetwork.Instantiate(BoltPrefabs.Soldier_Default, new Vector2(spawnPosition.x + 0.4f, spawnPosition.y), Quaternion.identity);
                gos[1] = BoltNetwork.Instantiate(BoltPrefabs.Soldier_Gray, new Vector2(spawnPosition.x, spawnPosition.y + 0.4f), Quaternion.identity);
                gos[2] = BoltNetwork.Instantiate(BoltPrefabs.Soldier_White, new Vector2(spawnPosition.x, spawnPosition.y - 0.4f), Quaternion.identity);
                break;
            case 8:
                go = BoltNetwork.Instantiate(BoltPrefabs.Slime, spawnPosition, Quaternion.identity);
                break;
        }

        if (!BoltNetwork.IsServer)
        {
            if (spawnNum != 7)
            {
                go.GetComponent<SpriteRenderer>().flipX = true;
            }
            else
            {
                for (int i = 0; i < gos.Length; i++)
                {
                    gos[i].GetComponent<SpriteRenderer>().flipX = true;
                }
            }
        }
    }
}
