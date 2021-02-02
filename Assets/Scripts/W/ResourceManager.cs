using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ResourceManager : MonoBehaviour
{
    #region public
    public int money;
    public float time;
    public Text moneyText;
    public static ResourceManager instance;
    public int[] unit_cost;
    private WaitForSeconds ws;
    
    #endregion

    #region private

    #endregion

    private void Awake()
    {
        
        if(instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {


        moneyText.text = money.ToString();
        ws = new WaitForSeconds(time);
        StartCoroutine(PassiveIncreaseMoney());
    }


    private void Update()
    {
        
    }
    public void TakeMoney(int _money)
    {
        money += _money;
        moneyText.text = money.ToString();
    }

    IEnumerator PassiveIncreaseMoney()
    {
        while (true)
        {
            yield return ws;
            money += 1;
            moneyText.text = money.ToString();
        }
       
    }

}
