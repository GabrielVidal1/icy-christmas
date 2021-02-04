using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MainMenu : MonoBehaviour {

	public GameObject mainMenu;
	public GameObject levelSelectionPanel;
	public GameObject optionsMenu;

	public Slider volumeSlider;

	public void LoadLevel( int index )
	{
		GameController.controller.LoadLevel (index);
	}


	public void MainMenuToggle()
	{
		levelSelectionPanel.SetActive (false);
		optionsMenu.SetActive (false);

		mainMenu.SetActive (true);



	}

	public void OptionsMenu()
	{
		mainMenu.SetActive (false);

		optionsMenu.SetActive (true);
	}


	public void LevelSelectionPanel()
	{
		mainMenu.SetActive (false);

		levelSelectionPanel.SetActive (true);

	}

	public void UpdateMasterVolume()
	{

		AudioListener.volume = volumeSlider.value;


	}

	public void ResetProgression()
	{
		GameController.controller.maxLevel = 0;
		GameController.controller.Save ();
		MainMenuToggle ();

	}

	public void QuitButton()
	{
		Application.Quit ();
	}
}
