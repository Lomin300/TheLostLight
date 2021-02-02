using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetActive : MonoBehaviour
{
    public GameObject[] ob;
    // Start is called before the first frame update
    public void ReverseActiveObject()
    {
        for(int i=0; i<ob.Length; i++)
        {
            if (ob[i].activeInHierarchy)
                ob[i].SetActive(false);
            else
                ob[i].SetActive(true);
        }
    }
}
