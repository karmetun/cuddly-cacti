using UnityEngine;
using System.Collections;

public class Shooter : MonoBehaviour
{
    //ref to the splatter object
    public Splatter splatter;

    //ref to line renderer component
    private LineRenderer lineRenderer;

    [SerializeField]//layermask for raycast
    private LayerMask splatterLayer;

	private void Awake()
    {
        //get the componenet on object
        lineRenderer = GetComponent<LineRenderer>();
	}

	private void Update ()
    {
        AimAndFire();
    }

    //method which fires
    private void AimAndFire()
    {
        //get the mouse click position
        Vector2 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //sets the line renderer position element 1 to the mouse position
        lineRenderer.SetPosition(1, mouseWorldPosition);
        //when mouse is click
        if (Input.GetMouseButtonDown(0))
        {
            //ray is created in the direction of mouse
            RaycastHit2D raycastHit = Physics2D.Raycast(transform.position, mouseWorldPosition, Mathf.Infinity,splatterLayer);
            //if it hits the splatter object is spawned
            if (raycastHit)
            {
                Instantiate(splatter, raycastHit.point, Quaternion.identity);
            }
        }
    }

}
