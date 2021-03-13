using UnityEngine;
using System.Collections;

public class BallCOntroller : MonoBehaviour {

    public Splatter splatter; //ref to object to spawn
    private LineRenderer lineRenderer;//ref to line renderee
    [SerializeField]
    private float speed = 2f;//speed multiplier of ball
    private RaycastHit2D hit;//hit ref
    private Rigidbody2D mybody;//ref to rigidbody of ball
    //direction of player movement
    private Vector3 direction;//ref to direction in which the ball will move
    private bool addForce = false;//ref to addforce bool
    private float distance;//ref to the distance between ball and mouse touch

    private void Awake()
    {
        //we get the following componenets attached to the object
        lineRenderer = GetComponent<LineRenderer>();
        mybody = GetComponent<Rigidbody2D>();
    }

    // Use this for initialization
    void Start ()
    {
        //at start direction is zero
        direction = Vector3.zero;
    }
	
	// Update is called once per frame
	void Update ()
    {
        BallDirection();
        if (addForce)
        {
            ShootBall();
        }
        SlowDownBall();
    }

    void FixedUpdate()
    {
        //ShootBall();
        //SlowDownBall();
    }
    //decides the direction of ball and the distance 
    private void BallDirection()
    {     
        //when mouse is clicked
        if (Input.GetMouseButtonDown(0))
        {
            //get the mouse position
            Vector2 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            lineRenderer.enabled = true;//activates the line renderer
            lineRenderer.SetPosition(0, transform.position);//the 0 position element is set to ball pos
            lineRenderer.SetPosition(1, mouseWorldPosition);//the 1 position element is set to mouse pos
        }
        //when mouse is moving
        if (Input.GetMouseButton(0))
        {
            //we check of line renderer is active
            if (lineRenderer.enabled == true)
            {
                //we keep updating the mouse position
                Vector2 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                lineRenderer.enabled = true;
                lineRenderer.SetPosition(0, transform.position);
                lineRenderer.SetPosition(1, mouseWorldPosition);
            }
        }
        //when mouse click is removed
        if (Input.GetMouseButtonUp(0))
        {
            //line renderer is set to deactive
            lineRenderer.enabled = false;
            //get the mouse position
            Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouseWorldPosition.z = transform.position.z;//changes the z value to ball z value
            //normalise the difference betweeb ball pos and mouse pos
            Vector2 diff = (mouseWorldPosition - transform.position).normalized;
            //get the disatnce between 2 points
            distance = Vector3.Distance(mouseWorldPosition , transform.position);
            direction = diff;//set the direction
            //some rotation settings for the ball
            float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, rot_z);
            //addforece is set to true
            addForce = true;
        }
    }
    //methode which make the ball move
    void ShootBall()
    {
        //moves the ball in the required direction
        mybody.velocity = (-direction) * speed * distance;
        addForce = false;//set the addforce value to false as we want to add forece for only 1 frame
    }
    //methode which slow downs the ball
    void SlowDownBall()
    {
        //we stop smoothly Player
        if (mybody.velocity.x > 0)
        {
            mybody.velocity -= new Vector2(0.01f, 0);
        }
        if (mybody.velocity.y > 0)
        {
            mybody.velocity -= new Vector2(0, 0.01f);
        }
        if (mybody.velocity.x < 0)
        {
            mybody.velocity += new Vector2(0.01f, 0);
        }
        if (mybody.velocity.y < 0)
        {
            mybody.velocity += new Vector2(0, 0.01f);
        }
        if (mybody.angularVelocity > 0)
        {
            mybody.angularVelocity -= 2;
        }
        if (mybody.angularVelocity < 0)
        {
            mybody.angularVelocity += 2;
        }
    }
    //detects the collider with specific tags
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Colors"))
        {
            //get the collided object color
            Color objCol =  other.GetComponent<SpriteRenderer>().color;
            //spawns the splatter
            Splatter splatterObj = (Splatter)Instantiate(splatter, other.transform.position, Quaternion.identity);
            splatterObj.GetComponent<Splatter>().splatColor = objCol;//set the splatter color
            splatterObj.GetComponent<Splatter>().randomColor = false;//make random false as we want the splatter color to the color we assigned
            splatterObj.GetComponent<Splatter>().ApplyStyle();//then apply the style
        }
    }

}
