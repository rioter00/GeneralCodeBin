/* Nick Hwang - Cat Game - 2018
    This script handles data FROM the physical controller and stores the quaternion and touch values.
    Simply stores values from parseSerialsData script.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(parseSerialData))]

public class PlayerController : MonoBehaviour {

	static public PlayerController instance = null;         // static instance of PlayerController which allows it to be accessed by any other script.

	[HideInInspector]
	public string[] Orientations;
    // [HideInInspector] // hide me later
    public float[] TouchValues = new float [6];
    //public 


	[Header("Controller Values")]
	public bool controllerConnected;
	public Vector3 orientationValues;
	public bool [] touchareas = new bool [6];				// area of bool touch areas -- will write a custom editor for this: Neck, Back, Tail, Chin, Chest, Belly

    public Quaternion quatOrientation;                      // data updated from parseSerialData script from the controller. Uses the Unity Method Quaternion.Set

	[SerializeField]
	public bool invertRoll, invertPitch, invertYaw;         // inverts incoming controller values
	[SerializeField]
	public bool ignoreRoll, ignorePitch, ignoreYaw;
	[SerializeField]
	public float rollOffset, pitchOffset, yawOffset;

	//Awake is always called before any Start functions
	void Awake()
	{
		Orientations = new string[] { "Roll", "Yaw", "Pitch"};

		//Check if instance already exists
		if (instance == null)

			//if not, set instance to this
			instance = this;

		//If instance already exists and it's not this:
		else if (instance != this) {

			//Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
			Destroy (gameObject);
		}

		//Sets this to not be destroyed when reloading scene
		DontDestroyOnLoad (gameObject);
	}

	public float Roll {
		get{
			return orientationValues.x;
		}
		set{
			if(ignoreRoll){
				orientationValues.x = 0;
			}
			else if (invertRoll) {
				orientationValues.x = (value * -1) + rollOffset;
			} else {
				orientationValues.x = value + rollOffset;
			}
		}
	}

	public float Pitch {
		get{
			return orientationValues.y;
		}
		set{
			if(ignorePitch){
				orientationValues.z = 0;
			}
			else if (invertPitch) {
				orientationValues.z = (value * -1) + pitchOffset;
			} else {
				orientationValues.z = value + pitchOffset;
			}
		}
	}

	public float Yaw {
		get{
			return orientationValues.z;
		}
		set{
			if(ignoreYaw){
				orientationValues.y = 0;
			}
			else if (invertYaw) {
				orientationValues.y = (value * -1) + yawOffset;
			} else {
				orientationValues.y = value + yawOffset;
			}
		}
	}
}
