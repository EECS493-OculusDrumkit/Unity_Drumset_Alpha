using UnityEngine;
using System.Collections;

public class Shoot : MonoBehaviour {
	GameObject camera;

	// Use this for initialization
	void Start () {
		camera = GameObject.Find("CenterEyeAnchor");
			

	}
	
	public GameObject rocket;
	public float speed = 10f;
	
	void FireRocket () {
		GameObject rocketClone = (GameObject) Instantiate(rocket, new Vector3(0,1,0), transform.rotation);
		rocketClone.GetComponent<Rigidbody>().velocity = transform.up * speed;
		
		// You can also acccess other components / scripts of the clone
	}
	
	// Calls the fire method when holding down ctrl or mouse
	void Update () {
		if (Input.GetKeyDown(KeyCode.L))
			FireRocket();
		}
}

