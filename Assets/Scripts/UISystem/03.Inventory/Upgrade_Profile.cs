using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Upgrade_Profile : MonoBehaviour
{
    // Start is called before the first frame update

    public List<GameObject> sortGameObject = new List<GameObject>();
    public List<string> sortText = new List<string>();
    public List<string> sortprofile = new List<string>();
    public string notice = "서로 순서에 맞게 적어야 동작합니다.";

    private void Awake()
    {
        for(int i=0; i<sortGameObject.Count; i++)
        {
            sortText.Add(sortGameObject[i].GetComponent<UnitAI>().m_unitStat.textName);
        }
    }

    public string FindProfile(string initedNameText)
    {
        Debug.Log(initedNameText);

        for (int i=0; i<sortText.Count; i++)
        {
            if(sortText[i] == initedNameText)
            {
                return sortprofile[i];
            }
        }
        Debug.Log("맞는 프로필 못찾았음");
        return null;
    }

}
