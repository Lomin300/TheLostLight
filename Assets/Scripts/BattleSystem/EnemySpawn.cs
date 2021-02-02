using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    public float spawnDelay;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        spawnDelay -= Time.deltaTime;

        if(spawnDelay<=0)
        {
            BattleCardManagers.Instance.SummonUnit(Random.Range(0, UnitInventory.Instance.unitL.Count), "Enemy");
            SetSpawnDelay();
        }
    }

    void SetSpawnDelay()
    {
        spawnDelay = Random.Range(0, 10f);
    }
}
