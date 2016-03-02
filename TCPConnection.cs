/* -------------------- TCP Connection Class ---------------------- *
 * ---------------------------------------------------------------- *
 * Provides interface for TCP Client
 * It is loaded dynamically by ICATEmpaticaBLEClient class.
 * Change conHost and conPort as per your need. 
 *
 * Written By: Deba Saha (dpsaha@vt.edu)
 * Virginia Tech, USA.  */

using UnityEngine;
using System.Collections;
using System;
using System.IO;
using System.Net.Sockets;
using System.Diagnostics;

using Debug = UnityEngine.Debug;

public class TCPConnection : MonoBehaviour {
	
	//the name of the connection, not required but better for overview if you have more than 1 connections running
	public string conName = "Localhost";
	
	//ip/address of the server, 127.0.0.1 is for your own computer
	public string conHost = "192.168.1.4";
	
	//port for the server, make sure to unblock this in your router firewall if you want to allow external connections
	public int conPort = 20;
	
	//a true/false variable for connection status
	public bool socketReady = false;

	public TcpClient mySocket;
	NetworkStream theStream;
	StreamWriter theWriter;
	StreamReader theReader;
	Stopwatch myStopWatch;
	
	//try to initiate connection
	public void setupSocket() {
		try {
			mySocket = new TcpClient(conHost, conPort);
			theStream = mySocket.GetStream();
			theWriter = new StreamWriter(theStream);
			theReader = new StreamReader(theStream);
			socketReady = true;
			mySocket.NoDelay = true;
			myStopWatch = Stopwatch.StartNew();
			Debug.Log("Connection Successful");	
		}
		catch (Exception e) {
			Debug.Log("Socket error:" + e);
		}
	}
	
	//send message to server
	public void writeSocket(string theLine) {
		if (!socketReady)
			return;
		String tmpString = theLine + "\r\n";
		theWriter.Write(tmpString);
		theWriter.Flush();
	}
	
	//read message from server
	public string readSocket() {
		String result = "";
		if (!socketReady)
			return result;
		if (theStream.DataAvailable) {
			Byte[] inStream = new Byte[mySocket.SendBufferSize];
			theStream.Read(inStream, 0, inStream.Length);
			result += System.Text.Encoding.UTF8.GetString(inStream).TrimEnd('\0');
			result += myStopWatch.Elapsed.ToString () + "  " + DateTime.UtcNow;
		}
		return result;
	}
	
	//disconnect from the socket
	public void closeSocket() {
		if (!socketReady)
			return;
		theWriter.Close();
		theReader.Close();
		mySocket.Close();
		socketReady = false;
	}
	
	//keep connection alive, reconnect if connection lost
	public void maintainConnection(){
		if(!theStream.CanRead) {
			setupSocket();
		}
	}
	
	
}
