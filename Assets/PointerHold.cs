using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointerHold : MonoBehaviour {

	//parameter variables
	public float gripStength = 150;
	public float gripThres = 0.2f;

	//private flags and temp variables
	private Vector2 mouseInitial;
	private bool holdingDown = true;
	private GameObject objectHeld = null;
	private float maxRay = Mathf.Infinity;
	private MainController mc;

	// Use this for initialization
	void Start () {
		mc = GameObject.Find ("MainController").GetComponent<MainController> ();
	}


	//Bind the held object to a ray
	private void BindToRay(Ray ray){
		Vector3 mouseCast = (ray.origin + maxRay * ray.direction);
		Vector3 objectLoc = objectHeld.transform.position;
		Vector3 forceTo = mouseCast - objectLoc;
		if ((mouseCast - objectLoc).magnitude <= gripThres) {
			return; //we don't need to apply any force it's already close
		} else if ((mouseCast - objectLoc).magnitude <= gripThres * 2) { 
			//grip with less strength if it's really close
			//this prevents swinging back and forth
			objectHeld.GetComponent<Rigidbody> ().AddForce (Vector3.Normalize (forceTo)*gripStength/10, ForceMode.Acceleration);
		} else {
			objectHeld.GetComponent<Rigidbody> ().AddForce (Vector3.Normalize (forceTo)*gripStength, ForceMode.Acceleration);
		}

	}

	// Update is called once per frame
	void Update () {
		if (Input.GetKey (KeyCode.Mouse0)) { //If left mouse button is pressed
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition); //Raycast to mouse pointer
			RaycastHit hit; // Our hit
			if (Physics.Raycast (ray, out hit, maxRay)) { //If our raycast hit a rigidbody
				if (hit.collider.tag!="Floppy") return; //Only grab floppy disks
				if (!holdingDown) {
					hit.collider.gameObject.GetComponent<Rigidbody> ().AddTorque (mc.GetRandomVector(15)); //Spin the object a bit on grab
					objectHeld = hit.collider.gameObject; //Store our held object
					maxRay = hit.distance; //Set our max ray distance so we know how far our grab was
					holdingDown = true; //Only run this code when we initially grab the object
				}
				BindToRay (ray); //bind to the ray
			} else if (holdingDown){
				BindToRay (ray); //if we initially grabbed onto something but the pointer isn't overlapping it, we still want to grab it
			}

		} else {
			holdingDown = false; //once we release the object, reset everything back
			maxRay = Mathf.Infinity;
		}

	}
}
