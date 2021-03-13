using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

    public Splatter splatter;

    //ref to rigidbody
    private Rigidbody2D myBody;
    [SerializeField]
    private float speed = 5f;                     //movement spped of player
    [SerializeField]
    private float jumpForce = 12;                 //jump force of player
    private bool moving = false;
                                                  
    void Start ()
    {
        myBody = GetComponent<Rigidbody2D>();      //we get the rigidbody component
    }
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        Movement();
        Jump();
    }

    void Movement()
    {
        //we get the keyboard inputs here
        float h = Input.GetAxisRaw("Horizontal");
        if (h > 0.01f)
        {
            moving = true;
        }
        else
        {
            moving = false;
        }
        myBody.velocity = new Vector3(h * speed, myBody.velocity.y);
    }

    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //we first make the y velocity zero
            myBody.velocity = new Vector2(myBody.velocity.x, 0);
            //and then we add the jumpforce
            myBody.velocity = new Vector2(myBody.velocity.x, jumpForce);
        }
    }
    //creates the splatter when moving and touching object with tag sureface
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Surface") && moving)
        {
            Splatter splatterObj = (Splatter)Instantiate(splatter, other.transform.position, Quaternion.identity);
        }
    }



}
