using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void PlayGame()
	{
		//Should show all levels and let player choose
		//Levels should be locked before previous level is completed

		SceneManager.LoadScene("level1");
	}

	public void ExitGame()
	{
		Application.Quit();
	}

}
