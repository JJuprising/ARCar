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
    private static int myProt = 50001;   //�˿�  
    static Socket serverSocket;
    Thread myThread;
    Dictionary<string, Thread> threadDic = new Dictionary<string, Thread>();//�洢�̣߳����������ر��߳�
    //����һ���б��洢�ͻ���
    static List<Socket> ClientConnectionItems = new List<Socket>();

    void Start()
    {
        //������IP��ַ
        IPAddress ip = IPAddress.Parse("127.0.0.1");
        //IPAddress ip = IPAddress.Any; //������ַ
        //Debug.Log(ip.ToString());
        serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        IPEndPoint iPEndPoint = new IPEndPoint(ip, myProt);
        //serverSocket.Bind(new IPEndPoint(ip, myProt));  //��IP��ַ���˿�  
        serverSocket.Bind(iPEndPoint);  //��IP��ַ���˿�  
        serverSocket.Listen(10);    //���10����������  
                                    //Console.WriteLine("creat service {0} success",
                                    //    serverSocket.LocalEndPoint.ToString());

        myThread = new Thread(ListenClientConnect);
        myThread.Start();
        //Console.ReadLine();
        Debug.Log("����������...........");
    }

    void Update()
    {
        
    }

    // �����������������������python�ű���
    // �����ͻ����Ƿ�����  
    private void ListenClientConnect()
    {
        while (true)
        {
            try
            {
                Socket clientSocket = serverSocket.Accept(); //1.����һ��Socket ���տͻ��˷�����������Ϣ û����Ϣʱ����
                clientSocket.Send(Encoding.ASCII.GetBytes("Server Say Hello")); //2.��ͻ��˷��� ���ӳɹ� ��Ϣ
                ClientConnectionItems.Add(clientSocket);
                if (ClientConnectionItems.Count == 2)
                {
                    Debug.Log("��ǰ�ͻ�������Ϊ����");
                }

                Thread receiveThread = new Thread(ReceiveMessage); //3.Ϊ�Ѿ����ӵĿͻ��˴���һ���߳� ���߳���������ͻ��˷��͵���Ϣ
                receiveThread.Start(clientSocket); //4.�����߳�



                //��ӵ��ֵ���
                string clientIp = ((IPEndPoint)clientSocket.RemoteEndPoint).Address.ToString();
                //Debug.Log( clientSocket.LocalEndPoint.ToString()); //��ȡip:�˿ں�
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

    
    private byte[] result = new byte[1024]; //1.�����byteֵ �������1024
    //�����߳̽������� ����Socket��Ϊֵ���룩
    private void ReceiveMessage(object clientSocket)
    {
        Socket myClientSocket = (Socket)clientSocket; //2.ת������Ŀͻ���Socket
        while (true)
        {
            
            try
            {
                //��������  
                int receiveNumber = myClientSocket.Receive(result); //3.���ͻ��˵õ���byteֵд��
                
                //Debug.Log(receiveNumber);
                //Debug.Log("----------------------------------------");
                //Debug.Log(receiveNumber);//�ӽ�����
                if (receiveNumber > 0)
                {
                    //����Ϣ����С��
                    if (ClientConnectionItems.Count == 2)
                    {
                        ClientConnectionItems[1].Send(result);
                    }
                    // ��ȡ�۵��ź�
                    String message = Encoding.UTF8.GetString(result, 0, receiveNumber);
                    string[] b = message.Split(',');
                    string EOGMes = b[0][1..];
                    StaticData.Eegsteerscore = long.Parse(b[12][1..]);//��������ֵ
                    if (long.Parse(EOGMes) < 100)
                    {
                        //�۵�С��100��V������ʹ�õ���
                        Debug.Log("client say :" + EOGMes);
                        StaticData.EogUseTool = true;
                    }
                }
                else
                {
                    Debug.Log("client�� " + ((IPEndPoint)myClientSocket.RemoteEndPoint).Address.ToString() + "�Ͽ�����");
                    threadDic[((IPEndPoint)myClientSocket.RemoteEndPoint).Address.ToString()].Abort(); //����߳�
                }
            }
            catch (Exception ex)
            {
                //myClientSocket.Shutdown(SocketShutdown.Both); //���ִ��� �ر�Socket
                Debug.Log(" ������Ϣ" + ex); //��ӡ������Ϣ
                break;
            }
        }
    }
    
    void OnApplicationQuit()
    {
        //�����̱߳���ر� �����´ο�������ִ��� ��������ֵĻ� ֻ������unity�ˣ�
        myThread.Abort();

        //�رտ������߳�
        foreach (string item in threadDic.Keys)
        {
            Debug.Log(item);//de.Key��Ӧ��key/value��ֵ��key
            //item.Value.GetType()
            threadDic[item].Abort();
        }
    }
}
