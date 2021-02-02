using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Threading;

//TransportTCP에서 실질적으로 서버와의 연결과 송수신을 담당함
public class TransportTCP : MonoBehaviour
{
    //서버와의 접속용 소켓
    private Socket m_socket;
    //송신 버퍼
    private PacketQueue m_sendQueue;
    //수신 버퍼
    private PacketQueue m_recvQueue;

    private bool m_isConnected = false;

    //이것은 아직은 쓸모없음
    public delegate void EventHandler(NetEventState state);
    private EventHandler m_handler;

    protected bool m_threadLoop = false;
    protected Thread m_thread = null;
    //버퍼 크기
    private static int s_mtu = 1400;

    // Start is called before the first frame update
    void Start()
    {
        m_sendQueue = new PacketQueue();
        m_recvQueue = new PacketQueue();
    }

    public bool Connect(string address,int port)
    {
        Debug.Log("TransportTCP connect called");

        bool ret = false;

        try
        {
            //소켓 생성과 서버와의 연결
            m_socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream,
                ProtocolType.Tcp);
            m_socket.NoDelay = true;
            m_socket.Connect(address, port);

            //서버와의 연결을 성공하면 송수신을 담당할 스레드 실행
            ret = LaunchThread();
            
        }
        catch
        {
            m_socket = null;
        }

        if(ret == true)
        {
            m_isConnected = true;
            Debug.Log("Connection Success.");
        }
        else
        {
            m_isConnected = false;
            Debug.Log("Connect fail");
        }

        //아직은 필요없음
        if(m_handler != null)
        {
            NetEventState state = new NetEventState();
            state.type = NetEventType.Connect;
            state.result = (m_isConnected == true) ? NetEventResult.Success : NetEventResult.Failure;
            m_handler(state);
            Debug.Log("event handler called");
        }

        return m_isConnected;
    }

    public void Disconnect()
    {
        m_isConnected = false;
        if(m_socket != null)
        {
            //소켓종료
            m_socket.Shutdown(SocketShutdown.Both);
            m_socket.Close();
            m_socket = null;
        }

        //아직은 쓸모없음
        if(m_handler != null)
        {
            NetEventState state = new NetEventState();
            state.type = NetEventType.Disconnect;
            state.result = NetEventResult.Success;
            m_handler(state);
        }

    }

    public int Send(byte[] data,int size)
    {
        if(m_sendQueue == null)
        {
            return 0;
        }
        //보낼 데이터를 큐에 넣음
        return m_sendQueue.Enqueue(data, size);
    }

    public int Receive(ref byte[] buffer,int size)
    {
        if (m_recvQueue == null)
        {
            return 0;
        }
        //서버에서 보낸데이터를 큐에서 가져옴
        return m_recvQueue.Dequeue(ref buffer, size);
    }
    
    //아직은 쓸모없음
    public void RegisterEventHandler(EventHandler handler)
    {
        m_handler += handler;
    }

    //아직은 쓸모없음
    public void UnregisterEventHandler(EventHandler handler)
    {
        m_handler -= handler;
    }

    bool LaunchThread()
    {

        try
        {
            //실질적으로 스레드 시작함
            m_threadLoop = true;
            m_thread = new Thread(new ThreadStart(Dispatch));
            m_thread.Start();
        }
        catch
        {
            Debug.Log("Cannot launch thread");
            return false;
        }
        return true;
    }

    //Dispatch 스레드 : send나 recv를 할 것이 없어도 계속 함수를 실행함...
    //기능에 하자가 있으면 여기부분을 제일 처음으로 바꿔야함
    public void Dispatch()
    {
        Debug.Log("Dispatch thread started");

        while (m_threadLoop)
        {
            if(m_socket != null && m_isConnected == true)
            {
                //송신처리
                DispatchSend();
                //수신처리
                DispatchRecv();
            }
        }
        Debug.Log("Dispatch thread ended");
    }

    void DispatchSend()
    {
        try
        {
            if (m_socket.Poll(0, SelectMode.SelectWrite))
            {
                byte[] buffer = new byte[s_mtu];

                int sendSize = m_sendQueue.Dequeue(ref buffer, buffer.Length);
                while (sendSize > 0)
                {
                    m_socket.Send(buffer, sendSize, SocketFlags.None);
                    sendSize = m_sendQueue.Dequeue(ref buffer, buffer.Length);
                }
            }
        }
        catch
        {
            return;
        }
    }

    void DispatchRecv()
    {
        try
        {
            while (m_socket.Poll(0, SelectMode.SelectRead))
            {
                byte[] buffer = new byte[s_mtu];

                int recvSize = m_socket.Receive(buffer, buffer.Length, SocketFlags.None);
                if (recvSize == 0)
                {
                    Debug.Log("Disconnect recv from server");
                    Disconnect();
                }
                else if (recvSize > 0)
                {
                    m_recvQueue.Enqueue(buffer, recvSize);
                }
            }
        }
        catch
        {
            return;
        }
    }

    public bool IsConnected()
    {
        return m_isConnected;
    }
}
