using System;
using System.Net;
using System.Net.Sockets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

public class SocketComManager : MonoBehaviour
{
    //通信用
    private Socket _socket;
    // 接続先サーバーのエンドポイント
    //private static readonly IPEndPoint _serverEndPoint = new IPEndPoint(IPAddress.Loopback, 22222);
    private static readonly IPEndPoint _serverEndPoint = new IPEndPoint(IPAddress.Parse("172.20.129.234"), 22222);
    // 受信用のバッファ
    private static readonly byte[] _buffer = new byte[0x100];
    //表示
    static string uploadData;
    // Start is called before the first frame update
    private static void ReceiveCallback(IAsyncResult ar)
    {
        try
        {
            var socket = ar.AsyncState as Socket;

            // 受信を待機する
            var len = socket.EndReceive(ar);

            //テキストを書き換え
            uploadData = Encoding.Default.GetString(_buffer, 0, len).ToString();
            //Debug.Log(Encoding.Default.GetString(buffer, 0, len).ToString());

            // 再度非同期での受信を開始する
            socket.BeginReceive(_buffer, 0, _buffer.Length, SocketFlags.None, ReceiveCallback, socket);
        }
        //catch (SocketException serr)
        catch (Exception e)
        {
            System.Threading.ManualResetEvent receiveDone = new System.Threading.ManualResetEvent(false);
            receiveDone.Set();
            //Debug.Log(e);
        }
    }
    //接続終了関数
    private void SessionClose()
    {
        if (_socket != null && _socket.Connected)
        {
            _socket.Close();
            _socket.Dispose();
        }
    }
    private void Connect()
    {
        // TCP/IPでの通信を行うソケットを作成する
        _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        // 接続する
        _socket.Connect(_serverEndPoint);
        // 非同期での受信を開始する
        _socket.BeginReceive(_buffer, 0, _buffer.Length, SocketFlags.None, ReceiveCallback, _socket);
    }
    private void getRankingListFromServer(int score)//    
    {
        // 接続する
        Connect();
        var input = score.ToString();
        _socket.Send(Encoding.Default.GetBytes(input));// + Environment.NewLine));

        //接続終了
        SessionClose();
    }
    // Update is called once per frame
    public void SendScoreBySocket(int score)
    {
        getRankingListFromServer(score);
    }
}
