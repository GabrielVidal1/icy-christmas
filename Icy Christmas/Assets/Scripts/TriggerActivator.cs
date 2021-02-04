using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerActivator : MonoBehaviour {

	public GameObject obj;

	void OnTriggerEnter()
	{
		obj.SetActive (true);
	}
}
