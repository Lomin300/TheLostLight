using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectDestroy : MonoBehaviour
{
    public GameObject[] guillotine = new GameObject[5];

    static public SelectDestroy instance = null;

    private void Start()
    {
        instance = this;
    }
    public void DestroySelectObject()
    {
        foreach(GameObject temp in guillotine)
        {
            Destroy(temp);
        }
    }

    public void DestroySelectObject(GameObject[] guillotine)
    {
        foreach(GameObject temp in guillotine)
        {
            Destroy(temp);
        }
    }

    public void DestroySelectObject(GameObject guillotine)
    {
        Destroy(guillotine);
    }

    private void OnDestroy()
    {
        Destroy(instance);
    }
}
