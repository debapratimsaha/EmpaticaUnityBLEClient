/* -- ICAT's Empatica Bluetooth Low Energy(BLE) Comm Client -- *
 * ----------------------------------------------------------- *
 * 0. Attach this to main camera or any empty game object
 * 1. On launch, it tries to connect to the localhost/port20 
 * 	  (You have to change it to your own ip/port combination).
 * 2. Enter the Device ID and connect to device.
 * 3. Select the data streams to log and hit "Log Data"
 * 4. Hit Ctrl+Shift+Z to disconnect at anytime.
 * 
 * Written By: Deba Saha (dpsaha@vt.edu)
 * Virginia Tech, USA.  */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System; 
using System.IO;
using System.Diagnostics;

using Debug = UnityEngine.Debug;

public class ICATEmpaticaBLEClient : MonoBehaviour {	
	//variables	
	private TCPConnection myTCP;	
	private string streamSelected;
	public string msgToServer;
	public string connectToServer;
	
	private string savefilename = "name" + DateTime.UtcNow.ToString("dd_mm_yyyy_hh_mm_ss") + ".txt";

	//flag to indicate device conection status
	private bool deviceConnected = false;

	//flag to indicate if data to be logged to file
	private bool logToFile = false;

	void Awake() {		
		//add a copy of TCPConnection to this game object		
		myTCP = gameObject.AddComponent<TCPConnection>();		
	}
	
	void Start () {		
		//DisplayTimerProperties ();
		if (myTCP.socketReady == false) {			
			Debug.Log("Attempting to connect..");
			//Establish TCP connection to server
			myTCP.setupSocket();
		}
	}

	void Update () {		
		//keep checking the server for messages, if a message is received from server, 
		//it gets logged in the Debug console (see function below)		
		SocketResponse ();
	}
		
	void OnGUI() {		
		//if connection has not been made, display button to connect		
		if (myTCP.socketReady == false) {			
			if (GUILayout.Button ("Connect")) {	
				Debug.Log("Attempting to connect..");
				//Establish TCP connection to server
				myTCP.setupSocket();
			}
		}

		//once TCP connection has been made, connect to Empatica device
		if (myTCP.socketReady == true && deviceConnected == false){
			if (GUILayout.Button ("Device List")) {	
				// ask to pupulate device list
				SendToServer("device_list");
			}
			connectToServer = GUILayout.TextField(connectToServer);
			if (GUILayout.Button ("Connect to Device", GUILayout.Height(30))) {				
				SendToServer("device_connect " + connectToServer);
				Debug.Log("Connected to Empatica. Press Ctrl+Shift+Z to disconnect Empatica at any time");
			}
		}

		//once device has been connected, choose which streams to select and start logging	
		if (myTCP.socketReady == true && deviceConnected == true && logToFile == false) {			
			msgToServer = GUILayout.TextField(msgToServer);
			if (GUILayout.Button ("Write to server", GUILayout.Height(30))) {				
				SendToServer(msgToServer);
			}

			//Buttons for selecting data streams
			streamSelected = GUILayout.TextField(streamSelected);
			if (GUILayout.Button ("Galvanic Skin Response")){
				SendToServer("device_subscribe gsr ON");
				streamSelected += "GSR ";
			}
			if (GUILayout.Button ("Accelerometer")){
				SendToServer("device_subscribe acc ON");
				streamSelected += "ACC ";
			}
			if (GUILayout.Button ("Blood Volume Pulse")){
				SendToServer("device_subscribe bvp ON");
				streamSelected += "BVP ";
			}
			if (GUILayout.Button ("Inter Beat Interval")){
				SendToServer("device_subscribe ibi ON");
				streamSelected += "IBI ";
			}
			if (GUILayout.Button ("Skin Temperature")){
				SendToServer("device_subscribe tmp ON");
				streamSelected += "TMP ";
			}

			//Button for logging data to file
			if (GUILayout.Button("Log Data")){
				logToFile = true;
				Debug.Log("Started Logging data. Press Ctrl+Shift+Z to disconnect Empatica at any time");
			}
		}

		//button combination for disconnecting
		Event e = Event.current;
		if (myTCP.socketReady == true && deviceConnected == true) {
			if (e.type == EventType.KeyDown && e.control && e.shift && e.keyCode == KeyCode.Z){
				Debug.Log("Disconnecting Device and TCP connection...");
				//disconnect Empatica
				SendToServer("device_disconnect");
				//disconnect TCP
				myTCP.closeSocket();

				//reset all flags
				deviceConnected = false;
				logToFile = false;
				streamSelected = "";
			}
		}
	}


	//socket reading script	
	void SocketResponse() {		
		string serverSays = myTCP.readSocket();	
		if (serverSays != "") {		
			if (myTCP.socketReady == true && deviceConnected == true && logToFile == true){
				//streamwriter for writing to file
				using(StreamWriter sw = File.AppendText(savefilename)){
					sw.WriteLine(serverSays);
				}
			}else{
				Debug.Log("[SERVER]" + serverSays);
				string serverConnectOK = @"R device_connect OK";
				//Check if server response was device_connect OK
				if (string.CompareOrdinal(Regex.Replace(serverConnectOK,@"\s",""),Regex.Replace(serverSays.Substring(0,serverConnectOK.Length),@"\s","")) == 0){
					deviceConnected = true;
				}
			}
		} 
	}

	//send message to the server	
	public void SendToServer(string str) {		 
		myTCP.writeSocket(str);		
		Debug.Log ("[CLIENT] " + str);		
	}

	//Method To check Stopwatch properties
	void DisplayTimerProperties()
	{
		// Display the timer frequency and resolution.
		if (Stopwatch.IsHighResolution){
			Debug.Log("Operations timed using the system's high-resolution performance counter.");
		}else{
			Debug.Log("Operations timed using the DateTime class.");
		}
		
		long frequency = Stopwatch.Frequency;
		Debug.Log(string.Format("Timer frequency in ticks per second = {0}",frequency));
		long nanosecPerTick = (1000L*1000L*1000L) / frequency;
		Debug.Log(string.Format("Timer is accurate within {0} nanoseconds",nanosecPerTick));
	}
}

