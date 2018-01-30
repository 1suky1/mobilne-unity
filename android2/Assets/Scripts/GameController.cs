﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

	public GameObject BaseObject;
	private BaseBuilder BaseBuilder;

	[HideInInspector]
	public List<GameObject> placeables;

	public void Start()
	{
		BaseBuilder = BaseObject.GetComponent<BaseBuilder>();
		placeables = BaseBuilder.Placeables;

		StartCoroutine(FadeOutSprites());
	}

	public void Finish()
	{
		foreach (GameObject placeable in placeables)
		{
			GameObject anchor = placeable.transform.parent.gameObject;
			float dist = Vector3.Distance(placeable.transform.position, anchor.transform.position);
			print("Distance between " + placeable.name + " and " + anchor.name + " is: "+ dist);
		}
	}

	//Coroutine stuff -----------------------------------------------

	IEnumerator FadeOutSprites()
	{
		yield return new WaitForSeconds(3);
		foreach (GameObject p in placeables)
		{
			SpriteRenderer renderer = p.GetComponent<SpriteRenderer>();
			renderer.FadeSprite
				(this, 2,
				(SpriteRenderer r) => 
				{
					UnfadeSprites(r);
					RepositionPlaceables();
					r.gameObject.GetComponent<BoxCollider2D>().enabled = true;
				}
				);
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
