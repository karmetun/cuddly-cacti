using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Splatter : MonoBehaviour
{
    public List<Sprite> sprites; //ref to the sprites which will be used by sprites renderer
    [HideInInspector]
    public bool randomColor = true; //set to false when the target gives the color
    [HideInInspector]
    public Color32 splatColor; //color values which can be assigned by another script
    private SpriteRenderer spriteRenderer;//ref to sprite renderer component

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        //at start we randomly select the sprites
        spriteRenderer.sprite = sprites[Random.Range(0, sprites.Count)];
        //checks if randomColor is true and then randomly apply the colors
        if (randomColor)
        {
            ApplyStyle();
        }
    }
    //this methode assign the color to the splatter
    public void ApplyStyle()
    {
        if (randomColor == true)
        {
            spriteRenderer.color = new Color(Random.Range(0, 1f), Random.Range(0, 1f), Random.Range(0, 1f));
        }
        else
        {//when the other script has power to assign the color , this code is used
            spriteRenderer.color = splatColor;
        }
        transform.rotation = Quaternion.Euler(0, 0, Random.Range(0, 360));
    }
}
