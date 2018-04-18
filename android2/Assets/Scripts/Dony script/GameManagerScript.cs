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
        images = Resources.LoadAll<Texture2D>("Sprites/Animals"); //load image za pitanja
        canvas.gameObject.SetActive(false); //disable canvas da se ne vidi na pocetku
        positons = new List<Vector3>() //pozicije za buttone/odgovore
        {
            new Vector3(-100,100,0),
            new Vector3(100,100,0),
            new Vector3(-100,-100,0),
            new Vector3(100,-100,0)
        };

        //load zvukova za korektan odgovor
        clips = Resources.LoadAll<AudioClip>("Sounds").Where(cl => !cl.name.Contains("_answer")).ToArray(); 

        //load komponenti sa gamemanagera za tocan/netocan odgovor
        AudioSource[] answerClips = GetComponents<AudioSource>(); 
        rightAnswer = answerClips[0];
        rightAnswer.volume = 0.5f;
        wrongAnswer = answerClips[1];
        wrongAnswer.volume = 0.5f;

        //loadamo random i random sound predstavljamo ko tocan odgovor i na temelju naziva (cat se zovu i image i sound) -
        //u answer spremamo naziv s kojim kasnije provjeravamo tocnost odabranog odgovora
        System.Random r = new System.Random();
        answer = clips[r.Next(clips.Length)].name;

        for (int i = 0; i < 4; i++)
        {
            //posto moram loadat slike iz resources foldera ko Texture2D, kreiram sprite
            var sprite = Sprite.Create(images[i], new Rect(0, 0, images[i].width, images[i].height), new Vector2(0.5f, 0.5f));
            //stavljam ime kak bi mogo prepoznat koji je odgovor poslje
            sprite.name = images[i].name;
            AddButtonsToCanvas(sprite, i);
        }
           
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

    void CheckAnswer()
    {
        //dohvacamo name pritisnutog buttona
		var imageName = EventSystem.current.currentSelectedGameObject.name;
        if (imageName.ToLower().Contains(answer))
        {
            //play applause
            rightAnswer.Play();
            //remove canvas/answer list/button list
            canvas.gameObject.SetActive(false);
            //allow moving
            //playerController.allowMoving = true;
            //generate random questin/answer
            System.Random r = new System.Random();
            answer = clips[r.Next(clips.Length)].name;
            //flag za generiranje sljedeceg pitanja u animator controlleru
            nextAnswer = true; 	

			//Change sprite on event
			if(numEvent < Events.Count)
			{
				Events[numEvent].GetComponent<AnimalController>().UpdateSprite();
				Events[numEvent].GetComponent<AnimalController>().enabled = false;
				numEvent++;
			}

        }
            
        else
            wrongAnswer.Play();
    }

}
