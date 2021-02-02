using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetActives : MonoBehaviour
{
    public List<GameObject> activeTrue = new List<GameObject>();
    public List<GameObject> activeFalse = new List<GameObject>();
    public List<GameObject> activeToggle = new List<GameObject>();

    public void OperateActives()
    {
        int i;

        for(i=0; i<activeTrue.Count; i++)
        {
            activeTrue[i].SetActive(true);
        }

        for (i = 0; i < activeFalse.Count; i++)
        {
            activeFalse[i].SetActive(false);
        }

        for (i = 0; i < activeToggle.Count; i++)
        {
            if(activeToggle[i].activeSelf)
                activeToggle[i].SetActive(false);
            else
                activeToggle[i].SetActive(true);
        }
    }
}
