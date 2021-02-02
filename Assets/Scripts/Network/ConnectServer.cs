using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConnectServer : MonoBehaviour
{
    static public ConnectServer instance;
    public PKT_REQ_FIRSTLOGIN firstLogin = new PKT_REQ_FIRSTLOGIN();
    public PKT_REQ_LOGIN login = new PKT_REQ_LOGIN();
    public PKT_REQ_UPDATEDATA updateData = new PKT_REQ_UPDATEDATA();
    public PKT_REQ_UPDATECARD updateCard = new PKT_REQ_UPDATECARD();
    string message;

    public Text T_ServerMsg;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        if (NetManager.instance.Connect())
        {
           
        }
        else
        {
            Debug.Log("연결 실패");
        }
    }

    private void OnDestroy()
    {
        Destroy(instance);
    }

    public void UpdateData()
    {
        T_ServerMsg.text = NetMessage.NOTICE_UPDATEDATA.ToString();
        updateData.Init();
        string message = JsonUtility.ToJson(updateData);
        updateData.nSize = message.Length;
        message = JsonUtility.ToJson(updateData);
        NetManager.instance.Send(message);
    }

    public void UpdateCard()
    {
        T_ServerMsg.text = NetMessage.NOTICE_UPDATECARD.ToString();
        updateCard.Init();
        string message = JsonUtility.ToJson(updateCard);
        updateCard.nSize = message.Length;
        message = JsonUtility.ToJson(updateCard);
        NetManager.instance.Send(message);
    }
}
