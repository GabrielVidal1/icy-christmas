using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sound : MonoBehaviour {



	void Start () {

		AudioSource audio = GetComponent<AudioSource> ();

		AudioClip clip = audio.clip;

		float duration = clip.length;

		Destroy (gameObject, duration + 0.1f);

	}
}
