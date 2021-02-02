using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Init_Inventory : MonoBehaviour
{
    Button btn_Unit;
    Button btn_Hero;

    // Start is called before the first frame update
    void Start()
    {
        btn_Unit = GameObject.Find("Category").GetComponent<Button>();
        btn_Hero = GameObject.Find("Category (1)").GetComponent<Button>();

        btn_Unit.onClick.AddListener(UnitInventory.Instance.Inventory_Unit);
        btn_Hero.onClick.AddListener(UnitInventory.Instance.Inventory_Hero);

        UnitInventory.Instance.inventory = GameObject.Find("Content");
        UnitInventory.Instance.Deck = GameObject.Find("DeckList");
        UnitInventory.Instance.heroDeck = GameObject.Find("HeroSlot");
        UnitInventory.Instance.Inventory_Unit();
        UnitInventory.Instance.Deck_Show();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
