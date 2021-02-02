using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Rube : MonoBehaviour
{
    public Text rubeText;

    // Update is called once per frame
    void FixedUpdate()
    {
        rubeText.text = UnitInventory.Instance.rube.ToString();
    }
}
