using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//neznam jel mozes ovo di mergat, samo za automove, mozda u GameCOntroller
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour {

    //Rigidbody2D rigidbody;
    Animator animator;
    //zapravo kad player hita neki box, tj triggera event, poanta je da stane i odgovori na pitanje i kasnije krene dalje
    public bool allowMoving;
	// Use this for initialization
	void Start () {
        //rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        allowMoving = false;
    }
	
	// Update is called once per frame
	void Update () {

        //animacije za movment/idle
        if(allowMoving)
        {
            Movement();
            animator.SetFloat("speed", 1.5f);
        }
        else
        {
            animator.SetFloat("speed", 0);
        }
        
	}

    void Movement()
    {
        //jadna movment komanda
        transform.Translate(Vector2.right * 7f * Time.deltaTime);
    }
}
