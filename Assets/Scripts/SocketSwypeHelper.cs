using Leap.Unity.Geometry;
using System;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class SocketSwypeHelper : MonoBehaviour
{
    bool IsBroadcasting = true;
    bool isProcessing;
    public bool IsConnected;

    public static SocketSwypeHelper instance;


    Socket udpSocket;
    private static TcpListener Listener = null;
    private Socket Client = null;
    IPEndPoint endPoint = null;

    string data = "";

    int BROADCAST_PORT = 9875;
    static int DATA_PORT = 1228;


    [SerializeField]
    int offset_x = 0;
    [SerializeField]
    int offset_y = 55;
    [SerializeField]
    float scale_coef = 1;

    [SerializeField]
    int height = 1920;
    [SerializeField]
    int width = 1080;

    [SerializeField]
    float cur_x = 0;
    [SerializeField]
    float cur_y = 0;

    Server server;

    [SerializeField]
    int VYSOR_HEIGHT = 970;
    [SerializeField]
    int VYSOR_WIDTH = 448;

    private void Update()
    {
        Win32.POINT pt;
        Win32.GetCursorPos(out pt);
        cur_x = pt.X;
        cur_y = pt.Y;
    }



    void Start()
    {
        SocketSwypeHelper prog = this;
        instance = prog;
        prog.NetworkSetup();
        prog.isProcessing = true;
        prog.FindClient();
        //do
        //{
        //    Debug.Log("Press F4 to close . . .");
        //} while (Console.ReadKey().Key != ConsoleKey.F4);
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
        byte[] bytes = new byte[1024 * 1024 * 10];

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

                    Debug.Log("Waiting for data from socket . . . (2)");

                    int length = Client.Receive(bytes);

                    if (length != 0)
                    {
                        data = Encoding.UTF8.GetString(bytes, 0, length);

                        Debug.Log($"Recieved text ({length}): {data} (2)");

                        string[] lines = data.Split('\n');

                        for (int i = 0; i < lines.Length; ++i)
                        {
                            Debug.Log(lines[i]);
                            string[] coords = lines[i].Split(';');
                            float coef_x = VYSOR_WIDTH / (float)Server.instance.keyboard_x;
                            float coef_y = VYSOR_HEIGHT / (float)Server.instance.screen_y;

                            if (coords.Length == 3)
                            {

                                int x = Mathf.RoundToInt(int.Parse(coords[0]) * coef_x);
                                int y = Mathf.RoundToInt(int.Parse(coords[1]) * coef_y) + offset_y;

                                Win32.MoveCursor(x, y);
                                Debug.Log($"MOVE: {x} {y}");
                            }
                            else if (coords[0].Equals("d"))
                            {
                                int x = Mathf.RoundToInt(int.Parse(coords[1]) * coef_x);
                                int y = Mathf.RoundToInt(int.Parse(coords[2]) * coef_y) + offset_y;

                                Win32.MoveCursor(x, y);
                                Debug.Log($"MOVE: {x} {y}");
                                Win32.SendDown();
                            }
                            else if (coords[0].Equals("u"))
                                Win32.SendUp();
                        }

                        //byte[] img = new byte[length];
                        //Array.Copy(bytes, img, length);

                        //OnMessageRecieved.Invoke(data);
                        //OnImageRecieved.Invoke(img);
                    }
                }
                catch (SocketException socketException)
                {
                    Debug.Log("ReadFromClient:  (2)" + socketException.ToString());
                }
        }
    }


    void FindClient()
    {
        new Thread(() =>
        {
            while (isProcessing)
                try
                {
                    IsConnected = false;

                    Task<Socket> connectionTask = Listener.AcceptSocketAsync();

                    while (IsBroadcasting && !connectionTask.IsCompleted)
                    {
                        Thread.Sleep(2000);
                        FindBroadcastAdress();
                    }

                    Client = connectionTask.Result;

                    IsConnected = true;
                    IsBroadcasting = false;

                    Thread textUpdate = new Thread(new ThreadStart(ReadFromClient));


                    textUpdate.IsBackground = true;
                    textUpdate.Start();

                    break;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
        }).Start();
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
                            endPoint = new IPEndPoint(broadcast, BROADCAST_PORT);
                            success = true;
                            BroadcastIP(hostIP.ToString());
                            //Debug.Log($"Broadcasted - {broadcast}");
                        }
                    }
                }
            }
        }


        if (broadcast == null || !success)
        {
            throw new ApplicationException("Broadcast IP NOT FOUND. (2)");
        }

        return broadcast;
    }

    private void BroadcastIP(String text)
    {
        var data = Encoding.UTF8.GetBytes(text);
        udpSocket.SendTo(data, endPoint);
    }

    public void SendToClient(string message)
    {
        try
        {
            Client.Send(Encoding.ASCII.GetBytes(message));
            // Debug.Log($"Sent: {message}");
        }
        catch (SocketException socketException)
        {
            //Debug.Log("SendToClient: " + socketException);
        }
    }
}