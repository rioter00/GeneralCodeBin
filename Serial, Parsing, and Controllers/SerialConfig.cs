/*
 * SerialConfig component
 * 
 * This holds some serial information used to communicate with controller through serial interface
 * See configuration section in Serial script for information on how to use this script.
 * 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SerialConfig : MonoBehaviour {

	public string[] portNames = {"/dev/tty.usb", "/dev/ttyUSB", "/dev/cu.usb", "/dev/cuUSB", ""};
//	public string[] portNames = {"/dev/cu.usb"};

//	public string[] portNames = {"/dev/tty.usb", "/dev/ttyUSB"};

	public int speed = 9600;

	/// <summary>
	/// Log some debug informations to the console to help find what's wrong when needed.
	/// </summary>
	public bool logDebugInfos = false;
}