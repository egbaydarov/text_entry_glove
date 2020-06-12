using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Server : MonoBehaviour
{

    public string mytext;

    Socket udpClient;

    private TcpListener Listener = null;

    private static Socket Client = null;

    public static float x;
    public static float y;

    public static bool isDown = false;

    static string data = "";

    static private Shift shift;

    static private Image im;

    static private GameObject go;

    int BROADCAST_PORT = 9876;
    int DATA_PORT = 1488;

    void Start()
    {

        NetworkSetup();

        Debug.Log($"Server: {Listener.LocalEndpoint}");

        var addr = Dns.GetHostEntry(Dns.GetHostName()).AddressList;
        Debug.Log($"local ip: {addr[addr.Length - 1].MapToIPv4()}");

        FindClient();

        Thread textUpdate = new Thread(new ThreadStart(ReadFromClient));
        textUpdate.IsBackground = true;
        textUpdate.Start();


        go = GameObject.Find("keyboard");
        shift = go.GetComponent<Shift>();
        im = GetComponent<Image>();

    }

    void FindClient()
    {
        new Thread(() =>
        {
            Debug.Log("Waiting for client . . .");

            Task<Socket> connection = Listener.AcceptSocketAsync();

            while (!connection.IsCompleted)
                BroadcastIP();

            Client = connection.Result;
            Debug.Log("Socket connected");
        }).Start();
    }


    void Update()
    {
        if (isDown)
        {
            Debug.Log(x + " " + y + " ");
            data += ((x + 540) + ";" + (-y + 1950) + ";").ToString();
        }
    }

    bool SocketConnected(Socket s)
    {
        bool part1 = s.Poll(1000, SelectMode.SelectRead);
        bool part2 = (s.Available == 0);
        if (part1 && part2)
            return false;
        else
            return true;
    }

    public void ReadFromClient()
    {
        new Thread(() =>
        {
            byte[] bytes = new byte[1024];

            while (true)
            {
                try
                {

                    if (Client == null)
                        continue;

                    if (!SocketConnected(Client))
                    {
                        Client = null;
                        FindClient();
                        continue;
                    }


                    Debug.Log("Waiting for data from socket . . .");

                    int length = Client.Receive(bytes);

                    if (length != 0)
                    {
                        string clientMessage = Encoding.UTF8.GetString(bytes, 0, length).Split('\0')[0];
                        Debug.Log("Recieved text: " + clientMessage);
                        mytext = clientMessage;
                    }
                }
                catch (SocketException socketException)
                {
                    Debug.Log("ReadFromClient: " + socketException.ToString());
                }
            }
        }).Start();
    }


    public static void SendToClient(string message)
    {
        try
        {
            Client.Send(Encoding.ASCII.GetBytes(message));
        }
        catch (SocketException socketException)
        {
            Debug.Log("SendToClient: " + socketException);
        }
    }

    private void NetworkSetup()
    {
        Listener = new TcpListener(IPAddress.Parse("0.0.0.0"), DATA_PORT);
        Listener.Start();

        udpClient = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        

    }

    private void BroadcastIP()
    {
        var addr = Dns.GetHostEntry(Dns.GetHostName()).AddressList;
        IPAddress broadcast = IPAddress.Parse("192.168.1.255");
        IPEndPoint ep = new IPEndPoint(broadcast, BROADCAST_PORT);


        var data = Encoding.UTF8.GetBytes(addr[addr.Length - 1].ToString());
        udpClient.SendTo(data, ep);
    }


    private void OnApplicationQuit()
    {
        Listener.Stop();
    }

    public static void OnPointerUp()
    {
        isDown = false;

        if (Client != null && Client.Connected)
            SendToClient(data + "\r\n");

        data = "";
    }

    public static void OnPointerDown()
    {
        isDown = true;

    }
    public static void shiftReset()
    {
        if (shift.i == 1)
        {
            im.sprite = shift.small;
            shift.i = 0;
        }
    }
}