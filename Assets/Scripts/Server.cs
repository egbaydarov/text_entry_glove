using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;

public class Server : MonoBehaviour
{

    public string mytext;

    Socket udpSocket;

    private TcpListener Listener = null;

    private static Socket Client = null;

    public static float x;
    public static float y;

    public static bool isDown = false;

    static string data = "";

    static private Shift shift;

    static private Image im;

    static private GameObject go;

    static int BROADCAST_PORT = 9876;
    static int DATA_PORT = 1488;

    static IPAddress broadcast = null;
    IPEndPoint ep = null;

    int keyboard_x;
    int keyboard_y;

    void Start()
    {

        NetworkSetup();

        broadcast = FindBroadcastAdress();
        ep = new IPEndPoint(broadcast, BROADCAST_PORT);

        Debug.Log($"broadcast ip: {broadcast}");


        FindClient();

        Thread textUpdate = new Thread(new ThreadStart(ReadFromClient));
        textUpdate.IsBackground = true;
        textUpdate.Start();


        go = GameObject.Find("keyboard");
        shift = go.GetComponent<Shift>();
        im = GetComponent<Image>();

    }

    IPAddress FindBroadcastAdress()
    {
        IPAddress broadcast = null;
        IPAddress hostIP = null;

        foreach (var netInterface in NetworkInterface.GetAllNetworkInterfaces())
        {
            if (netInterface.NetworkInterfaceType == NetworkInterfaceType.Wireless80211
                || netInterface.NetworkInterfaceType == NetworkInterfaceType.Ethernet)
            {
                var address = netInterface.GetIPProperties().UnicastAddresses[netInterface.GetIPProperties().UnicastAddresses.Count - 1];

                hostIP = address.Address;

                var addressInt = BitConverter.ToInt32(address.Address.GetAddressBytes(), 0);

                var maskInt = BitConverter.ToInt32(address.IPv4Mask.GetAddressBytes(), 0);
                var broadcastInt = addressInt | ~maskInt;
                broadcast = new IPAddress(BitConverter.GetBytes(broadcastInt));
                ep = new IPEndPoint(broadcast, BROADCAST_PORT);
            }
        }

        IPAddress[] localIPs = Dns.GetHostAddresses(Dns.GetHostName());
        bool isLocal = false;

        foreach (IPAddress localIP in localIPs)
        {
            if (hostIP.Equals(localIP))
            {
                isLocal = true;
                break;
            }
        }

        if (broadcast == null || !isLocal)
            throw new ApplicationException("Broadcast IP NOT FOUND.");

        return broadcast;
    }

    void FindClient()
    {
        new Thread(() =>
        {
            while (true)//TODO можно и красиво написать на самом деле, но потом
                try
                {
                    Debug.Log("Waiting for client . . .");

                    Task<Socket> connection = Listener.AcceptSocketAsync();

                    while (!connection.IsCompleted)
                        BroadcastIP();

                    Client = connection.Result;

                    byte[] bytes = new byte[1024];
                    int length = Client.Receive(bytes);
                    var client_xy = Encoding.UTF8.GetString(bytes, 0, length).Split(' ');
                    keyboard_y = int.Parse(client_xy[0]);
                    keyboard_x = int.Parse(client_xy[1]);
                    Debug.Log($"Height - {keyboard_y}, Width - {keyboard_x}");

                    Debug.Log("Socket connected");
                    break;
                }
                catch
                {
                    
                }
        }).Start();
    }


    void Update()
    {
        if (isDown)
        {
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
            Debug.Log($"Sent: {message}");
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

        udpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        udpSocket.DontFragment = true;
        udpSocket.EnableBroadcast = true;
        udpSocket.MulticastLoopback = false;

    }

    private void BroadcastIP()
    {
        var addr = Dns.GetHostEntry(Dns.GetHostName()).AddressList;

        var data = Encoding.UTF8.GetBytes(addr[addr.Length - 1].ToString());
        udpSocket.SendTo(data, ep);
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