using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drag : MonoBehaviour {

	//float distance = -1;

	private void OnMouseDrag()
	{
		//Vector3 mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);
		Vector3 objPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		//print("objPos " + objPos);
		transform.position = new Vector3(objPos.x, objPos.y, 0);
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
