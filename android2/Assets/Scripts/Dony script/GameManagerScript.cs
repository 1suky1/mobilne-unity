using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Linq;

//ovo je zapravo ko tvoj gamecontroller, mozda bi se dalo mergat
public class GameManagerScript : MonoBehaviour {

    //canvas za listu buttona za odgovore
    public Canvas canvas;
    //za allowanje movmenta
    public PlayerController playerController;
    [HideInInspector]
    public static AudioSource answerSound;
    public List<GameObject> Events;
	int numEvent = 0;

    //lista pozicija za buttone u canvasu
    //tocan odgovor
    public static string answer;
    public static bool nextAnswer;
    List<Vector3> positons;
    Texture2D[] images;
    AudioClip[] clips;
    AudioSource rightAnswer;
    AudioSource wrongAnswer;

	private void Awake()
	{
		Screen.orientation = ScreenOrientation.LandscapeLeft;
	}

	private void Start()
    {
        nextAnswer = false;
        images = Resources.LoadAll<Texture2D>("Sprites/" + this.tag); //load image za pitanja
        canvas.gameObject.SetActive(false); //disable canvas da se ne vidi na pocetku
        positons = new List<Vector3>() //pozicije za buttone/odgovore
        {
            new Vector3(-100,100,0),
            new Vector3(100,100,0),
            new Vector3(-100,-100,0),
            new Vector3(100,-100,0)
        };

        //load zvukova za korektan odgovor
        clips = Resources.LoadAll<AudioClip>("Sounds/" + this.tag).Where(cl => !cl.name.Contains("_answer")).ToArray(); 

        //load komponenti sa gamemanagera za tocan/netocan odgovor
        AudioSource[] answerClips = GetComponents<AudioSource>(); 
        rightAnswer = answerClips[0];
		rightAnswer.volume = 0.5f;
        wrongAnswer = answerClips[1];
		wrongAnswer.volume = 0.5f;
        answerSound = answerClips[2];
        answerSound.volume = 0.5f;

        //loadamo random i random sound predstavljamo ko tocan odgovor i na temelju naziva (cat se zovu i image i sound) -
        //u answer spremamo naziv s kojim kasnije provjeravamo tocnost odabranog odgovora
        System.Random r = new System.Random();
        int randomIndex = r.Next(clips.Length);
        answer = clips[randomIndex].name;

        answerSound.clip = clips[randomIndex];

        for (int i = 0; i < 4; i++)
        {
            //posto moram loadat slike iz resources foldera ko Texture2D, kreiram sprite
            var sprite = Sprite.Create(images[i], new Rect(0, 0, images[i].width, images[i].height), new Vector2(0.5f, 0.5f));
            //stavljam ime kak bi mogo prepoznat koji je odgovor poslje
            sprite.name = images[i].name;
            AddButtonsToCanvas(sprite, i);
        }

        AddReplayButton();
    }

	private void AddButtonsToCanvas(Sprite image, int i)
    {
        GameObject button = new GameObject();

        button.AddComponent<CanvasRenderer>();
        button.AddComponent<RectTransform>().sizeDelta = new Vector2(200, 200);
        Button mButton = button.AddComponent<Button>();
        Image mImage = button.AddComponent<Image>();
        mImage.sprite = image;
        mButton.targetGraphic = mImage;

		button.transform.SetParent(canvas.transform);
		button.GetComponent<RectTransform>().anchoredPosition = positons[i];
		//button.transform.position = positons[i];
        
        button.GetComponent<Button>().onClick.AddListener(CheckAnswer); //metoda za provjeru tocvnosti odgovora
        button.name = image.name; //stavljamo name kak bi poslje mogli dohvatit name pritisnutog buttona
    }

    private void AddReplayButton()
    {
<<<<<<< HEAD
        var texture = Resources.Load<Texture2D>("Sprites/button_repeat");
=======
        var texture = Resources.Load<Texture2D>("Sprites/Bankgorunds and stuff/replay_button");
>>>>>>> 8e3d28b543aece99b0602dba25019db09fe94472
        var sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        //stavljam ime kak bi mogo prepoznat koji je odgovor poslje
        sprite.name = texture.name;

        GameObject button = new GameObject();

        button.AddComponent<CanvasRenderer>();
        button.AddComponent<RectTransform>().sizeDelta = new Vector2(150, 150);
        Button mButton = button.AddComponent<Button>();
        Image mImage = button.AddComponent<Image>();
        mImage.sprite = sprite;
        mButton.targetGraphic = mImage;

        button.transform.SetParent(canvas.transform);
        button.GetComponent<RectTransform>().anchoredPosition = new Vector3(-300,0,0);
        //button.transform.position = positons[i];

        button.GetComponent<Button>().onClick.AddListener(PlaySound); //metoda za provjeru tocvnosti odgovora
        button.name = texture.name; //stavljamo name kak bi poslje mogli dohvatit name pritisnutog buttona
    }

    void CheckAnswer()
    {
        //dohvacamo name pritisnutog buttona
		var imageName = EventSystem.current.currentSelectedGameObject.name;
        if (imageName.ToLower().Contains(answer))
        {
            if(!rightAnswer.isPlaying)
                //play applause
                rightAnswer.Play();
            //remove canvas/answer list/button list
            canvas.gameObject.SetActive(false);
            //allow moving
            //playerController.allowMoving = true;
            //generate random questin/answer
            System.Random r = new System.Random();
            int randomIndex = r.Next(clips.Length);
            answer = clips[randomIndex].name;

            answerSound.clip = clips[randomIndex];
            answerSound.volume = 0.5f;
            //flag za generiranje sljedeceg pitanja u animator controlleru
            nextAnswer = true; 	

			//Change sprite on event
			if(numEvent < Events.Count)
			{
				numEvent++;
			}

        }
            
        else
        {
            if(!wrongAnswer.isPlaying)
                wrongAnswer.Play();
        }
            
    }

    public static void PlaySound()
    {
        if(!answerSound.isPlaying)
            answerSound.Play();
    }

}
