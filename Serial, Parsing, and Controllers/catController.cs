/* Nick Hwang - Cat Game - 2018
    Script should get data from the PlayerController script to update the Cat Object's orientation in the scene, smoothing should be used.
    This script can override incoming controller values, to use keyboard controls instead of the cat controller. See 'spoofController'


        check quaternion values from euler
        print ("upright: "+ Quaternion.Euler (0, 0, 0));
        print ("upside down: "+ Quaternion.Euler (0, 180, 180));
        print ("faceside up: "+ Quaternion.Euler (0, 0, 90));
        print ("tailside up: "+ Quaternion.Euler (0, 0, -90));
        print ("leftside up: "+ Quaternion.Euler (-90, 0, 0));
        print ("rightside up: "+ Quaternion.Euler (90, 0, 0));
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(PlayerController))]
[RequireComponent(typeof(RotationTransition))]
public class catController : MonoBehaviour
{

    #region object vars
    [Header("Cat Object States")]
    public bool useQuaternions;
    private Quaternion q_Calibrate = new Quaternion(0, 0, 0, 1);

    public Vector3 startingRotationOffset;
    public Vector3 orientationValues;
    public bool[] touchareas = new bool[6];                 // area of bool touch areas -- will write a custom editor for this: Neck, Back, Tail, Chin, Chest, Belly
    #endregion

    #region Spoof Controls
    [Header("Keyboard Spoof Controls")]
    public bool spoofController;                            // toggle on to 1. ignore incoming controller data 2. use keyboard commands to override and simulate incoming controller data. 

    //	spoof keys to simulate touchareas: Nick normally sets theses to Keys 1,2,3,4,5,6
    public TurnKey[] turnKeys = new TurnKey[6];
    public TouchKey[] touchKeys = new TouchKey[6];			//	spoof keys to simulate touchareas: Nick normally sets theses to Keys Q,W,E,R,T,Y
    public KeyCode scratchModifier;
    #endregion

    PlayerController PC;                                    // PlayerController instance reference
    private checkRotationState rotationState;
    private RotationTransition rotationTransition;

    public UnityAction<Objective> touchAction;

    void Start()
    {
        // Grab instance of PlayerController static instance
        PC = PlayerController.instance;
        rotationState = GetComponent<checkRotationState>();
        rotationTransition = GetComponent<RotationTransition>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!spoofController)
        {
			getKeyboardInput();
            // if using quaternions
            if (useQuaternions)
            {
                q_Calibrate = Quaternion.Euler(startingRotationOffset);
                transform.localRotation = PC.quatOrientation * Quaternion.Inverse(q_Calibrate);

                //transform.localRotation = PC.quatOrientation;
                orientationValues = transform.eulerAngles;
            }
            else
            {
                orientationValues = PC.orientationValues;                           // sets cat object's local orientation var
                                                                                    //  			touchkeys = PC.touchareas;											// sets cat touch area bools
                transform.localRotation = Quaternion.Euler(orientationValues);		// sets cat object's local transform rotation -- pop this out of update
            }

            // decode the gesture values from PlayerController.TouchValues[float]

            detectGestures();

        }

#region spoofing
        else
        {
            // print ("using spoof");
            getKeyboardInput();                                                 // control cat object with keyboard
            orientationValues = transform.rotation.eulerAngles;
        }
    }

    // this function gets called when 'spoof controllers' is true. Allows users to use keyboard to simulate the cat controller: orientation and touchareas: specific keys set in inspector
    private void getKeyboardInput()
    {
        // orientation key presses
        for (int i = 0; i < turnKeys.Length; i++) {
            if (Input.GetKeyDown(turnKeys[i].keyCode)) {
                print($"pressed {turnKeys[i].catRotation.ToDescription()} key");
                StopAllCoroutines();
                rotationTransition.SetGoalOrientation(turnKeys[i].catRotation.ToQuaternion());
                StartCoroutine(rotationTransition.Transition());
            }
        }

        // touchkey keydowns and keyups
        CatGesture catGesture = Input.GetKey(scratchModifier) ? CatGesture.Scratch : CatGesture.Pet;
        for (int i = 0; i < touchKeys.Length; i++) {
            if (Input.GetKeyDown(touchKeys[i].keyCode)) {
                print($"pressed {touchKeys[i].catLocation.ToDescription()} key");
                touchKeys[i].isTouched = true;
                Objective spoofInput = new Objective(rotationState.currentRotation, touchKeys[i].catLocation, catGesture);
                if (touchAction == null) {
                    print("touch action is null");
                }
                else {
                    print("touch action is not null");
                }
                touchAction?.Invoke(spoofInput);
            }

            if (Input.GetKeyUp(touchKeys[i].keyCode)) {
                print($"released {touchKeys[i].catLocation.ToDescription()} key");
                touchKeys[i].isTouched = false;
            }
        }
    }

#endregion


    void detectGestures(){
        for (int i = 0; PC.TouchValues.Length > i; i++){


        }
    }
}
