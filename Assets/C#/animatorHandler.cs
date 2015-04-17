using UnityEngine;
using System.Collections;

public class animatorHandler : MonoBehaviour {

	private Animator		drumAnimJelly;

	void Start(){
		drumAnimJelly = GetComponent<Animator> ();
	}

	public void triggerDrumAnim(){
		drumAnimJelly.SetTrigger ("I_pulledOut");
	}
}
