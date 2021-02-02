using System;
using System.Collections;
using System.Collections.Generic;
using UdpKit;
using UnityEngine;

public class Menu : Bolt.GlobalEventListener
{

    public int StartNum;
    public bool isServer;
    public void Start()
    {
        StartNum = 0;
        StartGame();
    }
    public void StartGame()
    {
        
        if (isServer)
        {
            BoltLauncher.StartServer();
        }
        else
        {
            BoltLauncher.StartClient();
        }
    }

    public override void BoltStartDone()
    {
        if (BoltNetwork.IsServer)
        {
            string matchName = "Menu";

            BoltNetwork.SetServerInfo(matchName, null);
            BoltNetwork.LoadScene("Menu");
        }
    }

    public override void Connected(BoltConnection connection)
    {
        BoltNetwork.LoadScene("InGame");
    }
    public override void SessionListUpdated(Map<Guid, UdpSession> sessionList)
    {
        foreach(var session in sessionList)
        {
            UdpSession photonSession = session.Value as UdpSession;

            if(photonSession.Source == UdpSessionSource.Photon)
            {
                BoltNetwork.Connect(photonSession);
            }
        }
    }
}
