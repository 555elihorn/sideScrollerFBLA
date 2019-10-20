﻿using System.Collections;
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
    

    private void Start()
    {
        material = GetComponent<Renderer>().material;
        GetComponent<Renderer>().material.color = new Color(material.color.r, material.color.g, material.color.b, 0); ;
    }

    private void Update()
    {
        var color = material.color;
        if (gameObjectFadeIn)
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

    public void FadeIn()
    {
        gameObjectFadeIn = true;
    }

    public void FadeOut()
    {
        gameObjectFadeOut = true;
    }

}