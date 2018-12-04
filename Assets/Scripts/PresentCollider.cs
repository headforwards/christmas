using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PresentCollider : MonoBehaviour {

void OnCollisionEnter(Collision collision)
    {
        var present = collision.gameObject;

        // when collide destroy the pumpkin.
        if (present.name.ToLower().StartsWith("present"))
        {
    //        Instantiate(Resources.Load("splat"), present.transform.position,Quaternion.identity);
            Destroy(present);

           EventManager.TriggerEvent("scoreupdate", gameObject.name);
        }
    }      
	
}
