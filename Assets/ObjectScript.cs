using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectScript : MonoBehaviour {

	public float startingForce = 1;
	public float startingRotation = 1;
	public float notMoving = 0.1f;
	public float notRotating = 0.1f;
	public float velDown = 0.01f;
	float x, y, z;
	Rigidbody r;
	private int forceDuration = 20;
	private int frames = 0;


	// Use this for initialization
	void Start () {
		r = gameObject.GetComponent<Rigidbody> ();
		x = Random.Range (-startingForce, startingForce);
		y = Random.Range (-startingForce, startingForce);
		z = Random.Range (-startingForce, startingForce);

		//print ("adding force");
		/*
		r.AddForce(new Vector3 (x, y, z));

		x = Random.Range (-startingRotation, startingRotation);
		y = Random.Range (-startingRotation, startingRotation);
		z = Random.Range (-startingRotation, startingRotation);*/

	}
	
	// Update is called once per frame
	void Update () {

	}
}
