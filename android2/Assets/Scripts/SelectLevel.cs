using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectLevel : MonoBehaviour {

	public Sprite lockSprite;
	private bool canLoad = false;

	public void Start()
	{
		string s = gameObject.name;
		int num = int.Parse(s.Substring(5));
		if (num > 1)
		{
			int prevLevel = num - 1;
			if (PlayerPrefs.HasKey("highScore" + "level" + prevLevel))
			{
				float score = PlayerPrefs.GetFloat("highScore" + "level" + prevLevel);
				if (score >= 0.75f)
					canLoad = true;
				else
					canLoad = false;
			}
		}
		else if (num == 1)
			canLoad = true;

		if (!canLoad)
			gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = lockSprite;
	}

	private void OnMouseDown()
	{
		if(canLoad)
			SceneManager.LoadScene(gameObject.name);
	}
}
