using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AcceptFloppy : MonoBehaviour {

	MainController mc;
	GameObject AlmostIdealFloppy = null;
	GameObject IdealFloppy = null;

	void Start () {
		//Get our prefabs for floppy insert animation
		AlmostIdealFloppy = GameObject.Find ("AlmostIdealFloppy");
		IdealFloppy = GameObject.Find ("IdealFloppy");

		mc = GameObject.Find ("MainController").GetComponent<MainController> ();
	}

	void OnTriggerEnter(Collider c){
		//Detect floppy disk in front of slot
		if (c.gameObject.tag == "Floppy") {
			c.gameObject.GetComponent<Rigidbody> ().isKinematic = true;
			//Start animation
			StartCoroutine (LerpTo(c.gameObject, AlmostIdealFloppy, IdealFloppy));
		}
	}

	//Lerps twice to both states of our floppy disks
	private IEnumerator LerpTo(GameObject FloppyToAccept, GameObject FloppyOne, GameObject FloppyTwo){
		float progress = 0.01f;
		Transform start = FloppyToAccept.transform; 
		Transform end = FloppyOne.transform;

		//Simple Lerp
		while (progress<0.4){
			FloppyToAccept.transform.position = Vector3.Lerp (start.position, end.position, progress);
			FloppyToAccept.transform.localRotation = Quaternion.Lerp (start.localRotation, end.localRotation, progress);
			progress += 0.01f;
			yield return new WaitForFixedUpdate();
		}

		if (FloppyTwo != null) {

			//Start the second state animation lerp
			StartCoroutine (LerpTo(FloppyToAccept, IdealFloppy, null));
			yield return new WaitForSeconds (0.3f);
			mc.FloppyCounterPP ();

			//If we're on the main menu just load the game
			if (SceneManager.GetActiveScene().name=="main") {
				SceneManager.LoadScene ("game");
			}
		}
	}

}
