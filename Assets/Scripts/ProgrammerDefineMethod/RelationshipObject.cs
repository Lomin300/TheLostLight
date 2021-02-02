using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RelationshipObject : MonoBehaviour
{
    public List<GameObject> objects = new List<GameObject>();
    public void SelectLeader()
    {
        for (int i = 0; i < UnitInventory.Instance.leaders.Count; i++)
        {
            if (objects[0].name == UnitInventory.Instance.leaders[i].name)
                UnitInventory.Instance.leader = UnitInventory.Instance.leaders[i];
            //UnitInventory.Instance.leader = GameObject.Find(UnitInventory.Instance.leaders[i].name);
        }
        //UnitInventory.Instance.leader = objects[0];
    }
}
