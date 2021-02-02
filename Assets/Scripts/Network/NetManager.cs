using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class NetManager : MonoBehaviour
{
    //이것은 다른 클래스에서 NetManager.instance.Send() 이런 식으로 쓰일것임
    public static NetManager instance;
    public TransportTCP m_transport;
    //서버의 ip주소와 포트번호
    private string m_hostAddress = "127.0.0.1";
    private int m_port = 9000;
    //매프레임마다 해야하는 Update가 달라져야할 때 쓰임
    public NetState m_state = NetState.NOT_CONNECTED;
    public PACKET_HEADER packet;
    public PKT_NOTICE_MATCHING Notice_Matching;
    public PKT_NOTICE_UPDATEDATA Game_Resource;
    private NetMessage m_msgState;
    public enum NetState
    {
        NOT_CONNECTED = 0,
        CONNECTED,
        ROBBYCONNECTED,
        GAMECONNECTED,
        CLOSING,
        ERROR,
    }

    //유니티는 씬이 바뀔 때 오브젝트가 다 사라지지만 이렇게 쓰면 오브젝트가 살아있음
    private void Awake()
    {
        if(instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
            m_transport = GetComponent<TransportTCP>();
            Notice_Matching = new PKT_NOTICE_MATCHING();
            Notice_Matching.Init();
        }
    }



    // Update is called once per frame "01.ConnectServer"
    void Update()
    {
        
        switch (m_state)
        {
            case NetState.NOT_CONNECTED:
                break;

            case NetState.CONNECTED:
                UpdateConnect();
                break;

            case NetState.ROBBYCONNECTED:
                UpdateRobby();
                break;

            case NetState.GAMECONNECTED:
                UpdateGame();
                break;
            case NetState.CLOSING:

                break;

        }   
    }

    //다른 클래스에서 쓸 함수
    //서버와 연결
    public bool Connect()
    {
        bool ret = m_transport.Connect(m_hostAddress, m_port);

        if (ret)
        {
            m_state = NetState.CONNECTED;
        }
        else
        {
            m_state = NetState.ERROR;
        }

        return ret;
    }

    //다른 클래스에서 쓸 함수
    //서버에 송신할 때
    public void Send(string message)
    { 
       
        byte[] buffer = System.Text.Encoding.ASCII.GetBytes(message);
        m_transport.Send(buffer, buffer.Length);
    }

    public void Send(string[] msg)
    {
        string message = "";
        message = SendPassing(message, msg);
        byte[] buffer = System.Text.Encoding.ASCII.GetBytes(message);
        m_transport.Send(buffer, buffer.Length);

    }
    //다른 클래스에서 쓸 함수
    //서버한테 수신할 때
    public int Receive(ref byte[] buffer,int size)
    {

        int recvSize = m_transport.Receive(ref buffer, size);

        return recvSize;
    }
    public void UpdateConnect()
    {
        byte[] buffer = new byte[1024];
      
        
        int recvSize = m_transport.Receive(ref buffer, buffer.Length);

        if (recvSize > 0)
        {
            string message = System.Text.Encoding.UTF8.GetString(buffer);
            string[] result = RecvPassing(message);

            m_msgState = (NetMessage)Enum.Parse(typeof(NetMessage), result[0]);
            switch (m_msgState)
            {

                case NetMessage.NOTICE_MAT:
                    int mat = Convert.ToInt32(result[1]);
                    if (mat ==0)
                    {
                        BoltLauncher.StartServer();
                    }
                    else if(mat == 1)
                    {
                        BoltLauncher.StartClient();
                    }
                    break;
            }
        }
    }

    private string SendPassing(string msg, string[] packedMSG)
    {

        for (int i = 0; i < packedMSG.Length; i++)
        {
            msg += packedMSG[i] + "/";
        }

        Debug.Log("SEND" + msg);
        return msg;
    }


    public string[] RecvPassing(string msg)
    {
        string[] result = msg.Split(new char[] { '/' });
        Debug.Log("RECV" + result[0]);
        return result;
    }

    public void UpdateRobby()
    {
        byte[] buffer = new byte[256];

        int recvSize = m_transport.Receive(ref buffer, buffer.Length);

        if (recvSize > 0)
        {
            string message = System.Text.Encoding.UTF8.GetString(buffer);
            Debug.Log(message);

            packet = JsonUtility.FromJson<PACKET_HEADER>(message);

            switch (packet.nID)
            {
                case (int)NetMessage.NOTICE_MATCHING:
                    m_state = NetState.GAMECONNECTED;
                    SceneManager.LoadScene("04.InGame1");
                    PKT_REQINITMULLIGAN mulligans = new PKT_REQINITMULLIGAN();
                    mulligans.Init();
                    message = JsonUtility.ToJson(mulligans);
                    mulligans.nSize = message.Length;
                    message = JsonUtility.ToJson(mulligans);
                    Send(message);
                    break;

                case (int)NetMessage.NOTICE_MATCHINGCANCEL:
                    SceneManager.LoadScene("02.Lobby");
                    StartCoroutine(StartUpdateResource());
                    break;

                default:
                    break;
            }

        }

    }
    public void UpdateGame()
    {
        byte[] buffer = new byte[1024];

        int recvSize = m_transport.Receive(ref buffer, buffer.Length);

        if (recvSize > 0)
        {
            string message = System.Text.Encoding.UTF8.GetString(buffer);
            Debug.Log(message);

            packet = JsonUtility.FromJson<PACKET_HEADER>(message);

            switch (packet.nID)
            {
                case (int)NetMessage.NOTICE_INITMULLIGAN:
                    PKT_NOTICEINITMULLIGAN mulligan = new PKT_NOTICEINITMULLIGAN();
                    mulligan.Init();
                    mulligan = JsonUtility.FromJson<PKT_NOTICEINITMULLIGAN>(message);
                    //UnitInventory.Instance.ServerUnitData(mulligan.Mulligan);
                    break;

                case (int)NetMessage.NOTICE_MULLIGANFINISH:
                    SelectDestroy.instance.DestroySelectObject();
                    BattleCardManagers.Instance.isActive = true;
                    break;

                case (int)NetMessage.NOTICE_SUMMONSPAWN:
                    GameObject temp;

                    PKT_NOTICESUMMONSPAWN summonSpawn = new PKT_NOTICESUMMONSPAWN();
                    summonSpawn.Init();
                    summonSpawn = JsonUtility.FromJson<PKT_NOTICESUMMONSPAWN>(message);
                    temp = Instantiate(GameObject.Find(summonSpawn.name).gameObject,
                       new Vector3((float)summonSpawn.x,(float)summonSpawn.y,0), Quaternion.identity);
                    if(summonSpawn.isEnemy == 1)
                    {
                        Vector3 scale = temp.transform.localScale;
                        scale.x *= -1;
                        temp.transform.localScale = scale;
                    }
                    temp.GetComponent<UnitState>().Setting();

                    break;
            }
        }

    }

    private void OnDestroy()
    {
        Destroy(instance);
    }//아아아ㅏ니리이ㅣ이이이이

    IEnumerator StartUpdateResource()
    {
        yield return new WaitForSeconds(0.01f);
        ResourceUI.instance.UpdateResource(Game_Resource.level, Game_Resource.gold, Game_Resource.nickname);
    }
}
