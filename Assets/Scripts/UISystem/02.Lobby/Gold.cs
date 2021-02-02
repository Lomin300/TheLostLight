using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gold : MonoBehaviour
{
    public Text goldText;

    

    // Update is called once per frame
    void FixedUpdate()
    {
        Debug.Log(UnitInventory.Instance.gold);
        goldText.text = UnitInventory.Instance.gold.ToString();
    }
}
