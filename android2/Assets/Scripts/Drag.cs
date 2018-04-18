using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drag : MonoBehaviour {

    //float distance = -1;
    Vector3 objPos;
    private void OnMouseDrag()
	{
        //Vector3 mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);
        objPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		transform.position = new Vector3(objPos.x, objPos.y, 0);
	}

    private void OnMouseUp()
    {
        Vector3 anchorPos = this.transform.parent.position;
        float distance = Vector2.Distance(objPos, anchorPos);
        if (Mathf.Abs(distance) < 1.0f)
            transform.position = new Vector3(anchorPos.x, anchorPos.y, anchorPos.z);
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
