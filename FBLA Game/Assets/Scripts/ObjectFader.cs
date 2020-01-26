using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectFader : MonoBehaviour
{


    // Update is called once per frame
    [SerializeField] private float fadeOutPerSecond = 2.5f;
    [SerializeField] private float fadeInPerSecond = 0.1f;
    bool gameObjectFadeIn = false;
    bool gameObjectFadeOut = false;
    Material material;
    float transparency;

    // Start is called before the first frame update
    private void Start()
    {
        material = GetComponent<Renderer>().material;
        GetComponent<Renderer>().material.color = new Color(material.color.r, material.color.g, material.color.b, 0); ;
    }

    // Update is called once per frame
    private void Update()
    {
        var color = material.color;

        if(gameObjectFadeIn == true && gameObjectFadeOut == true)
        {
            print("CHECK!");
            gameObjectFadeOut = false;

            if (color.a <= 1)
            {
                material.color = new Color(color.r, color.g, color.b, color.a + (fadeInPerSecond * Time.deltaTime));
                color = material.color;
            }
            else
            {
                gameObjectFadeIn = false;
            }

        }
        else if (gameObjectFadeIn)
        {
            if (color.a <= 1)
            {
                material.color = new Color(color.r, color.g, color.b, color.a + (fadeInPerSecond * Time.deltaTime));
                color = material.color;
            }
            else
            {
                gameObjectFadeIn = false;
            }
        }
        else if(gameObjectFadeOut)
        {
            if(color.a >= 0)
            {
                material.color = new Color(color.r, color.g, color.b, color.a - (fadeOutPerSecond * Time.deltaTime));
                color = material.color;
            }
            else
            {
                gameObjectFadeOut = false;
            }
        }
        
    }

    //fades in e suggestion button
    public void FadeIn()
    {
        gameObjectFadeIn = true;
        gameObjectFadeOut = false;
    }

    //fades out e suggestion button
    public void FadeOut()
    {
        gameObjectFadeIn = false;
        gameObjectFadeOut = true;
    }

}
