using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackPanelClick : MonoBehaviour
{
    public GameObject upgradeUI;

    public void CloseUpgradUI()
    {
        UIUpgrade_Init.Guillotine();
        upgradeUI.transform.GetChild(1).gameObject.SetActive(false); //0.UIBackSprite flase
        upgradeUI.transform.GetChild(0).gameObject.SetActive(false); //백 판넬 false
        
        
    }


    // Start is called before the first frame update
}
