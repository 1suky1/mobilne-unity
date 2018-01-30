using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class BaseBuilder : MonoBehaviour
{

	public Sprite BackgroundImage;
	public List<Sprite> Sprites;

	public bool generateBackground = false;
	public bool generateAnchors = false;

	[HideInInspector]
	public GameObject Background;
	[HideInInspector]
	public List<GameObject> Anchors;
	[HideInInspector]
	public List<GameObject> Placeables;

	void Update ()
	{
		if(generateBackground)
		{
			generateBackground = false;
			if(Background == null)
			{
				Background = new GameObject();
				Background.name = "Background";
				Background.transform.parent = gameObject.transform;
				Background.transform.position = new Vector3(0f, 0f, 1f);
				Background.AddComponent<SpriteRenderer>().sprite = BackgroundImage;
			}
			else
			{
				Background.GetComponent<SpriteRenderer>().sprite = BackgroundImage;
			}			
		}

		if(generateAnchors)
		{
			generateAnchors = false;
			
			//Clear lists and delete GameObjects
			if(Placeables != null || Anchors != null)
			{				
				foreach (GameObject p in Placeables)
				{
					DestroyImmediate(p);
				}
				foreach (GameObject a in Anchors)
				{
					DestroyImmediate(a);
				}
				Placeables.Clear();
				Anchors.Clear();
			}

			foreach (Sprite s in Sprites)
			{
				GameObject Anchor = new GameObject();
				Anchor.name = "Anchor - " + s.name;
				Anchor.transform.parent = gameObject.transform;
				Anchor.transform.position = Vector3.zero;
				Anchors.Add(Anchor);

				GameObject Placeable = new GameObject();
				Placeable.name = "Placeable - " + s.name;
				Placeable.transform.parent = Anchor.transform;
				Placeable.transform.position = Vector3.zero;
				Placeable.AddComponent<SpriteRenderer>().sprite = s;
				Placeable.AddComponent<BoxCollider2D>();
				Placeable.GetComponent<BoxCollider2D>().enabled = false;
				Placeable.AddComponent<Drag>();

				Placeables.Add(Placeable);
			}

		}
	}



}
