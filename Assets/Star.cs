using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Star : MonoBehaviour {

	MainController mc;
	float speed;
	float start;

	void Start () {
		mc = GameObject.Find ("MainController").GetComponent<MainController> ();
		speed = mc.starSpeed;
		start = Time.fixedTime;
	}

	void Update () {
		//move forward then die
		gameObject.GetComponent<Rigidbody> ().AddForce (Vector3.Normalize (transform.forward) * speed, ForceMode.Force);
		if (Time.fixedTime - start > 3)
			Destroy (gameObject);
	}
}
