using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snow : MonoBehaviour {

	void Start () 
	{
		ParticleSystem p = GetComponent<ParticleSystem> ();
		for (float i = 0; i < 10f; i++) {
			p.Simulate (1f);
		}
		
	}
}
