using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System;

public class socketScripts : MonoBehaviour {	
	//variables	
	private TCPConnection myTCP;	
	private string serverMsg;	
	public string msgToServer;

	void Awake() {		
		//add a copy of TCPConnection to this game object		
		myTCP = gameObject.AddComponent<TCPConnection>();		
	}
	
	void Start () {		
		
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
				//try to connect				
				Debug.Log("Attempting to connect..");				
				myTCP.setupSocket();				
			}
		}

		//once connection has been made, display editable text field with a
		//button to send that string to the server (see function below)		
		if (myTCP.socketReady == true) {			
			msgToServer = GUILayout.TextField(msgToServer);
			if (GUILayout.Button ("Write to server", GUILayout.Height(30))) {				
				SendToServer(msgToServer);
			}
		}
	}
		
	//socket reading script	
	void SocketResponse() {		
		string serverSays = myTCP.readSocket();		
		if (serverSays != "") {			
			Debug.Log("[SERVER]" + serverSays);			
		}
	}

	//send message to the server	
	public void SendToServer(string str) {		
		myTCP.writeSocket(str);		
		Debug.Log ("[CLIENT] -> " + str);		
	}
}

