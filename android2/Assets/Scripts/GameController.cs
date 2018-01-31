using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

	public GameObject BaseObject;
	public GameObject EndScreen;
	public Text ScoreText;
	public Button FinishButton;
	private BaseBuilder BaseBuilder;

	[HideInInspector]
	public List<GameObject> placeables;

	public void Start()
	{
		FinishButton.gameObject.SetActive(false);
		EndScreen.SetActive(false);

		BaseBuilder = BaseObject.GetComponent<BaseBuilder>();
		placeables = BaseBuilder.Placeables;

		StartCoroutine(FadeOutSprites());
	}

	public void Finish()
	{
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

		//Show score screen
		EndScreen.SetActive(true);
		ScoreText.text = dist.ToString();
		FinishButton.gameObject.SetActive(false);
	}

	//Restart scene
	public void Repeat()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
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

	public void RepositionPlaceables()
	{
		//Dynamically reposition placeables depending on the amount in a grid
		//Grid limit is 4x2 ::::
		//width limit is from -2.7 to 2.7
		// Y = -5.3 for top row, -7.8 for 2nd row
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
