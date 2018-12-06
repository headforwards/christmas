using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PresentLifetime : MonoBehaviour {


	[Tooltip("Lifetime in seconds before present is disposed")]
	public float lifeTime = 4.0f;

	public void StartCountDown () {
		StartCoroutine(DestroyPresent());
	}
	
	IEnumerator DestroyPresent()
	{
		yield return new WaitForSeconds(lifeTime);

		try
		{
			Destroy(gameObject);
		}
		catch
		{
			//
		}
	}
}
