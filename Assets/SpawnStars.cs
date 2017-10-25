using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnStars : MonoBehaviour {

	public float starsPerSecond = 5;
	public float yDiff = 5;
	private float lastSpawn;

	void Start () {
		lastSpawn = Time.fixedTime;
	}

	void Update () {
		if (Time.fixedTime-lastSpawn>=(1.0/starsPerSecond)){
			//Simple script to spawn stars with random Y
			GameObject g = Instantiate(Resources.Load<GameObject>("Star"));
			Vector3 v = transform.position;
			v.y += Random.Range (-yDiff, yDiff);
			g.transform.position = v;
			g.transform.localRotation = transform.localRotation;
			lastSpawn = Time.fixedTime;
		}
	}
}
