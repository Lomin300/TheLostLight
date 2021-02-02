using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpBarScript : MonoBehaviour
{
    public float LerpSpeed;
    float CurrentFill;
    Image content;

    UnitState stat;

    Vector3 scale;

    // Start is called before the first frame update
    void Start()
    {
        content = GetComponent<Image>();
        stat = GetComponentInParent<UnitState>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        CurrentFill = stat.CurrentHP / stat.MaxHP;

        content.fillAmount = Mathf.Lerp(content.fillAmount,CurrentFill,Time.deltaTime*LerpSpeed);
    }
}
