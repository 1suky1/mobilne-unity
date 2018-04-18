using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EventTap : MonoBehaviour {

	//public GameObject Character;
	PlayerController playerController;

	// Use this for initialization
	void Start () {

		GameObject Character = GameObject.Find("Character");

		playerController = Character.GetComponent<PlayerController>();
	}

	private void OnMouseUp()
	{
		if(Input.GetMouseButtonUp(0))
		{
			if (!EventSystem.current.IsPointerOverGameObject())
			{
				playerController.allowMoving = true;
			}
		}
		else
		{
			if (!EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
			{
				playerController.allowMoving = true;
			}
		}
		
	}


	// Update is called once per frame
	void Update () {
		
	}
}
