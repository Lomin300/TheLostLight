using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


//PacketQueue는 서버와의 송수신이 늦어질 것을 대비해 만약 바로 송수신 처리를 못한다면
//큐(메모리버퍼)에 넣어놨다가 하나씩 처리한다.
public class PacketQueue
{
    //패킷의 크기 정보 구조체
    struct PacketInfo
    {
        public int offset;
        public int size;
    }

    //스트림 데이터를 파일이나 소켓대신 메모리에 직접 출력함
    private MemoryStream m_streamBuffer;
    //여러개의 패킷정보를 리스트에서 관리
    private List<PacketInfo> m_offsetList;

    private int m_offset = 0;

    //생성자
    public PacketQueue()
    {
        m_streamBuffer = new MemoryStream();
        m_offsetList = new List<PacketInfo>();
    }

    //메모리퍼버에 패킷을 저장하기위한 함수
    public int Enqueue(byte[] data,int size)
    {
        PacketInfo info = new PacketInfo();

        info.offset = m_offset;
        info.size = size;
        //패킷 저장 정보를 보존
        m_offsetList.Add(info);

        //패킷 데이터 보존
        //메모리버퍼에 패킷 데이터를 저장
        m_streamBuffer.Position = m_offset;
        m_streamBuffer.Write(data, 0, size);
        m_streamBuffer.Flush();
        m_offset += size;

        return size;
    }

    //메모리버퍼에 패킷을 가져오는 함수
    public int Dequeue(ref byte[] buffer,int size)
    {
        //뺄 정보가 없으면 함수종료
        if (m_offsetList.Count <= 0)
        {
            return -1;
        }

        PacketInfo info = m_offsetList[0];

        //버퍼에서 해당하는 패킷 데이터를 가져온다
        int dataSize = Math.Min(size, info.size);
        m_streamBuffer.Position = info.offset;
        int recvSize = m_streamBuffer.Read(buffer, 0, dataSize);
        
        //가져오면 리스트에서 삭제
        if (recvSize > 0)
        {
            m_offsetList.RemoveAt(0);
        }

        //리스트가 비었으면 확인차 메모리버퍼에 데이터를 지움
        if(m_offsetList.Count == 0)
        {
            Clear();
            m_offset = 0;
            
        }

        return recvSize;
    }

    public void Clear()
    {
        byte[] buffer = m_streamBuffer.GetBuffer();
        Array.Clear(buffer, 0, buffer.Length);

        m_streamBuffer.Position = 0;
        m_streamBuffer.SetLength(0);
    }
}
