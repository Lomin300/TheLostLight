using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InitShop : MonoBehaviour
{
    public Text nametext;
    // Start is called before the first frame update
    

    // Update is called once per frame
    void FixedUpdate()
    {
        nametext.text = UnitInventory.Instance.nickNameText.GetComponent<Text>().text;
    }
}
