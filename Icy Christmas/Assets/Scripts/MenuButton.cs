using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuButton : MonoBehaviour {


	public GameObject lockIcon;
	public int levelIndex;
	public Text levelText;

	private Button button;

	void Start () {

		button = GetComponent<Button> ();

		if (levelIndex == 0)
			levelText.text = "Tutorial";
		else
			levelText.text = "Level " + levelIndex.ToString ();

		if (GameController.controller.maxLevel < levelIndex) {
			button.enabled = false;
			lockIcon.SetActive (true);
			levelText.gameObject.SetActive (false);
		}


	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
