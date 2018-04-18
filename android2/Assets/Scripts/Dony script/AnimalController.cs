using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ovo je zapravo samo za detekciju nekog eventa i loadanje random clipa/answera generiranog u GMScirpt
public class AnimalController : MonoBehaviour {

    public PlayerController playerScript;
    public GameManagerScript SelectAnswerScript;
    public Canvas canvas;
    AudioSource source;
    SpriteRenderer spriteRenderer;
	Sprite sprite;
	// Use this for initialization
	void Start () {
        //source-u nije dodan clip, nego se dodaje na temelju answer-a
        source = GetComponent<AudioSource>();
        source.volume = 0.5f;
        spriteRenderer = GetComponent<SpriteRenderer>();
        LoadComponents();   
    }

    void LoadComponents()
    {
        //ako je answer null, gamemanagerscirpt jos nije loadan ,da izbjegnemo nepotrebe errorove
        if (GameManagerScript.answer == null)
            return;
        source.clip = Resources.Load<AudioClip>("Sounds/" + GameManagerScript.answer);

        var texture = Resources.Load<Texture2D>("Sprites/Animals/" + GameManagerScript.answer);
        sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        sprite.name = texture.name;
        //spriteRenderer.sprite = sprite;
    }
	
	// Update is called once per frame
	void Update () {
        //ako je odgovoreno pitanje, dize se flag u gamemanagerscripti
        //ako se animalCOntroller loada prije gamemanagerscripte, source.clip ce bit null
        //pa u update provjerimo ako je null, pozovemo load component
        if(GameManagerScript.nextAnswer || source.clip == null)
        {
            LoadComponents();
            GameManagerScript.nextAnswer = false;
        }
		
	}
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            source.Play();
            //spriteRenderer.sortingOrder = 1;
            playerScript.allowMoving = false;
            canvas.gameObject.SetActive(true);
			//spriteRenderer.sprite = sprite;
		}
    }

	public void UpdateSprite()
	{
		spriteRenderer.sprite = sprite;
	}
}
