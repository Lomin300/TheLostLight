using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_FireFly : MonoBehaviour
{
    public TextMeshProUGUI meshPro;

    private void Update()
    {
        meshPro.text = BattleCardManagers.Instance.firefly.ToString();
    }
}
