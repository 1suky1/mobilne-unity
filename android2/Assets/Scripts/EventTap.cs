using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventTap : MonoBehaviour {

	//public GameObject Character;
	PlayerController playerController;

	// Use this for initialization
	void Start () {

		GameObject Character = GameObject.Find("Character");

		playerController = Character.GetComponent<PlayerController>();
	}

	private void OnMouseDown()
	{
		playerController.allowMoving = true;
	}

	// Update is called once per frame
	void Update () {
		
	}
}
