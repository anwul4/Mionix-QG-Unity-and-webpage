/*
    -----------------------
    UDP-Send
    -----------------------
    // [url]http://msdn.microsoft.com/de-de/library/bb979228.aspx#ID0E3BAC[/url]
*/
using UnityEngine;
using System.Collections;
using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

public class UDPSend : MonoBehaviour 
{
	private static int localPort;
	
	// prefs
	private string IP;  // define in init
	
	public int port ;  // define in init
	
	// "connection" things
	IPEndPoint remoteEndPoint;
	UdpClient client;
		
	// call it from shell (as program)
	private static void Main()
	{
		UDPSend sendObj=new UDPSend();
		sendObj.init();
		
		// testing via console
		// sendObj.inputFromConsole();
		
		// as server sending endless

		
	}
	// start from unity3d
	public void Start ()
	{
		init();
	}
	// init
	public void init()
	{
		// Endpunkt definieren, von dem die Nachrichten gesendet werden.
		// Define endpoint from which the messages are sent.
		//print ("UDPSend.init()");
		
		// define
		IP = "172.30.245.235"; //locallocal
		//IP="127.0.0.1"; //local
		//IP = "10.0.0.1"; //adam
		
		//port=8051;
		//port  = 7777;
		
		// ----------------------------
		// Senden
		// ----------------------------
		remoteEndPoint = new IPEndPoint(IPAddress.Parse(IP), port );
		client = new UdpClient();
			
	}
	
	// inputFromConsole
	private void inputFromConsole()
	{
		try
		{
			string text ;
			do
			{
				text  = Console.ReadLine();
				
				// Den Text zum Remote-Client senden.
				//Send the text to the remote client
				if (text  != "")
				{
					
					// Daten mit der UTF8-Kodierung in das Binärformat kodieren.
					// Encode data with the UTF8 encoding to binary format
					byte[] data  = Encoding.UTF8.GetBytes(text );
					
					// Den Text zum Remote-Client senden.
					// Send the text to the remote client
					client.Send(data , data .Length , remoteEndPoint);
				}
			} while (text  != "");
		}
		catch (Exception err)
		{
			print (err.ToString ());
		}
		
	}
	
	// sendData
	// this function should be called if you wish to send a TRIGGER 
	public void sendInt(int message)
	{
		try
		{
			
			
			// Daten mit der UTF8-Kodierung in das Binärformat kodieren.
			// Encode data with the UTF8 encoding to binary format .
			byte[] data  = BitConverter.GetBytes(message);

			// Den message zum Remote-Client senden.
			// Send the message to the remote client .
			client.Send(data , data .Length , remoteEndPoint);

		}
		catch (Exception err)
		{
			print (err.ToString ());
		}
	}


	// endless test


}