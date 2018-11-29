
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PresentSpawnerExtended : MonoBehaviour {

	[Tooltip("Maximum distance from gameObject a present will spawn")]
	public float distance = 2.0f;
	public float maxInterval = 1.0f;
	public float minInterval = 0.1f;

	void Start () {
		StartCoroutine(Spawn());
	}

	IEnumerator Spawn() {

		yield return new WaitForSeconds(Random.Range(minInterval,maxInterval));

		var pos = gameObject.transform.position;
		pos.x += Random.Range(distance * -1, distance);
		pos.z += Random.Range(distance * -1, distance);

		var index = Mathf.FloorToInt(Random.Range(0, 66));

		var present = "Present_Box_";

		if(index <= 41)
			present = string.Format("{0}Color_{1:D2}",present,index);
		else
			present = string.Format("{0}Pattern_{1:D2}",present,index - 41);
		
		Debug.Log(present);

		Instantiate(Resources.Load(present),pos,Quaternion.identity);

		StartCoroutine(Spawn());
	}
	
}
