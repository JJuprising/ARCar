using UnityEngine;
using System.Net.Sockets;
using System.Text;
using System.IO;
using System;
using System.Threading;
using System.Collections.Generic;
using System.Net;

public class SocketClient : MonoBehaviour
{
    private static int myProt = 50001;   //端口  
    static Socket serverSocket;
    Thread myThread;
    Dictionary<string, Thread> threadDic = new Dictionary<string, Thread>();//存储线程，程序结束后关闭线程
    //定义一个列表，存储客户端
    static List<Socket> ClientConnectionItems = new List<Socket>();

    void Start()
    {
        //服务器IP地址
        IPAddress ip = IPAddress.Parse("127.0.0.1");
        //IPAddress ip = IPAddress.Any; //本机地址
        //Debug.Log(ip.ToString());
        serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        IPEndPoint iPEndPoint = new IPEndPoint(ip, myProt);
        //serverSocket.Bind(new IPEndPoint(ip, myProt));  //绑定IP地址：端口  
        serverSocket.Bind(iPEndPoint);  //绑定IP地址：端口  
        serverSocket.Listen(10);    //最多10个连接请求  
                                    //Console.WriteLine("creat service {0} success",
                                    //    serverSocket.LocalEndPoint.ToString());

        myThread = new Thread(ListenClientConnect);
        myThread.Start();
        //Console.ReadLine();
        Debug.Log("服务器启动...........");
    }

    void Update()
    {
        
    }

    // 下面的三个函数是用于连接python脚本的
    // 监听客户端是否连接  
    private void ListenClientConnect()
    {
        while (true)
        {
            try
            {
                Socket clientSocket = serverSocket.Accept(); //1.创建一个Socket 接收客户端发来的请求信息 没有消息时堵塞
                clientSocket.Send(Encoding.ASCII.GetBytes("Server Say Hello")); //2.向客户端发送 连接成功 消息
                ClientConnectionItems.Add(clientSocket);
                if (ClientConnectionItems.Count == 2)
                {
                    Debug.Log("当前客户端数量为两个");
                }

                Thread receiveThread = new Thread(ReceiveMessage); //3.为已经连接的客户端创建一个线程 此线程用来处理客户端发送的消息
                receiveThread.Start(clientSocket); //4.开启线程



                //添加到字典中
                string clientIp = ((IPEndPoint)clientSocket.RemoteEndPoint).Address.ToString();
                //Debug.Log( clientSocket.LocalEndPoint.ToString()); //获取ip:端口号
                if (!threadDic.ContainsKey(clientIp))
                {
                    threadDic.Add(clientIp, receiveThread);
                }
            }
            catch(Exception e)
            {
                Debug.Log(e.Message);
                break;
            }
            
        }
    }

    
    private byte[] result = new byte[1024]; //1.存入的byte值 最大数量1024
    //开启线程接收数据 （将Socket作为值传入）
    private void ReceiveMessage(object clientSocket)
    {
        Socket myClientSocket = (Socket)clientSocket; //2.转换传入的客户端Socket
        while (true)
        {
            
            try
            {
                //接收数据  
                int receiveNumber = myClientSocket.Receive(result); //3.将客户端得到的byte值写入
                
                //Debug.Log(receiveNumber);
                //Debug.Log("----------------------------------------");
                //Debug.Log(receiveNumber);//子节数量
                if (receiveNumber > 0)
                {
                    //把信息发给小车
                    if (ClientConnectionItems.Count == 2)
                    {
                        ClientConnectionItems[1].Send(result);
                    }
                    // 获取眼电信号
                    String message = Encoding.UTF8.GetString(result, 0, receiveNumber);
                    string[] b = message.Split(',');
                    string EOGMes = b[0][1..];
                    StaticData.Eegsteerscore = long.Parse(b[12][1..]);//陀螺仪数值
                    if (long.Parse(EOGMes) < 100)
                    {
                        //眼电小于100μV，触发使用道具
                        Debug.Log("client say :" + EOGMes);
                        StaticData.EogUseTool = true;
                    }
                }
                else
                {
                    Debug.Log("client： " + ((IPEndPoint)myClientSocket.RemoteEndPoint).Address.ToString() + "断开连接");
                    threadDic[((IPEndPoint)myClientSocket.RemoteEndPoint).Address.ToString()].Abort(); //清除线程
                }
            }
            catch (Exception ex)
            {
                //myClientSocket.Shutdown(SocketShutdown.Both); //出现错误 关闭Socket
                Debug.Log(" 错误信息" + ex); //打印错误信息
                break;
            }
        }
    }
    
    void OnApplicationQuit()
    {
        //结束线程必须关闭 否则下次开启会出现错误 （如果出现的话 只能重启unity了）
        myThread.Abort();

        //关闭开启的线程
        foreach (string item in threadDic.Keys)
        {
            Debug.Log(item);//de.Key对应于key/value键值对key
            //item.Value.GetType()
            threadDic[item].Abort();
        }
    }
}
