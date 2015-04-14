using UnityEngine;
using System.Collections;

public class Timer : MonoBehaviour {
	
	public int BPM = 120;
	public int divisions = 16;
	public int LoopRadius = 10;
	public int loopHover = 4;
	public GameObject DrumBeat;
	public GameObject PlayHead;

	private float time = 0.0f;
	private int curDivision = 0;
	private int signature = 4;
	private GameObject playHead;
	private GameObject [] beatsByDre;


	void Start () {

		Mesh mesh = GetComponent<MeshFilter> ().mesh;
		Renderer rend = GetComponent<Renderer> ();
		float loopDiam = rend.bounds.size.x;

		Vector3 timerPosition =  new Vector3 (0, 0, 0);
		Quaternion timerRotation = Quaternion.Euler(new Vector3(0, 0, 0));

		playHead = Instantiate (PlayHead, timerPosition, timerRotation) as GameObject;
		playHead.transform.parent = transform;
		playHead.transform.localScale = new Vector3 (1, 1, 1);

		

//		playHead = Instantiate (PlayHead, timerPosition, timerRotation) as GameObject;
//		playHead.transform.parent = transform;

		beatsByDre = new GameObject[16];

		for (int i = 0; i < divisions; i++) 
		{
			float AngleDegrees = i * (360/divisions);
			float AngleRadians = Mathf.Deg2Rad * AngleDegrees;
			Vector3 BeatPosition = new Vector3(Mathf.Sin(AngleRadians) * LoopRadius, loopHover, Mathf.Cos(AngleRadians) * LoopRadius);
			Quaternion BeatRotation =  Quaternion.Euler(new Vector3(0, AngleDegrees, 0));
			
			beatsByDre[i] = Instantiate(DrumBeat, BeatPosition, BeatRotation) as GameObject;
			//beatsByDre[i].transform.parent = transform;
		}

	}
	
	// Update is called once per frame
	void Update () {
		float beatsPerSecond = BPM / 60;
		float secondsPerRotation = beatsPerSecond / 4;
		float degreesPerSecond = secondsPerRotation * 360;

		float secondsPerDevision = secondsPerRotation / 16;

		if (time == 0) {
			//beatsByDre[curSection].play
			curDivision++;
			if (curDivision >= divisions){
				curDivision = 0;
			}
		}

		time += Time.deltaTime;

		if (time >= secondsPerDevision) {
			time = 0;
		}

		playHead.transform.Rotate (Vector3.up * degreesPerSecond * Time.deltaTime, Space.Self);
	}
}
