using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class GameController : MonoBehaviour {

	public static GameController controller;

	public int maxLevel;

	void Awake () {

		if (controller == null) {
			DontDestroyOnLoad (gameObject);
			controller = this;
		} else if (controller != this) {
			Destroy (gameObject);
		}

		controller.Load ();

	}

	public void LoadLevel (int levelIndex)
	{
		Time.timeScale = 1f;
		if (levelIndex == -1)
			SceneManager.LoadScene ("Menu");
		else if (levelIndex > -1 && levelIndex < 100) {/////////////////////////////
			// levelIndex == 0 : tutorial
			SceneManager.LoadScene (("Level" + levelIndex.ToString ()));
		}
	}
		

	public void Save()
	{

		BinaryFormatter bf = new BinaryFormatter ();
		FileStream file = File.Create( Application.persistentDataPath + "/GameData.dat");

		GameData data = new GameData();

		data.maxLevel = maxLevel;

		bf.Serialize( file, data );
		file.Close();
	}

	public void Load()
	{
		if (File.Exists (Application.persistentDataPath + "/GameData.dat")) {

			BinaryFormatter bf = new BinaryFormatter ();
			FileStream file = File.Open (Application.persistentDataPath + "/GameData.dat", FileMode.Open); 

			GameData data = (GameData)bf.Deserialize (file);
			file.Close ();

			maxLevel = data.maxLevel;

		} else {

			maxLevel = 0;

		}
	}




}

[Serializable]
class GameData
{
	public int maxLevel;


}