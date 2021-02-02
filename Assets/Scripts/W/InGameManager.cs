using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class InGameManager : Bolt.EntityEventListener<IICustomCanvas>
{
    public GameObject ServerHero;
    public GameObject ClientHero;
    public GameObject go;
    public GameObject endPanel;
    private void Start()
    {
        go = null;

        if (BoltNetwork.IsServer)
        {
            go = BoltNetwork.Instantiate(BoltPrefabs.Dwarf, ServerHero.transform.position, Quaternion.identity);
        }
        else if (BoltNetwork.IsClient)
        {
            go = BoltNetwork.Instantiate(BoltPrefabs.Dokkebi, ClientHero.transform.position, Quaternion.identity);
            go.GetComponent<SpriteRenderer>().flipX = true;
        }

    }
    public void OnClickEnd()
    {
        SceneManager.LoadScene("02.Lobby");
    }
}
