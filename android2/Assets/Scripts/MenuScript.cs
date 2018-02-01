using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

 [ExecuteInEditMode]
public class MenuScript : MonoBehaviour {

	[Header("Update Levels")]
	public bool updateLevels = false;
	[Space]
	[Space]
	public float distance = 6f;
	public List<Sprite> LevelSprites;
	public List<Sprite> Stars;
	[HideInInspector]
	public List<GameObject> levels;
	//public List<GameObject> levelScores;

	private int numSCenes;
	private int cPos; //Position of camera to be saved in player prefs to remember where to start when going back to menu

	// Use this for initialization
	void Start ()
	{
		//PlayerPrefs.DeleteAll();
		//Debug.Log("playerprefs called");

		numSCenes = SceneManager.sceneCountInBuildSettings - 1;
		if(PlayerPrefs.HasKey("MenuCameraPos"))
		{
			cPos = PlayerPrefs.GetInt("MenuCameraPos");
			Camera.main.transform.position = new Vector3((cPos*distance), 0, -10);
		}

		updateLevels = true;
			
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(updateLevels)
		{
			updateLevels = false;
			numSCenes = SceneManager.sceneCountInBuildSettings - 1;

			//delete go + clean list
			foreach (GameObject go in levels)
			{
				DestroyImmediate(go);
			}
			levels.Clear();

			for (int i = 0; i < numSCenes; i++)
			{
				GameObject go = new GameObject();
				go.name = "level" + (i + 1);
				SpriteRenderer sr = go.AddComponent<SpriteRenderer>();
				sr.sprite = LevelSprites[i];
				go.AddComponent<BoxCollider2D>();
				SelectLevel sl = go.AddComponent<SelectLevel>();
				sl.lockSprite = Stars[6];

				GameObject score = new GameObject();
				score.transform.SetParent(go.transform);
				score.transform.position = new Vector3(0, -4.5f, 0);
				score.name = "level" + (i + 1) + "score";
				score.AddComponent<SpriteRenderer>();
				SetScoreSprite(score, go.name);


				go.transform.position = new Vector3(i * distance, 0, 0);

				levels.Add(go);
			}

		}
	}

	public void SetScoreSprite(GameObject s, string goName)
	{
		SpriteRenderer sr = s.GetComponent<SpriteRenderer>();
		sr.sprite = Stars[0];
		
		if (PlayerPrefs.HasKey("highScore" + goName))
		{
			float score = PlayerPrefs.GetFloat("highScore" + goName);
			
			if (score >= 0.75f)
				sr.sprite = Stars[1];
			if(score >= 0.8f)
				sr.sprite = Stars[2];
			if (score >= 0.82f)
				sr.sprite = Stars[3];
			if (score >= 0.85f)
				sr.sprite = Stars[4];
			if (score >= 0.9f)
				sr.sprite = Stars[5];
		}

	}


	public void ScrollLeft()
	{
		if(Camera.main.transform.position.x > 0)
		{
			Vector3 cameraPos = Camera.main.transform.position;
			Vector3 target = new Vector3(-distance, 0, 0);

			Camera.main.transform.Translate(target);
			cPos--;
			PlayerPrefs.SetInt("MenuCameraPos", cPos);
		}
	}

	public void ScrollRight()
	{
		if (Camera.main.transform.position.x < (distance * (numSCenes - 1)))
		{
			Vector3 cameraPos = Camera.main.transform.position;
			Vector3 target = new Vector3(distance, 0, 0);

			Camera.main.transform.Translate(target);
			cPos++;
			PlayerPrefs.SetInt("MenuCameraPos", cPos);
		}
	}

	public void ExitGame()
	{
		//PlayerPrefs.Save();
		Application.Quit();
	}

}
