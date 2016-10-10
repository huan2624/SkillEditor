using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;
using System;


// 短连接Socket

class ShortConnectSocket : MonoBehaviour
{
    private const int BUFFER_LEN = 4096;                    //缓冲区默认长度，定义一次发送和接收数据的总长度
    private Socket m_Socket;
    private Action<net.DGPKG> m_RspCallback;
    private byte[] m_SendBuffer;                            //发送数据包的缓冲区
    private byte[] m_RecvBuffer = new byte[BUFFER_LEN];     //接受数据包的缓冲区
    private int m_nRecvSize = 0;
    private byte[] m_MsgBuffer;                             //组包后的数据缓冲区
    private net.DGPKG m_rspPkg = null;
    private bool bRecvFinish = false;
    private float m_timeOut = 5.0f;

    public static bool CreateSesion(string server, int port, byte[] sendMsg, Action<net.DGPKG> rspCallback)
    {
        ShortConnectSocket sesion = new GameObject("ShortConnectSocket").AddComponent<ShortConnectSocket>();
        //UI3System.lockScreen(902/*" 正在连接..."*/, sesion.m_timeOut);

        try
        {
            IPAddress[] IPs = Dns.GetHostAddresses(server);
            sesion.m_Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            sesion.m_Socket.BeginConnect(IPs, port, new AsyncCallback(sesion.ConnectComplete), null);
        }
        catch (System.Net.Sockets.SocketException e)
        {
            Debuger.LogError("Sesion Create Failed, ErrorCode:" + ((SocketError)e.ErrorCode).ToString());
            sesion.Shutdown();
            Destroy(sesion.gameObject);
            sesion = null;
            return false;
        }

        sesion.m_SendBuffer = sendMsg;
        sesion.m_RspCallback = rspCallback;
        return true;
    }

    void OnDestroy()
    {
        //UI3System.unlockScreen();
    }

    void Update()
    {
        if (bRecvFinish && m_rspPkg != null)
        {
            m_RspCallback(m_rspPkg);
            m_rspPkg = null;
            Destroy(gameObject);
            return;
        }

        if ((m_timeOut -= Time.deltaTime) <= 0)
        {
            Destroy(gameObject);
        }
    }

    void Shutdown()
    {
        try
        {
            if (m_Socket != null)
            {
                m_Socket.Shutdown(SocketShutdown.Both);
                m_Socket.Close();
            }
            m_Socket = null;
        }
        catch (System.Net.Sockets.SocketException)
        {
            m_Socket = null;
        }
    }

    void ConnectComplete(IAsyncResult ar)
    {
        try
        {
            m_Socket.EndConnect(ar);
            m_Socket.BeginSend(m_SendBuffer, 0, m_SendBuffer.Length, 0, new AsyncCallback(SendCallBack), 0);
            m_Socket.BeginReceive(m_RecvBuffer, 0, m_RecvBuffer.Length, 0, new AsyncCallback(RecvCallBack), 0);
        }
        catch (System.Net.Sockets.SocketException e)
        {
            Debuger.LogError("Sesion Connect Error, ErrorCode : " + ((SocketError)e.ErrorCode).ToString());
        }
    }

    void SendCallBack(IAsyncResult ar)
    {
        try
        {
            m_Socket.EndSend(ar);
        }
        catch (System.Net.Sockets.SocketException e)
        {
            Debuger.LogError("Sesion Send Error, ErrorCode : " + ((SocketError)e.ErrorCode).ToString());
        }
    }

    void RecvCallBack(IAsyncResult ar)
    {
        try
        {
            int nRead = m_Socket.EndReceive(ar);

            if (m_nRecvSize == 0)
            {
                System.IO.MemoryStream stream = new System.IO.MemoryStream(m_RecvBuffer, 0, sizeof(uint) * 1);
                System.IO.BinaryReader binaryReader = new System.IO.BinaryReader(stream);

                int totalLength = binaryReader.ReadInt32();
                m_MsgBuffer = new byte[totalLength];
            }

            Array.Copy(m_RecvBuffer, 0, m_MsgBuffer, m_nRecvSize, nRead);

            // 如果消息没有完成组包，那么继续组包
            if (m_MsgBuffer != null && m_MsgBuffer.Length > m_nRecvSize + nRead)
            {
                m_Socket.BeginReceive(m_RecvBuffer, 0, m_RecvBuffer.Length, 0, new AsyncCallback(RecvCallBack), 0);
            }

            if (m_MsgBuffer.Length <= m_nRecvSize + nRead)
            {
                m_rspPkg = new net.DGPKG();
                m_rspPkg.doDeserialize(m_MsgBuffer);
                bRecvFinish = true;

                Shutdown();
            }

            m_nRecvSize += nRead;

            Debuger.Log("Recv : " + nRead);

            //msdn If the remote host shuts down the Socket connection with the Shutdown method, and all available data has been received, the EndReceive method will complete immediately and return zero bytes.
            if (nRead == 0)
            {
                //SetErrorState(SocketError.Shutdown);
            }
        }
        catch (System.Net.Sockets.SocketException ex)
        {
            // SetErrorState(ex.SocketErrorCode);
        }
    }
}