using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PresentCollider : MonoBehaviour {


    public float fadeOutTime = 0.1f;
    public float volume = 0.3f;

    void OnCollisionEnter(Collision collision)
    {
        var present = collision.gameObject;

        // when collide destroy the present.
        if (present.name.ToLower().StartsWith("present"))
        {
           CollectPresent(present);

           EventManager.TriggerEvent(EventNames.UpdateScore, gameObject.name);
        }
    }     

    void CollectPresent(GameObject present)
	{
		try
		{
			var collectedEffect = Instantiate(Resources.Load("effects/presentCollected"),present.transform.position,Quaternion.identity) as GameObject;

            var idx = Random.Range(1,4);
            string audioClip = string.Empty;
            switch(idx)
            {
                case 1 : audioClip = "DM-CGS-15";
                    break;
                case 2 : audioClip = "DM-CGS-26";
                    break;
                case 3 : audioClip = "DM-CGS-45";
                    break;

            }
            var audioSource = collectedEffect.GetComponent<AudioSource>();
            audioSource.clip = Resources.Load(audioClip) as AudioClip;
            audioSource.volume = volume;
            audioSource.Play();
			
			Destroy(present, fadeOutTime * 2);
			Destroy(collectedEffect,fadeOutTime * 5);

			StartCoroutine(PauseThenScale(present));
		}
		catch
		{
			//
		}
	}

	IEnumerator PauseThenScale(GameObject present)
	{
		yield return new WaitForSeconds(fadeOutTime);
		StartCoroutine(ScaleOverTime(present));
	}

	IEnumerator ScaleOverTime(GameObject present)
     {
         Vector3 originalScale = present.transform.localScale;
         Vector3 destinationScale = new Vector3(0.01f, 0.01f, 0.01f);
         
         float currentTime = 0.0f;
         
         do
         {
             present.transform.localScale = Vector3.Lerp(originalScale, destinationScale, currentTime / fadeOutTime);
             currentTime += Time.deltaTime;
             yield return null;
         } while (currentTime <= fadeOutTime);
     } 
	
}
