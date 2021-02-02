using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InitStringToText : MonoBehaviour
{
    public Text textComponent;
    public string textString;

    // Start is called before the first frame update
    void Start()
    {
        textComponent.text = textString;
    }
}
