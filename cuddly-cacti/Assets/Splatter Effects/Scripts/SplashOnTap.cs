using UnityEngine;
using System.Collections;

public class SplashOnTap : MonoBehaviour {

    public Splatter splatter;
    [SerializeField]
    private LayerMask splatterLayer;//layermask to identify specific objects

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        Tap();

    }
    //methode which create splatter on tap
    void Tap()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //get the mouse click position
            Ray hitRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            //check for the physics on it and only do it for objects on splatterLayer
            RaycastHit2D hit = Physics2D.GetRayIntersection(hitRay, Mathf.Infinity, splatterLayer);
            //if not hitting collider returns
            if (hit.collider == null)
                return;
            //if yes creates the splatter
            if (hit.collider != null)
            {
                Instantiate(splatter, hit.point, Quaternion.identity);
            }
        }
    }
}
