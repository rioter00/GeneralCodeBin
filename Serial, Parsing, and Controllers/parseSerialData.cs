
/* Nick Hwang - Cat Game - 2018
 This script that has three main functions which gets called via Unity's 'message' system, called from the Serial.cs script when the Controller is properly connected.
 This script also re-assigns quaternion data from the Arduino 101's IMU.
 The IMU's axis orientation is 'East-North-Up' for its X-Y-Z
 Unity's world axes is 'Left Handed'

 So the Quaternion values have to be re-mapped & the w of the IMU (which is currently first value)
 needed to be remapped for Unity's fourth quaternion value.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Serial))]

public class parseSerialData : MonoBehaviour {

	public string[] parseTerms;

	PlayerController PC;

	void Start(){
		// Grab instance of PlayerController instance
		PC = PlayerController.instance;
		parseTerms = PC.Orientations;
	}
	
	void OnSerialData(string data){
//		print ("Data: " + data);
	}

	void OnSerialValues(string[] data){
		foreach (string line in data) {
//			print ("Values: " + line); 
		}
	}

    void OnSerialLine(string data){
		//print ("Line Data: " + data);
		for (int i = 0; i < parseTerms.Length; i++) {
            if (data.Contains(parseTerms[i]))
            {
                string[] tempString = data.Split(':');

                // finds the property that matches the 'orientation'
                var orientation = PC.GetType().GetProperty(parseTerms[i]);

                if (orientation != null)
                {
                    // writes oritentation values in PlayerController
                    //					print ("writing to orientation " + orientation.ToString ());
                    orientation.SetValue(PC, float.Parse(tempString[1]), null);
                }
                else
                {
                    print("orientation not writable");
                }
            }
		}

        // parse the quat values
        parseQuat(data);

        // parse pressure sensor values
        parsePressure(data);

	}

    void parseQuat(string data) {
        if (data.Contains("quat"))
        {
            char[] delimiters = { ':', ',' };
            string[] tempString = data.Split(delimiters, 5);
            PC.quatOrientation.Set(float.Parse(tempString[2]) * 1f, float.Parse(tempString[4]) * -1f, float.Parse(tempString[3]) * 1f, float.Parse(tempString[1]));
            //PC.quatOrientation.Set(1, 0, 0, 0);
        }
    }

    void parsePressure(string data) {
        if (data.Contains("pressure"))
        {
            char[] delimiters = { ':', ',' };
            string[] tempString = data.Split(delimiters, 5);
            for (int i = 0; i < tempString.Length; i++)
            {
                PC.TouchValues.SetValue(tempString[i], i);
            }
        }
    }
}
