using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NetEventType
{

    Connect = 0, //접속 이벤트.
    Disconnect, //끊기 이벤트
    SendError, //송신 오류
    ReceiveError, //수신 오류
}

public enum NetEventResult
{
    Failure = -1, //실패
    Success = 0, //성공
}

public enum NetMessage
{
    REQ_MATCHING =1000,
    NOTICE_MATCHING,
    REQ_MATCHINGCANCEL,
    NOTICE_MATCHINGCANCEL,
    REQ_FIRSTLOGIN,
    REQ_LOGIN,
    NOTICE_LOGIN,
    REQ_UPDATEDATA,
    NOTICE_UPDATEDATA,
    REQ_UPDATECARD,
    NOTICE_UPDATECARD,
    REQ_INITMULLIGAN,
    NOTICE_INITMULLIGAN,
    REQ_MULLIGANFINISH,
    NOTICE_MULLIGANFINISH,
    REQ_SUMMONSPWAN,
    NOTICE_SUMMONSPAWN,
    REQ_MAT,
    NOTICE_MAT,
    REQ_MATCANCEL,
    NOTICE_MATCANCEL,

}

public class NetEventState
{
    public NetEventType type; //이벤트 타입
    public NetEventResult result; //이벤트 결과
}
public class PACKET_HEADER
{
    public int nID;
    public int nSize;
}

public class PKT_REQ_MATCHING:PACKET_HEADER
{
    public void Init()
    {
        nID = (int)NetMessage.REQ_MATCHING;
        nSize = 0;
    }
}

public class PKT_NOTICE_MATCHING : PACKET_HEADER
{
    public void Init()
    {
        nID = 0;
        nSize = 0;
    }
}

public class PKT_REQ_MATCHINGCANCEL : PACKET_HEADER
{

    public void Init()
    {
        nID = (int)NetMessage.REQ_MATCHINGCANCEL;
        nSize = 0;
    }
}

public class PKT_NOTICE_MATCHINGCANCEL : PACKET_HEADER
{
    public void Init()
    {
        nID = 0;
        nSize = 0;
    }
}

public class PKT_REQ_FIRSTLOGIN : PACKET_HEADER
{
    public void Init()
    {
        nID = (int)NetMessage.REQ_FIRSTLOGIN;
        nSize = 0;
    }
}

public class PKT_REQ_LOGIN : PACKET_HEADER
{
    public void Init(int _key)
    {
        nID = (int)NetMessage.REQ_LOGIN;
        nSize = 0;
        key = _key;
    }
    public int key;
}

public class PKT_NOTICE_LOGIN : PACKET_HEADER
{
    public void Init()
    {
        nID = 0;
        nSize = 0;
        key = 0;
    }
    public int key;
}

public class PKT_REQ_UPDATEDATA : PACKET_HEADER
{
    public void Init()
    {
        nID = (int)NetMessage.REQ_UPDATEDATA;
        nSize = 0;
    }
}

public class PKT_NOTICE_UPDATEDATA : PACKET_HEADER
{
    public void Init()
    {
        nID = 0;
        nSize = 0;
        nickname = "";
        level = 0;
        gold = 0;
    }
    public string nickname;
    public int level;
    public int gold;
}

public class PKT_REQ_UPDATECARD : PACKET_HEADER
{
    public void Init()
    {
        nID = (int)NetMessage.REQ_UPDATECARD;
        nSize = 0;
    }
}
[System.Serializable]
public struct CARDINFO
{
    public int level;
    public int cost;
    public string name;

}

public class PKT_NOTICEUPDATECARD : PACKET_HEADER
{

    public void Init()
    {
        nID = 0;
        nSize = 0;
        Cards = new List<CARDINFO>();
    }
    public List<CARDINFO> Cards;
}

public class PKT_REQINITMULLIGAN : PACKET_HEADER
{
    public void Init()
    {
        nID = (int)NetMessage.REQ_INITMULLIGAN;
        nSize = 0;
    }
}

[System.Serializable]
public struct GAMECARDINFO
{
    public int cost;
    public string name;

}

public class PKT_NOTICEINITMULLIGAN : PACKET_HEADER
{

    public void Init()
    {
        nID = 0;
        nSize = 0;
        Mulligan = new List<GAMECARDINFO>();
    }
    public List<GAMECARDINFO> Mulligan;
}

public class PKT_REQMULLIGANFINISH : PACKET_HEADER
{

    public void Init()
    {
        nID = (int)NetMessage.REQ_MULLIGANFINISH;
        nSize = 0;
        isleft = 0;
    }
    public int isleft;
}

public class PKT_NOTICEMULLIGANFINISH : PACKET_HEADER
{
    public void Init()
    {
        nID = 0;
        nSize = 0;
    }
}

public class PKT_REQSUMMONSPAWN : PACKET_HEADER
{

    public void Init()
    {
        nID = (int)NetMessage.REQ_SUMMONSPWAN;
        nSize = 0;
        x = 0;
        y = 0;
    }

    public int x;
    public int y;
   public string name;
}

public class PKT_NOTICESUMMONSPAWN : PACKET_HEADER
{

    public void Init()
    {
        nID = 0;
        nSize = 0;
        isEnemy = 0;
        x = 0;
        y = 0;
    }
    public int isEnemy;
    public string name;
    public float x;
    public float y;
}