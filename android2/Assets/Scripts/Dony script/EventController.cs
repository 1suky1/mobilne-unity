using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ovo je zapravo samo za detekciju nekog eventa i loadanje random clipa/answera generiranog u GMScirpt
public class EventController : MonoBehaviour {

    public PlayerController playerScript;
    public GameManagerScript SelectAnswerScript;
    public Canvas canvas;
    SpriteRenderer spriteRenderer;
	Sprite sprite;
	// Use this for initialization
	void Start () {
        //source-u nije dodan clip, nego se dodaje na temelju answer-a
        spriteRenderer = GetComponent<SpriteRenderer>();

    }

    void LoadComponents()
    {
        //ako je answer null, gamemanagerscirpt jos nije loadan ,da izbjegnemo nepotrebe errorove
        if (GameManagerScript.answer == null)
            return;

        var texture = Resources.Load<Texture2D>("Sprites/" + this.tag + "/" + GameManagerScript.answer);
        sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        sprite.name = texture.name;
        //spriteRenderer.sprite = sprite;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            LoadComponents();
            GameManagerScript.PlaySound();
            //spriteRenderer.sortingOrder = 1;
            playerScript.allowMoving = false;
            canvas.gameObject.SetActive(true);
            //spriteRenderer.sprite = sprite;

        }
    }

	public void UpdateSprite()
	{
		spriteRenderer.sprite = sprite;
		transform.localScale *= 3f;
	}
}
