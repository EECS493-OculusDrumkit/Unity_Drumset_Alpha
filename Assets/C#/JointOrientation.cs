﻿using UnityEngine;
using System.Collections;
using BeatsByDre;
using System;

using LockingPolicy = Thalmic.Myo.LockingPolicy;
using Pose = Thalmic.Myo.Pose;
using UnlockType = Thalmic.Myo.UnlockType;
using VibrationType = Thalmic.Myo.VibrationType;
using InstrumentType = BeatsByDre.DrumBeat.InstrumentType;

// Orient the object to match that of the Myo armband.
// Compensate for initial yaw (orientation about the gravity vector) and roll (orientation about
// the wearer's arm) by allowing the user to set a reference orientation.
// Making the fingers spread pose or pressing the 'r' key resets the reference orientation.
public class JointOrientation : MonoBehaviour
{
    // Myo game object to connect with.
    // This object must have a ThalmicMyo script attached.
    public GameObject myo = null;

	public float LoopNumber;
	public float LoopHeight;
	public float LoopRadius;
	public Transform crossSphere;
	public Transform camera;
	//public GameObject crossHair;

	// 0 equal open hand
	// 1 equals fist and item is grabbed
	// 2 equals fist but nothing grabbed
	private int Grab;
	private GameObject hoveredBeat;
	private GameObject grabbedBeat;
	private LayerMask BeatLayermask;
	private LayerMask InstramentLayermask;

	private DrumBeat BeatScript;
	private DrumBeat grabbedScript;

	//drum animator handler
	private GameObject jellyHandler;
	private bool jellyExited;

	private InstrumentType _heldInstrument;

	//Handle for Crosshair Animator
	public Animator crossHair;
	public GameObject crossHairObject;

	void Start () {
		// Begin with drumstick
		Grab = 0;
		jellyExited = false;

	 	BeatLayermask = 1 << LayerMask.NameToLayer ("Beat"); // only check for collisions with beats
	 	InstramentLayermask = 1 << LayerMask.NameToLayer ("Instrament"); // only check for collisions with beats
	}
	
	private void InstramentGameobject (InstrumentType instrumentEnum, GameObject Beat) {
		pipHandler pip = Beat.transform.GetChild (0).GetComponent<pipHandler> ();
		switch (instrumentEnum) {
		case InstrumentType.SnareDrum:
			pip.showSnare();
			break;
		case InstrumentType.Cowbell:
			pip.showCowbell();
			break;
		case InstrumentType.CrashCymbal:
			pip.showCrash();
			break;
		case InstrumentType.TomDrum:
			pip.showTom();
			break;
		case InstrumentType.HiHatCymbal:
			pip.showHiHat();
			break;
		case InstrumentType.BassDrum:
			pip.showBass();
			break;
		default:
			throw new NotSupportedException("Invalid instrament");
		}
	}

    // A rotation that compensates for the Myo armband's orientation parallel to the ground, i.e. yaw.
    // Once set, the direction the Myo armband is facing becomes "forward" within the program.
    // Set by making the fingers spread pose or pressing "r".
    private Quaternion _antiYaw = Quaternion.identity;

    // A reference angle representing how the armband is rotated about the wearer's arm, i.e. roll.
    // Set by making the fingers spread pose or pressing "r".
    private float _referenceRoll = 0.0f;

    // The pose from the last update. This is used to determine if the pose has changed
    // so that actions are only performed upon making them rather than every frame during
    // which they are active.
    private Pose _lastPose = Pose.Unknown;

    // Update is called once per frame.
    void Update ()
    {
		// Access the ThalmicMyo component attached to the Myo object.
        ThalmicMyo thalmicMyo = myo.GetComponent<ThalmicMyo> ();

// attempt at location
		Vector3 myoVelocity = thalmicMyo.gyroscope;
		//DrumStick.position = new Vector3 (DrumStick.position.x, DrumStick.position.y, DrumStick.position.z - (0.1F * myoVelocity.z * Time.deltaTime)); 

        // Update references when the pose becomes fingers spread or the q key is pressed.
        bool updateReference = false;
        if (thalmicMyo.pose != _lastPose) {
            _lastPose = thalmicMyo.pose;

            if (thalmicMyo.pose == Pose.DoubleTap) {
                updateReference = true;
				ExtendUnlockAndNotifyUserAction(thalmicMyo);
            }
        }
        if (Input.GetKeyDown ("r")) {
            updateReference = true;
        }

// Grab event. For Max 
		// Object hasn't been grabbed
		if (Grab == 0) {

			if (thalmicMyo.pose == Pose.Fist) {

				//set grab trigger for crossHair
				crossHair.SetTrigger("Fist");

				RaycastHit hit;
				Vector3 fwd = crossHair.transform.TransformDirection (Vector3.forward);
				if (Physics.Raycast (transform.position, fwd, out hit, 15, InstramentLayermask.value)) {
					grabbedBeat = hit.collider.gameObject.GetComponent<Collider> ().gameObject;

					//set drum animator thng
					jellyHandler = grabbedBeat; 

					// Get instrument
					var beat = (InstrumentCan)grabbedBeat.GetComponent(typeof(InstrumentCan));
					_heldInstrument = beat.Instrument;
					Grab = 1;

					InstramentGameobject(_heldInstrument, crossHairObject);
				}
				else if (Physics.Raycast (transform.position, fwd, out hit, 15, BeatLayermask.value)){
					// Get the beat that is being raycast
					grabbedBeat = hit.collider.gameObject.GetComponent<Collider> ().gameObject;
					// get the script of that beat
					grabbedScript = (DrumBeat) grabbedBeat.GetComponent(typeof(DrumBeat));
					// get whether the beat is occupied
					if (grabbedScript.HasInstrument()){
						Grab = 1;
						// Get Instrument
						_heldInstrument = grabbedScript.Instrument;
						grabbedScript.Clear();

						//set drum animator thng
						jellyHandler = grabbedBeat;

						pipHandler pip = grabbedBeat.transform.GetChild (0).GetComponent<pipHandler> ();
						pip.disappear ();

						InstramentGameobject(_heldInstrument, crossHairObject);
					}else{
						Grab = 2;
					}
				} else {
					Grab = 2;
				}
			} else {
				RaycastHit hit;
				Vector3 fwd = crossHair.transform.TransformDirection (Vector3.forward);
				if (Physics.Raycast (transform.position, fwd, out hit, 15, InstramentLayermask.value)) {
					// go to hover
					crossHair.SetBool ("overObject", true);
					/*
					crossHair.GetComponent<Renderer> ().material.color = Color.yellow;
					if (crossHair.transform.localScale.x > 0.1f)
						crossHair.transform.localScale = new Vector3 (0.01f, 0.01f, 0.0f);
					else
						crossHair.transform.localScale = new Vector3 (crossHair.transform.localScale.x + 0.002f, crossHair.transform.localScale.y + 0.002f, 0.0f);
					*/
				} else if (Physics.Raycast (transform.position, fwd, out hit, 15, BeatLayermask.value)) {
					hoveredBeat = hit.collider.gameObject.GetComponent<Collider> ().gameObject;
					BeatScript = (DrumBeat) hoveredBeat.GetComponent(typeof(DrumBeat));
					if (BeatScript.HasInstrument()){
						// go to hover
						crossHair.SetBool ("overObject", true);
						/*
						crossHair.GetComponent<Renderer> ().material.color = Color.yellow;
						if (crossHair.transform.localScale.x > 0.1f)
							crossHair.transform.localScale = new Vector3 (0.01f, 0.01f, 0.0f);
						else
							crossHair.transform.localScale = new Vector3 (crossHair.transform.localScale.x + 0.002f, crossHair.transform.localScale.y + 0.002f, 0.0f);
						*/
					} else {
						//crossHair.GetComponent<Renderer> ().material.color = Color.black;
						//crossHair.transform.localScale = new Vector3 (0.1f, 0.1f, 0.0f);
						crossHair.SetBool ("overObject", false);
					}
				} else {
					//crossHair.GetComponent<Renderer> ().material.color = Color.black;
					//crossHair.transform.localScale = new Vector3 (0.1f, 0.1f, 0.0f);

					//set crossHair to not hovering over Object
					crossHair.SetBool ("overObject", false);

				}
			}
			//object has been grabbed
		} else if (Grab == 1) {

			if (thalmicMyo.pose == Pose.FingersSpread || Input.GetKeyDown("d")) {
				//set grab trigger for crossHair
				crossHair.SetTrigger("Release");

				//DrumStick.GetComponent<Renderer> ().enabled = true;
				//crossHair.GetComponent<Renderer> ().enabled = false;
				Grab = 0;
				//crossHair.transform.localScale = new Vector3 (0.1f, 0.1f, 0.0f);
				
				if (hoveredBeat != null) {
					BeatScript = (DrumBeat)hoveredBeat.GetComponent (typeof(DrumBeat));
					BeatScript.State = DrumBeat.BeatState.Empty;

				jellyExited = false;
				jellyHandler = null;
				}

				RaycastHit hit;
				Vector3 fwd = crossHair.transform.TransformDirection (Vector3.forward);
				if (Physics.Raycast (transform.position, fwd, out hit, 15, BeatLayermask.value)) {
					hoveredBeat = hit.collider.gameObject.GetComponent<Collider> ().gameObject;
					BeatScript = (DrumBeat) hoveredBeat.GetComponent(typeof(DrumBeat));
					BeatScript.State = DrumBeat.BeatState.Occupied;
					BeatScript.Instrument = _heldInstrument;

					InstramentGameobject(_heldInstrument, hoveredBeat);
					_heldInstrument = InstrumentType.None;
				}

				pipHandler pip = crossHairObject.transform.GetChild (0).GetComponent<pipHandler> ();
				pip.disappear ();
				hoveredBeat = null;
				grabbedBeat = null;
			} else {
				//crossHair.transform.localScale = new Vector3 (0.05f, 0.05f, 0.0f);

				RaycastHit hit;
				Vector3 fwd = crossHair.transform.TransformDirection (Vector3.forward);
				if (Physics.Raycast (transform.position, fwd, out hit, 15, InstramentLayermask.value)) {
				} else if (Physics.Raycast (transform.position, fwd, out hit, 15, BeatLayermask.value)) {
					hoveredBeat = hit.collider.gameObject.GetComponent<Collider> ().gameObject;
					BeatScript = (DrumBeat) hoveredBeat.GetComponent(typeof(DrumBeat));
					BeatScript.State = DrumBeat.BeatState.Hovered;
				} else {
					if (hoveredBeat != null) {
						BeatScript = (DrumBeat) hoveredBeat.GetComponent(typeof(DrumBeat));
						if (!BeatScript.HasInstrument()){
							BeatScript.State = DrumBeat.BeatState.Empty;
						}
						else{
							BeatScript.State = DrumBeat.BeatState.Occupied;
						}
						hoveredBeat = null;
					}
				}

				if (jellyExited == false){
					if(Physics.Raycast (transform.position, fwd, out hit, 15)){
						GameObject InstramentHover = hit.collider.gameObject.GetComponent<Collider> ().gameObject;
						if (jellyHandler != InstramentHover){
							jellyExited = true;
							jellyHandler.GetComponent<animatorHandler> ().triggerDrumAnim();
						}
						
					}
					else{
						jellyExited= true;
						jellyHandler.GetComponent<animatorHandler> ().triggerDrumAnim();
					}
				}

			}	
		} else {

			if (thalmicMyo.pose == Pose.FingersSpread || Input.GetKeyDown("d")) {
				//set grab trigger for crossHair
				crossHair.SetTrigger("Release");
				Grab = 0;
				jellyExited = false;
				jellyHandler = null;
			}
		}

//rotate drum stick
		crossSphere.rotation = transform.rotation;

        // Update references. This anchors the joint on-screen such that it faces forward away
        // from the viewer when the Myo armband is oriented the way it is when these references are taken.
        if (updateReference) {
            // _antiYaw represents a rotation of the Myo armband about the Y axis (up) which aligns the forward
            // vector of the rotation with Z = 1 when the wearer's arm is pointing in the reference direction.
            _antiYaw = Quaternion.FromToRotation (
                new Vector3 (myo.transform.forward.x, 0, myo.transform.forward.z),
				new Vector3 (Mathf.Sin(camera.eulerAngles.y *  Mathf.Deg2Rad), 0, Mathf.Cos(camera.eulerAngles.y *  Mathf.Deg2Rad))
				);
			
			// _referenceRoll represents how many degrees the Myo armband is rotated clockwise
            // about its forward axis (when looking down the wearer's arm towards their hand) from the reference zero
            // roll direction. This direction is calculated and explained below. When this reference is
            // taken, the joint will be rotated about its forward axis such that it faces upwards when
            // the roll value matches the reference.
            Vector3 referenceZeroRoll = computeZeroRollVector (myo.transform.forward);
            _referenceRoll = rollFromZero (referenceZeroRoll, myo.transform.forward, myo.transform.up);
        }

        // Current zero roll vector and roll value.
        Vector3 zeroRoll = computeZeroRollVector (myo.transform.forward);
        float roll = rollFromZero (zeroRoll, myo.transform.forward, myo.transform.up);

        // The relative roll is simply how much the current roll has changed relative to the reference roll.
        // adjustAngle simply keeps the resultant value within -180 to 180 degrees.
        float relativeRoll = normalizeAngle (roll - _referenceRoll);

        // antiRoll represents a rotation about the myo Armband's forward axis adjusting for reference roll.
        Quaternion antiRoll = Quaternion.AngleAxis (relativeRoll, myo.transform.forward);

        // Here the anti-roll and yaw rotations are applied to the myo Armband's forward direction to yield
        // the orientation of the joint.
        transform.rotation = _antiYaw * antiRoll * Quaternion.LookRotation (myo.transform.forward);

        // The above calculations were done assuming the Myo armbands's +x direction, in its own coordinate system,
        // was facing toward the wearer's elbow. If the Myo armband is worn with its +x direction facing the other way,
        // the rotation needs to be updated to compensate.
        if (thalmicMyo.xDirection == Thalmic.Myo.XDirection.TowardWrist) {
            // Mirror the rotation around the XZ plane in Unity's coordinate system (XY plane in Myo's coordinate
            // system). This makes the rotation reflect the arm's orientation, rather than that of the Myo armband.
           
			transform.rotation = new Quaternion(transform.localRotation.x,
                                                -transform.localRotation.y,
                                                transform.localRotation.z,
                                                -transform.localRotation.w);
        }
    }

    // Compute the angle of rotation clockwise about the forward axis relative to the provided zero roll direction.
    // As the armband is rotated about the forward axis this value will change, regardless of which way the
    // forward vector of the Myo is pointing. The returned value will be between -180 and 180 degrees.
    float rollFromZero (Vector3 zeroRoll, Vector3 forward, Vector3 up)
    {
        // The cosine of the angle between the up vector and the zero roll vector. Since both are
        // orthogonal to the forward vector, this tells us how far the Myo has been turned around the
        // forward axis relative to the zero roll vector, but we need to determine separately whether the
        // Myo has been rolled clockwise or counterclockwise.
        float cosine = Vector3.Dot (up, zeroRoll);

        // To determine the sign of the roll, we take the cross product of the up vector and the zero
        // roll vector. This cross product will either be the same or opposite direction as the forward
        // vector depending on whether up is clockwise or counter-clockwise from zero roll.
        // Thus the sign of the dot product of forward and it yields the sign of our roll value.
        Vector3 cp = Vector3.Cross (up, zeroRoll);
        float directionCosine = Vector3.Dot (forward, cp);
        float sign = directionCosine < 0.0f ? 1.0f : -1.0f;

        // Return the angle of roll (in degrees) from the cosine and the sign.
        return sign * Mathf.Rad2Deg * Mathf.Acos (cosine);
    }

    // Compute a vector that points perpendicular to the forward direction,
    // minimizing angular distance from world up (positive Y axis).
    // This represents the direction of no rotation about its forward axis.
    Vector3 computeZeroRollVector (Vector3 forward)
    {
        Vector3 antigravity = Vector3.up;
        Vector3 m = Vector3.Cross (myo.transform.forward, antigravity);
        Vector3 roll = Vector3.Cross (m, myo.transform.forward);

        return roll.normalized;
    }

    // Adjust the provided angle to be within a -180 to 180.
    float normalizeAngle (float angle)
    {
        if (angle > 180.0f) {
            return angle - 360.0f;
        }
        if (angle < -180.0f) {
            return angle + 360.0f;
        }
        return angle;
    }

    // Extend the unlock if ThalmcHub's locking policy is standard, and notifies the given myo that a user action was
    // recognized.
    void ExtendUnlockAndNotifyUserAction (ThalmicMyo myo)
    {
        ThalmicHub hub = ThalmicHub.instance;

        if (hub.lockingPolicy == LockingPolicy.Standard) {
            myo.Unlock (UnlockType.Timed);
        }

        myo.NotifyUserAction ();
    }
}
