using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour {

	public Camera cameraToFace;
	
	// Update is called once per frame
	void Update () {
		if(cameraToFace != null)
			transform.LookAt(transform.position + cameraToFace.transform.rotation * Vector3.forward, cameraToFace.transform.rotation * Vector3.up);
	}
}
