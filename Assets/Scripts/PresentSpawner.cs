
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PresentSpawner : MonoBehaviour {

	[Tooltip("Maximum distance from gameObject a present will spawn")]
	public float distance = 2.0f;
	
	[Tooltip("Max delay before a present is spawned")]
	public float maxInterval = 1.0f;

	[Tooltip("Min delay before a present is spawned")]
	public float minInterval = 0.1f;

	[Tooltip("Lifetime in seconds before present is disposed automatically")]
	public float lifeTime = 4.0f;

	[Tooltip("Velocity random value")]
	public float velocity = 2.0f;

		[Tooltip("Torque random value")]
	public float tourqe = 2.0f;

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
			present = string.Format("presents/{0}Color_{1:D2}",present,index);
		else
			present = string.Format("presents/{0}Pattern_{1:D2}",present,index - 42);

		var instance = Instantiate(Resources.Load(present),pos,Quaternion.identity) as GameObject;

		var rigidBody = instance.AddComponent<Rigidbody>();
		rigidBody.mass = 1; 
		rigidBody.velocity = new Vector3(Random.Range(velocity*-1,velocity),0,Random.Range(velocity*-1,velocity));
		rigidBody.AddTorque(new Vector3(Random.Range(tourqe*-1,tourqe),Random.Range(tourqe*-1,tourqe),Random.Range(tourqe*-1,tourqe)),ForceMode.Impulse);

		var meshCollider = instance.GetComponent<MeshCollider>();
		meshCollider.material = (PhysicMaterial)Resources.Load("PhysicMaterials/Rubber");
		meshCollider.convex = true;

		var presentLifeTime = instance.AddComponent<PresentLifetime>();
		presentLifeTime.lifeTime = lifeTime;
		presentLifeTime.StartCountDown();

		StartCoroutine(Spawn());
	}
	
}
