using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSpawn : Bolt.EntityBehaviour<IICustomCanvas>
{
    public GameObject dummyUnit;
    private GameObject multiUnit;
    public bool isSpawn = true;
    
    private bool isClick = false;
    private Vector2 mousePosition;
    private int spawnNum;

    public override void Attached()
    {
        state.isSpawn = isSpawn;
    }

    private void Start()
    {
        if (!BoltNetwork.IsServer)
        {
            dummyUnit.GetComponent<SpriteRenderer>().flipX = true;
        }
    }

    private void Update()
    {

        if (isClick)
        {

            mousePosition = Input.mousePosition;
            mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
            dummyUnit.transform.position = mousePosition;
            if (Input.GetMouseButtonDown(0))
            {
                isClick = false;
                Spawn(spawnNum, dummyUnit.transform.position);
                if (spawnNum == 7)
                {
                    Destroy(multiUnit);
                }
                else
                    dummyUnit.GetComponent<SpriteRenderer>().sprite = null;
            }
        }
    }

    public void Spawn(int num,Vector3 spawnPosition)
    {

        GameObject go = null;
        GameObject[] gos = new GameObject[3];
        switch (num)
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

    public void ClickSpawn(int num)
    {
        spawnNum = num;
        isClick = true;
        GameObject go = Resources.Load(num.ToString()) as GameObject;

        if (num == 7)
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
        dummyUnit.GetComponent<SpriteRenderer>().sprite = go.GetComponent<SpriteRenderer>().sprite;
        
    }
}
