using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Server : MonoBehaviour
{

	public string mytext;

	private TcpListener tcpListenerSend;

	private Thread sendThread;

	public static TcpClient sendClient;

	private TcpListener tcpRecieveListener;

	private Thread recieveThread;

	private TcpClient recieveClient;

	public static float x;
	public static float y;

	public static bool isDown = false;

	static string data = "";

	static private Shift shift;

	static private Image im;

	static private GameObject go;

	void Start()
	{

		sendThread = new Thread(new ThreadStart(CreateSendListener));
		sendThread.IsBackground = true;
		sendThread.Start();

		recieveThread = new Thread(new ThreadStart(CreateRecieveListener));
		recieveThread.IsBackground = true;
		recieveThread.Start();

		go = GameObject.Find("keyboard");
		shift = go.GetComponent<Shift>();
		im = GetComponent<Image>();


	}


	void Update()
	{
		if (isDown)
		{
			Debug.Log(x + " " + y + " ");
			data += ((x + 540) + " ; " + (-y + 1950) + " ;").ToString();
		}
	}


	private void CreateRecieveListener()
	{
		try
		{
			tcpRecieveListener = new TcpListener(IPAddress.Parse("0.0.0.0"), 8080);
			tcpRecieveListener.Start();
			Debug.Log("Server is listening");
			Byte[] bytes = new Byte[1024];
			while (true)
			{
				using (recieveClient = tcpRecieveListener.AcceptTcpClient())
				{
					using (NetworkStream stream = recieveClient.GetStream())
					{
						int length;
						while ((length = stream.Read(bytes, 0, bytes.Length)) != 0)
						{
							var incommingData = new byte[length];
							Array.Copy(bytes, 0, incommingData, 0, length);
							string clientMessage = Encoding.UTF8.GetString(incommingData);
							Debug.Log("Recieved text: " + clientMessage);
							mytext = clientMessage;
						}
					}
				}
			}
		}
		catch (SocketException socketException)
		{
			Debug.Log("SocketException " + socketException.ToString());
		}
	}

	private void CreateSendListener()
	{
		try
		{
			// Create listener on localhost port 8080. 			
			tcpListenerSend = new TcpListener(IPAddress.Parse("0.0.0.0"), 8000);
			tcpListenerSend.Start();
			sendClient = tcpListenerSend.AcceptTcpClient();
			NetworkStream stream = sendClient.GetStream();
			Debug.Log("Server is listening");

		}
		catch (SocketException socketException)
		{
			Debug.Log("SocketException " + socketException.ToString());
		}
	}


	public static void Send(string message)
	{
		if (sendClient == null)
		{
			return;
		}
		Vector3 mousePos = Input.mousePosition;
		try
		{
			NetworkStream stream = sendClient.GetStream();
			if (stream.CanWrite)
			{

				byte[] byteArrayMessage = Encoding.ASCII.GetBytes(message);
				stream.Write(byteArrayMessage, 0, byteArrayMessage.Length);
			}
		}
		catch (SocketException socketException)
		{
			Debug.Log("Socket exception: " + socketException);
		}
	}

	private void OnApplicationQuit()
	{
		tcpRecieveListener.Stop();
		tcpListenerSend.Stop();
	}
	public static void OnPointerUp()
	{
		isDown = false;
		Send(data + "\r\n");
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