using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.IO.Ports;

public class SerialTest : MonoBehaviour {

	// Use this for initialization
	void Start () {
		string[] ports = System.IO.Ports.SerialPort.GetPortNames ();
		if(ports.Length > 0 )
		print (ports);
		foreach(string port in ports){
			print ("port: " + port); 
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
