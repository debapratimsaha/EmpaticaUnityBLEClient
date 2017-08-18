# EmpaticaUnityBLEClient
Empatica BLE Client for Unity 3D Engine, developed at ICAT, Virginia Tech

This software provide a Unity 3D Engine based GUI to communicate with the Empatica Windows BLE Server, whose working is 
described in [http://developer.empatica.com/windows-ble-server.html]. 

This code was developed as a part of a research project that resulted in the following publication, please cite this work if u find it useful:
```
@inproceedings{saha2017affective,  
     title={Affective Feedback in a Virtual Reality based Intelligent Supermarket},  
     author={Saha, Deba Pratim and Martin, Thomas L. and Knapp, R. Benjamin},  
     booktitle={Adjunct Proceedings of UbiComp 2017, ACM Joint Conference on Pervasive and Ubiquitous Computing, Maui, Hawai'i, USA},  
     year={2017},  
     organization={ACM}  
}
```

## How to use:
Pre-requisite: 
It assumes Empatica BLE Server for Windows is already installed, running and connected to the Empatica E4 device(s) intended to communicate with.

Please use the following steps to work with this software:
 * 0. Attach "ICATEmpaticaBLEClient.cs" to main camera or any empty game object which loads at the beginning.
 * 1. On launch, it tries to connect to the localhost/port20 
  	  (You have to change it to your own ip/port combination as per your PC's configuration for BLE driver).
	  [Refer to Requirements and Configuration section of http://developer.empatica.com/windows-ble-server.html for more information]
 * 2. Once the script successfully runs inside Unity, enter the Device ID and connect to device.
 * 3. Select the data streams to log and hit "Log Data"
 * 4. Hit Ctrl+Shift+Z to disconnect at anytime.
