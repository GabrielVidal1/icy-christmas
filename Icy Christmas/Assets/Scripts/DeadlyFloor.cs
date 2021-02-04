using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadlyFloor : MonoBehaviour {

	public GameObject deathCam;
	public LevelManager lm;

	public GameObject deathSound;

	void OnTriggerEnter( Collider other )
	{
		if (other.tag == "Player") {

			deathCam.SetActive (true);
			Destroy (other.gameObject);
			Instantiate (deathSound, transform.position, Quaternion.identity);
			lm.dead = true;
		}
	}
}
