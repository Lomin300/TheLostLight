using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ResourceUI : MonoBehaviour
{

    static public ResourceUI instance;
    public Text Gold;
    public Text Level;
    public Text Name;
    private int gold;
    private int level;
    private string nickname;
    public NetManager netManager;
    // Start is called before the first frame update
    void Start()
    {

        instance = this;
        netManager = GetComponent<NetManager>();
        
    }

    private void Update()
    {
        Name.text = nickname;
        Level.text = "LV. " + level;
        Gold.text =gold.ToString();
    }

    public void UpdateResource(int _level,int _gold ,string _name)
    {
        level = _level;
        gold = _gold;
        nickname = _name;
    }

    private void OnDestroy()
    {
        Destroy(instance);
    }
}
