using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour {

	public GameObject[] cheminees;

	public int levelIndex;

	public GameObject WinPanel;
	public GameObject PausePanel;

	public bool win;

	public bool pause;

	public bool dead;

	public vp_FPInput playerInput;

	public void Test () {

		foreach (GameObject c in cheminees) {
			if (!c.GetComponent<Cheminee> ().isFull) {
				return;
			}
		}

		Win ();
	}

	public void LoadLevel( int index )
	{
		GameController.controller.LoadLevel (index);
	}


	void Update()
	{
		if (win) {

			if (Input.GetKeyDown (KeyCode.Space)) {
				if ( levelIndex < 10 )
					GameController.controller.LoadLevel (GameController.controller.maxLevel);
				else 
					GameController.controller.LoadLevel (-1);

			}

			if (Input.GetKeyDown (KeyCode.Escape)) {
				GameController.controller.LoadLevel (-1);
			}

			if (Input.GetKeyDown (KeyCode.R)) {
				GameController.controller.LoadLevel (levelIndex);
			}

		} else {
			if (pause || dead) {
				if (Input.GetKeyDown (KeyCode.R)) {
					GameController.controller.LoadLevel (levelIndex);
				}
			}

			if (Input.GetKeyDown (KeyCode.Escape) && !dead)
				TogglePause ();
			
			if ( dead && Input.GetKeyDown(KeyCode.Escape ) )
				GameController.controller.LoadLevel (-1);
			
		}



	}
	
	void TogglePause()
	{
		if (!pause) {
			PausePanel.SetActive (true);
			//Time.timeScale = 0f;
			pause = true;
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
			if(!dead )
				playerInput.MouseCursorForced = true;

		} else {

			if(!dead )
				playerInput.MouseCursorForced = false;

			PausePanel.SetActive (false);
			//Time.timeScale = 1f;
			pause = false;
			Cursor.visible = false;
			Cursor.lockState = CursorLockMode.Confined;



		}
	}
	
	void Win () {
		win = true;
		int maxLevel = Mathf.Max (levelIndex + 1, GameController.controller.maxLevel);
		GameController.controller.maxLevel = maxLevel;

		GameController.controller.Save ();

		WinPanel.SetActive (true);

	}
}
