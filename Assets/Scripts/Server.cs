using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Reflection;
using System.Security.Permissions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

public class Server : MonoBehaviour
{

    public static string mytext = " ";
    public static bool isTextUpdated = false;

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

    private static int unity_keyboard_x = 1080;
    private static int unity_keyboard_y = 660;
    private static int unity_screen_y = 2214;
    public static int keyboard_x;
    public static int keyboard_y;
    int screen_y;
    private float coef_x;
    private float coef_y;
    public static bool isSizeSet;


    public static bool IsConnected;
    bool IsBroadcasting = true;
    bool isProcessing;

    
    public static Stopwatch gest_time = new Stopwatch();
    
    public static Stopwatch move_time = new Stopwatch();
    
    void Start()
    {
        go = GameObject.Find("keyboard");
        shift = go.GetComponent<Shift>();
        im = go.GetComponent<Image>();
        

        NetworkSetup();
        isProcessing = true;

        FindClient();
        
       // move_time.Start();
    }

    IPAddress FindBroadcastAdress()
    {
        IPAddress broadcast = null;
        IPAddress[] localIPs = Dns.GetHostAddresses(Dns.GetHostName());

        bool success = false;

        for (int i = localIPs.Length - 1; i > -1; --i)
        {
            var localIP = localIPs[i];

            foreach (var netInterface in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (netInterface.NetworkInterfaceType == NetworkInterfaceType.Wireless80211
                    || netInterface.NetworkInterfaceType == NetworkInterfaceType.Ethernet)
                {
                    foreach (var address in netInterface.GetIPProperties().UnicastAddresses)
                    {
                        IPAddress hostIP = address.Address;
                        if (hostIP.Equals(localIP))
                        {
                            var addressInt = BitConverter.ToInt32(address.Address.GetAddressBytes(), 0);

                            var maskInt = BitConverter.ToInt32(address.IPv4Mask.GetAddressBytes(), 0);
                            var broadcastInt = addressInt | ~maskInt;
                            broadcast = new IPAddress(BitConverter.GetBytes(broadcastInt));
                            ep = new IPEndPoint(broadcast, BROADCAST_PORT);
                            success = true;
                            BroadcastIP("HI");
                            //Debug.Log($"Broadcasted - {broadcast}");
                        }
                    }
                }
            }
        }


        if (broadcast == null || !success)
        {
            enabled = false;
            throw new ApplicationException("Broadcast IP NOT FOUND.");
        }

        return broadcast;
    }

    void FindClient()
    {
        new Thread(() =>
        {
            while (isProcessing)
                try
                {
                    IsConnected = false;
                    Debug.Log("Waiting for client . . .");

                    Task<Socket> connectionTask = Listener.AcceptSocketAsync();

                    while (IsBroadcasting && !connectionTask.IsCompleted)
                    {
                        Thread.Sleep(2000);
                        FindBroadcastAdress();
                    }

                    Client = connectionTask.Result;

                    Debug.Log("Client connected!");



                    byte[] bytes = new byte[1024];
                    int length = Client.Receive(bytes);
                    var client_xy = Encoding.UTF8.GetString(bytes, 0, length).Split(' ');
                    screen_y = int.Parse(client_xy[0]);
                    keyboard_x = int.Parse(client_xy[1]);
                    keyboard_y = int.Parse(client_xy[2]);
                    coef_x = (float)(keyboard_x / (unity_keyboard_x * 1.0));
                    coef_y = (float)(keyboard_y / (unity_keyboard_y * 1.0));
                    Debug.Log($"Height - {keyboard_y}, Width - {keyboard_x}, Screen Height - {screen_y}");
                    Debug.Log(coef_x + " " + coef_y);
                    Debug.Log("Socket connected");
                    IsConnected = true;
                    IsBroadcasting = false;

                    Thread textUpdate = new Thread(new ThreadStart(ReadFromClient));
                    textUpdate.IsBackground = true;
                    textUpdate.Start();
                    break;
                }
                catch (Exception ex)
                {
                    Debug.LogError(ex.Message);
                }
        }).Start();
    }


    void Update()
    {
        if (isDown)
        {
            //Debug.Log("x: " + x + " ; y: " + y);
            float data_x = (float)(x * coef_x + keyboard_x / 2.0);
            float data_y = (float)(-y * coef_y + screen_y - (keyboard_y / 2.0));
            data += (data_x + ";" + data_y + ";").ToString();
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
        byte[] bytes = new byte[1024];

        while (isProcessing)
        {
            if (IsConnected)
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
                        string[] data = Encoding.UTF8.GetString(bytes, 0, length).Split('\n');
                        string clientMessage = data.Aggregate("", (max, cur) => max.Length > cur.Length ? max : cur);
                        Debug.Log("Recieved text: " + clientMessage);
                        if (clientMessage[0] == '\r')
                            clientMessage = "";
                        mytext = clientMessage;
                        isTextUpdated = true;
                        
                    }
                }
                catch (SocketException socketException)
                {
                    Debug.Log("ReadFromClient - " + socketException.ToString());
                }
        }
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

    private void BroadcastIP(String text)
    {
        var data = Encoding.UTF8.GetBytes(text);
        udpSocket.SendTo(data, ep);
    }


    public static void OnPointerUp()
    {
        isDown = false;
        gest_time.Stop();
        move_time.Start();
        if (Client != null && Client.Connected)
            SendToClient(data + "\r\n");

        data = "";
        
    }

    public static void OnPointerDown()
    {
        isDown = true;
        gest_time.Start();
        move_time.Stop();
        if(!EntryProcessing.full_time.IsRunning)
            EntryProcessing.full_time.Start();

    }
    public static void shiftReset()
    {
        if (shift.i == 1)
        {
            im.sprite = shift.small;
            shift.i = 0;
        }
    }

    private void OnApplicationQuit()
    {
        isProcessing = false;

        if (Client != null && Client.Connected)
            Client.Close();
        IsConnected = false;
        IsBroadcasting = false;
        Listener.Stop();
        udpSocket.Close();
    }

    private void Awake()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("Server");

        if (objs.Length > 1)
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this.gameObject);
    }
}