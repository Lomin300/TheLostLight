using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InitLobby : MonoBehaviour
{
    public GameObject illust;
    public Text nameText;
    public AudioClip bgm;
    void Start()
    {
        UnitInventory.Instance.illust = this.illust;
        /*for(int i=0; i<UnitInventory.Instance.leaders.Count; i++)
        {
            UnitInventory.Instance.leader = GameObject.Find(UnitInventory.Instance.leaders[i].name);
        }*/
        UnitInventory.Instance.SetLeader();
        nameText.GetComponent<Text>().text = UnitInventory.Instance.nickNameText.GetComponent<Text>().text;
        if(DontDestroyThisObject.Instance.audioSource.clip != bgm)
        {
            DontDestroyThisObject.Instance.audioSource.clip = bgm;
            DontDestroyThisObject.Instance.audioSource.Play();
        }
        
    }

    private void FixedUpdate()
    {
        nameText.text = UnitInventory.Instance.nickNameText.GetComponent<Text>().text;
    }


}
