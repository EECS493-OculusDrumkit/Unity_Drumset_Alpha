using UnityEngine;
using System.Collections;
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


	void Start () {

		// Unused
//		Mesh mesh = GetComponent<MeshFilter> ().mesh;
//		Renderer rend = GetComponent<Renderer> ();
//		float loopDiam = rend.bounds.size.x;

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
		}

	}
	
	// Update is called once per frame
	void Update () {
		float beatsPerSecond = BPM / 60;
		float secondsPerRotation = divisions / beatsPerSecond;
		float degreesPerSecond = 360.0f / secondsPerRotation;
		float secondsPerDivision = 1 / beatsPerSecond;

		if (done) {
			DrumBeat beat = (DrumBeat)beatsByDre[curDivision].GetComponent(typeof(DrumBeat));
			beat.Play(secondsPerDivision);
			curDivision++;
			if (curDivision >= divisions){
				curDivision = 0;
			}
		}

		done = false;
		time += Time.deltaTime;

		if (time >= secondsPerDivision) {
			time = 0;
			done = true;
		}

		playHead.transform.Rotate (Vector3.up * degreesPerSecond * Time.deltaTime, Space.Self);
	}
}
