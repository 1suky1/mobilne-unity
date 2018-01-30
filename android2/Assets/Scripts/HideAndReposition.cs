using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideAndReposition : MonoBehaviour {

	// Use this for initialization
	void Start ()
	{
		StartCoroutine(FadeOutSprites());
	}

	IEnumerator FadeOutSprites()
	{
		yield return new WaitForSeconds(3);
		SpriteRenderer renderer = gameObject.GetComponent<SpriteRenderer>();
		renderer.FadeSprite
			(this, 2,
			(SpriteRenderer r) => { UnfadeSprites(r); }
			);
		//gameObject.SetActive(false);
	}

	public void UnfadeSprites(SpriteRenderer renderer)
	{
		Color color = renderer.color;
		color.a = 255f;
		renderer.color = color;
	}

}
