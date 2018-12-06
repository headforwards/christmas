using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PresentLifetime : MonoBehaviour {


	[Tooltip("Lifetime in seconds before present is disposed")]
	public float lifeTime = 4.0f;

	public float fadeOutTime = 0.5f;

	public void StartCountDown () {
		StartCoroutine(DestroyPresent());
	}
	
	IEnumerator DestroyPresent()
	{
		yield return new WaitForSeconds(lifeTime);

		try
		{
			var destroyEffect = Instantiate(Resources.Load("effects/presentDestroyed"),gameObject.transform.position,Quaternion.identity);
			
			Destroy(gameObject, fadeOutTime * 2);
			Destroy(destroyEffect,fadeOutTime * 3);

			StartCoroutine(PauseThenScale());
		}
		catch
		{
			//
		}
	}

	IEnumerator PauseThenScale()
	{
		yield return new WaitForSeconds(fadeOutTime);
		StartCoroutine(ScaleOverTime(fadeOutTime));
	}

	IEnumerator ScaleOverTime(float time)
     {
         Vector3 originalScale = gameObject.transform.localScale;
         Vector3 destinationScale = new Vector3(0.01f, 0.01f, 0.01f);
         
         float currentTime = 0.0f;
         
         do
         {
             gameObject.transform.localScale = Vector3.Lerp(originalScale, destinationScale, currentTime / time);
             currentTime += Time.deltaTime;
             yield return null;
         } while (currentTime <= time);
     }
}
