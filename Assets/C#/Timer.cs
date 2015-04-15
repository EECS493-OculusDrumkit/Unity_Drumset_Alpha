using UnityEngine;
using System.Collections;
using System.Threading;
using BeatsByDre;

public class Timer : MonoBehaviour {
	
	public int BPM = 120;
	public int divisions = 16;
	public int LoopRadius = 10;
	public int loopHover = 4;
	public GameObject DrumBeat;
	public GameObject PlayHead;

	private float time = 0.0f;
	private bool done = true;
	private int curDivision = 0;
	private float signature = 4.0f;
	private GameObject playHead;
	private GameObject [] beatsByDre;

	// State variables
	float beatsPerSecond;
	float secondsPerRotation;
	float degreesPerSecond;
	float secondsPerDivision;

	void Start () {

		// Unused
//		Mesh mesh = GetComponent<MeshFilter> ().mesh;
//		Renderer rend = GetComponent<Renderer> ();
//		float loopDiam = rend.bounds.size.x;

		beatsPerSecond = BPM / 60;
		secondsPerRotation = divisions / beatsPerSecond;
		degreesPerSecond = 360.0f / secondsPerRotation;
		secondsPerDivision = 1 / beatsPerSecond;

		Vector3 timerPosition =  new Vector3 (0, 0, 0);
		Quaternion timerRotation = Quaternion.Euler(new Vector3(0, 0, 0));

		playHead = Instantiate (PlayHead, timerPosition, timerRotation) as GameObject;
		playHead.transform.parent = transform;
		playHead.transform.localScale = new Vector3 (1, 1, 1);

		beatsByDre = new GameObject[divisions];

		for (int i = 0; i < divisions; i++) 
		{
			float AngleDegrees = i * (360.0f/divisions);
			float AngleRadians = Mathf.Deg2Rad * AngleDegrees;
			Vector3 BeatPosition = new Vector3(Mathf.Sin(AngleRadians) * LoopRadius, loopHover, Mathf.Cos(AngleRadians) * LoopRadius);
			Quaternion BeatRotation =  Quaternion.Euler(new Vector3(0, AngleDegrees, 0));
			
			beatsByDre[i] = Instantiate(DrumBeat, BeatPosition, BeatRotation) as GameObject;
			((DrumBeat)beatsByDre [i].GetComponent (typeof(DrumBeat))).DurationMs = (int)(secondsPerDivision * 1000);
		}

	}
	
	// Update is called once per frame
	void Update () {
		

//		if (done) {
//			DrumBeat beat = (DrumBeat)beatsByDre[curDivision].GetComponent(typeof(DrumBeat));
//			Thread playThread = new Thread (() => beat.Play (secondsPerDivision));
//			playThread.Start ();
//			beat.Play(secondsPerDivision);
//			curDivision++;
//			if (curDivision >= divisions){
//				curDivision = 0;
//			}
//		}
//
//		done = false;
//		time += Time.deltaTime;
//
//		if (time >= secondsPerDivision) {
//			time = 0;
//			done = true;
//		}

		playHead.transform.Rotate (Vector3.up * degreesPerSecond * Time.deltaTime, Space.Self);
	}
}
