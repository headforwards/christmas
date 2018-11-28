using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PresentSpawnerExtended : MonoBehaviour {

	public float distance = 2.0f;
	public float maxInterval = 1.0f;
	public float minInterval = 0.1f;

	// Use this for initialization
	void Start () {
		StartCoroutine(presentSpawn());

	}

	IEnumerator presentSpawn() {
		yield return new WaitForSeconds(Random.Range(minInterval,maxInterval));
		var pos = gameObject.transform.position;
		pos.x += Random.Range(distance * -1, distance);
		pos.z += Random.Range(distance * -1, distance);
		Instantiate(Resources.Load("present"),pos,Quaternion.identity);

		StartCoroutine(presentSpawn());

	}
	
}
