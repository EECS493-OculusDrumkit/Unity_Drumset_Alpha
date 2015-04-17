using UnityEngine;
using System.Collections;

public class pipHandler : MonoBehaviour {

	private Animator	tom;
	private Animator	snare;
	private Animator	bass;
	private Animator	hiHat;
	private Animator	cowbell;
	private Animator	crash;


	// Use this for initialization
	void Start () {
		tom = transform.GetChild (0).GetComponent<Animator> ();
		snare = transform.GetChild (1).GetComponent<Animator> ();
		hiHat = transform.GetChild (2).GetComponent<Animator> ();
		crash = transform.GetChild (3).GetComponent<Animator> ();
		cowbell = transform.GetChild (4).GetComponent<Animator> ();
		bass = transform.GetChild (5).GetComponent<Animator> ();
	}

	public void disappear(){
		tom.SetBool ("shown", false);
		snare.SetBool ("shown", false);
		hiHat.SetBool ("shown", false);
		crash.SetBool ("shown", false);
		cowbell.SetBool ("shown", false);
		bass.SetBool ("shown", false);
	}

	public void showTom(){
		disappear ();
		tom.SetBool ("shown", true);
	}

	public void showSnare(){
		disappear ();
		snare.SetBool ("shown", true);
	}

	public void showBass(){
		disappear ();
		bass.SetBool ("shown", true);
	}

	public void showHiHat(){
		disappear ();
		hiHat.SetBool ("shown", true);
	}


	public void showCrash(){
		disappear ();
		crash.SetBool ("shown", true);
	}

	public void showCowbell(){
		disappear ();
		cowbell.SetBool ("shown", true);
	}
}
