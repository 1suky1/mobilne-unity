using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class BaseBuilder : MonoBehaviour
{

	public Sprite BaseImage;
	public Sprite BackgroundImage;
	public List<Sprite> Sprites;

	public bool generateBackground = false;
	public bool generateBase = false;
	public bool generateAnchors = false;

	[HideInInspector]
	public GameObject BaseImageObject;
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
                Background = new GameObject                {
                    name = "Background"
                };
                Background.transform.parent = gameObject.transform;
				Background.transform.position = new Vector3(0f, -3.49f, 2f);
				Background.AddComponent<SpriteRenderer>().sprite = BackgroundImage;
			}
			else
			{
				Background.GetComponent<SpriteRenderer>().sprite = BackgroundImage;
			}
		}

		if(generateBase)
		{
			generateBase = false;
			if(BaseImageObject == null)
			{
                BaseImageObject = new GameObject                {
                    name = "Base Image"
                };
                BaseImageObject.transform.parent = gameObject.transform;
				BaseImageObject.transform.position = new Vector3(0f, 0f, 1f);
				BaseImageObject.AddComponent<SpriteRenderer>().sprite = BaseImage;
			}
			else
			{
				BaseImageObject.GetComponent<SpriteRenderer>().sprite = BaseImage;
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
                GameObject Anchor = new GameObject                {
                    name = "Anchor - " + s.name
                };
                Anchor.transform.parent = gameObject.transform;
				Anchor.transform.position = Vector3.zero;
				Anchors.Add(Anchor);

                GameObject Placeable = new GameObject{
                    name = "Placeable - " + s.name,
                };
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
