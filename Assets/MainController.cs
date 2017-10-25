using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class MainController : MonoBehaviour {

	//parameters we want to be able to change
	public float starSpeed = 2;
	public float crashTime = 0.5f;
	public float loseTime = 20;
	public bool isMenu = false;

	//private and temp variables
	private GameObject cam; //main camera

	//for shaking the screen
	private float rotAmount = 0.1f; 
	private float shakeAmount = 0.1f;
	private Vector3 rando;

	//state variables
	private bool crashed = false;
	private bool gameWon = false;
	private bool gameLost = false;
	private bool checkedFloppies = false;

	private Image panicImage = null;
	private GameObject losePanel = null;
	private GameObject winPanel = null;
	private int floppyCounter = 0; //keep track of how many floppies we inserted
	float t = 0; //keep track of our time since level load

	public void FloppyCounterPP(){ //setter
		floppyCounter++;
	}

	public Vector3 GetRandomVector(float f){ //helper function to generate random Vector3s
		return new Vector3 (Random.Range (-f, f), Random.Range (-f, f), Random.Range (-f, f));
	}

	// Use this for initialization
	void Start () {
		t = Time.timeSinceLevelLoad; //count from level load
		if (!isMenu) {

			//Get our gameobject instances and disable them
			panicImage = GameObject.Find ("Panel").GetComponent<Image> ();
			cam = GameObject.Find ("Main Camera");
			losePanel = GameObject.Find ("LosePanel");
			winPanel = GameObject.Find ("WinPanel");
			panicImage.color = new Color (0, 0, 0, 0);
			losePanel.SetActive (false);
			winPanel.SetActive(false);
			rando = GetRandomVector(shakeAmount); //for shaking screen
		}
		spinFloppies (); //nudge our menu floppies
	}

	void FixedUpdate () {
		t = Time.timeSinceLevelLoad;
		if (isMenu) return; //we don't need to update anything on the menu


		if (cam.transform.rotation.eulerAngles.x < 350) {
			cam.transform.Rotate (new Vector3 (-rotAmount, 0, 0));
			rotAmount += 0.025f; //move the camera up from looking down
		} else {
			cam.transform.Rotate((-rando));
			rando = GetRandomVector(shakeAmount);
			cam.transform.Rotate(rando);
			//constantly shake the camera
		}

		//States
		if (t > crashTime - 7 && t < crashTime - 6 && !checkedFloppies) { //do the check floppy animation 7 seconds before the crash
			StartCoroutine (checkFloppies ());
			checkedFloppies = true;

		} else if (t > crashTime - 1 && t < crashTime) { //1 second before the crash, start shaking screen and turn on the alarm blink
			shakeAmount = 1f;
			GameObject[] texts = GameObject.FindGameObjectsWithTag ("TVText"); //Change TV text
			texts [0].GetComponent<TextMesh> ().text = "WARNING";
			texts [1].GetComponent<TextMesh> ().text = "  FLOPPY";
			texts [2].GetComponent<TextMesh> ().text = "  ERROR";
			StartCoroutine (panicBlink ());

		} else if (t > crashTime && cam.transform.rotation.y < 0) {
			cam.transform.Rotate (new Vector3 (0, 0.5f, 0)); //next state, rotate the camera to look at the floppies

		} else if (t > crashTime && floppyCounter < 9 && t < loseTime) {
			GameObject[] texts = GameObject.FindGameObjectsWithTag ("TVText");
			texts [0].GetComponent<TextMesh> ().text = "   "+ System.Math.Round(loseTime - t, 2); //change the TVtext to a countdown
			shakeAmount = 0.4f; //less violent shake

		} else if (floppyCounter >= 9 && !gameWon) {
			gameWon = true; //win the game
			shakeAmount = 0.1f;
			winPanel.SetActive (true);

		} else if (t >= loseTime && !gameLost &&!gameWon) {
			losePanel.SetActive (true);
			gameLost = true; //lose the game
		} 	

		if ((gameWon||gameLost)&&Input.GetKeyDown(KeyCode.Mouse0)){
			SceneManager.LoadScene ("main");
			//mouse hits after game is over return to menu
		}

	}

	void spinFloppies(){ //Nudges each floppy disk a bit with some rotation too
		GameObject[] floppies = GameObject.FindGameObjectsWithTag ("Floppy");
		for (int i = 0; i<floppies.Length;i++){
			Rigidbody r = floppies [i].gameObject.GetComponent<Rigidbody> ();
			r.AddForce (GetRandomVector (5));
			r.AddTorque (GetRandomVector(20));
		}
	}

	private IEnumerator checkFloppies(){ //coroutine for checking floppy disks
		int i = 0;
		int max = 100;
		while (i < max) {
			cam.transform.Rotate (0, 0.5f, 0);
			i++;
			yield return new WaitForFixedUpdate ();
		}
		while (i > 0) {
			cam.transform.Rotate (0, -0.5f, 0);
			i--;
			yield return new WaitForFixedUpdate ();
		}
		Destroy(GameObject.Find ("FloppyNeat"));
		Instantiate (Resources.Load<GameObject> ("FloppyDisaster")); //replace neat floppies with mess
		spinFloppies ();

	}		

	private IEnumerator panicBlink(){ //lerp a red alarm until the game has been won.
		Color start = new Color (255, 0, 0, 0);
		Color end = new Color (255, 0, 0, 0.2f);
		float i = 0;
		while (i < 1) {
			panicImage.color = Color.Lerp (start, end, i);
			i += 0.02f;
			yield return new WaitForFixedUpdate ();
		}
		while (i > 0) {
			panicImage.color = Color.Lerp (start, end, i);
			i -= 0.02f;
			yield return new WaitForFixedUpdate ();
		}
		if (!gameWon) {
			StartCoroutine (panicBlink ());
		}
	}
}
