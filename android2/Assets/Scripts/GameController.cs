using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

	[Header("Base Object")]
	public GameObject BaseObject;

	[Header("UI")]
	public GameObject EndScreen;
	//public Text ScoreText;
	public Button FinishButton;
	public Button ExitButton;
	public Button RepeatButton;
	public Button NextButton;
	public List<GameObject> Stars;

    [Header("Sprites")]
	public Sprite FullStar;
	public Sprite HalfStar;

	private BaseBuilder BaseBuilder;
    private AudioSource winSound;
    private AudioSource loseSound;

    [HideInInspector]
	public List<GameObject> placeables;

	public void Awake()
	{
		Screen.orientation = ScreenOrientation.Portrait;
	}

	public void Start()
	{
        AudioSource[] answerClips = GetComponents<AudioSource>();
        winSound = answerClips[0];
        winSound.volume = 0.5f;
        loseSound = answerClips[1];
        loseSound.volume = 0.5f;

        FinishButton.gameObject.SetActive(false);
		EndScreen.SetActive(false);

		//RepeatButton.GetComponent<RectTransform>().anchoredPosition
		NextButton.gameObject.SetActive(false);

		BaseBuilder = BaseObject.GetComponent<BaseBuilder>();
		placeables = BaseBuilder.Placeables;

		StartCoroutine(FadeOutSprites());
	}

	public void Finish()
	{
		//calculate average distance between all placeables
		float dist = 0f;
		foreach (GameObject placeable in placeables)
		{
			GameObject anchor = placeable.transform.parent.gameObject;
			dist += Vector3.Distance(placeable.transform.position, anchor.transform.position);

			//Disable dragging on placeables
			placeable.GetComponent<Drag>().enabled = false;
			placeable.GetComponent<Collider2D>().enabled = false;
		}
		dist = dist / placeables.Count;

		//normalize distance to score
		float score = 1f - dist;

        //Win should be ~0.2 or less dist
        /* score
		 * 0.75 = 1 star
		 * 0.8 = 1.5 star
		 * 0.82 = 2 star
		 * 0.85 = 2.5 star
		 * 0.9 = 3 star
		 */
        //if win show next button and reposition replay button
        if (score >= 0.75f)
        {
            winSound.Play();
            RectTransform rect = RepeatButton.GetComponent<RectTransform>();
            rect.anchoredPosition = new Vector3(-5.8f, rect.anchoredPosition.y, 0);
            NextButton.gameObject.SetActive(true);

        }
        else
            loseSound.Play();

		//Show score screen
		EndScreen.SetActive(true);
		//ScoreText.text = score.ToString();
		DisplayStars(score, Stars);
		FinishButton.gameObject.SetActive(false);
		SetPlayerScore(score);
	}

	private void DisplayStars(float score, List<GameObject> stars)
	{
		if (score >= 0.75f)
			stars[0].GetComponent<Image>().sprite = FullStar;
		if (score >= 0.8f)
			stars[1].GetComponent<Image>().sprite = HalfStar;
		if (score >= 0.82f)
			stars[1].GetComponent<Image>().sprite = FullStar;
		if (score >= 0.85f)
			stars[2].GetComponent<Image>().sprite = HalfStar;
		if (score >= 0.9f)
			stars[2].GetComponent<Image>().sprite = FullStar;
	}

	private void SetPlayerScore(float score)
	{
		string h = "highScore";
		string s = SceneManager.GetActiveScene().name;
		float prevScore = 0;

		//Check if there's a previous high score
		if (PlayerPrefs.HasKey(h+s))
			prevScore = PlayerPrefs.GetFloat(h + s);

		//If current score is higher than previous, make it new high score
		if (prevScore < score)
			PlayerPrefs.SetFloat(h + s, score);

		//Debug.Log("prevscore: " + prevScore);
	}


	//Buttons -------------------------------------------------------

	public void Repeat()
	{
		PlayerPrefs.Save();
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}

	public void ExitLevel()
	{
		PlayerPrefs.Save();
		SceneManager.LoadScene("menu");
	}

	public void NextLevel()
	{
		PlayerPrefs.Save();
        if (SceneManager.GetActiveScene().buildIndex + 1 >= SceneManager.sceneCountInBuildSettings)
            SceneManager.LoadScene("menu");
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
		if (SceneManager.GetActiveScene().buildIndex + 1 >= SceneManager.sceneCountInBuildSettings)
			SceneManager.LoadScene("menu");
		else
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
	}

	//Coroutine stuff -----------------------------------------------

	IEnumerator FadeOutSprites()
	{
		yield return new WaitForSeconds(3);
		foreach (GameObject p in placeables)
		{
			SpriteRenderer renderer = p.GetComponent<SpriteRenderer>();
			renderer.FadeSprite(this, 2, (SpriteRenderer r) => 
				{
					UnfadeSprites(r);
					//Create outlines
					CreateOutlines(p);

					RepositionPlaceables();
					r.gameObject.GetComponent<BoxCollider2D>().enabled = true;
					FinishButton.gameObject.SetActive(true);
				});
		}
	}

	public void UnfadeSprites(SpriteRenderer renderer)
	{
		Color color = renderer.color;
		color.a = 255f;
		renderer.color = color;
	}

	private void CreateOutlines(GameObject p)
	{
		GameObject outline = new GameObject();
		SpriteRenderer sr = outline.AddComponent<SpriteRenderer>();
		sr.sprite = p.GetComponent<SpriteRenderer>().sprite;
		Color c = new Color(1f, 1f, 1f, 0.1f);
		sr.color = c;
		outline.name = "Outline - " + p.name;
		outline.transform.position = new Vector3(p.transform.parent.transform.position.x, p.transform.parent.transform.position.y, 0.5f);
		
	}

	public void RepositionPlaceables()
	{
		//Dynamically reposition placeables depending on the amount in a grid
		//Grid limit is 4x2 ::::
		//width limit is from -2.7 to 2.7
		// Y = -5.3 for top row, -7.8 for 2nd row
		//not exactly dnamic atm
		float x_min = -2.7f;
		float x_max = 2.7f;
		float y_top = -5.3f;
		float y_bot = -7.8f;
		float distance = x_max * 2;
		float gap = 0f;

		if(placeables.Count <= 4)
		{
			gap = (distance / (placeables.Count - 1));
			y_top = ((y_top - y_bot) / 2) + y_bot;
		}
		else
		{
			gap = distance / 3;
		}	

		float x = x_min;
		for (int i = 0; i < placeables.Count; i++)
		{
			if (i < 4)
			{
				placeables[i].transform.position = new Vector3(x, y_top, 0);
				x += gap;
			}
			else if (i == 4)
			{
				x = x_min;
				placeables[i].transform.position = new Vector3(x, y_bot, 0);
				x += gap;
			}				
			else
			{
				placeables[i].transform.position = new Vector3(x, y_bot, 0);
				x += gap;
			}
				
		}


	}
}
