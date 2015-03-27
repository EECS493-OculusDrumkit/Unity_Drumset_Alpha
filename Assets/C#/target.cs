using UnityEngine;
using System.Collections;

public class target : MonoBehaviour {

	public GameObject newTarget; 
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionEnter (Collision col)
		{
		if(col.gameObject.name == "target" || col.gameObject.name == "target(Clone)" )
			{
			print ("Sean");
			Destroy(col.gameObject);
			Destroy(this);
			Vector3 position = new Vector3(Random.Range(-5.0F, 5.0F), 3, Random.Range(-5.0F, 5.0F));
			GameObject targetClone = (GameObject) Instantiate(newTarget, position, transform.rotation);

			}
		}
}
