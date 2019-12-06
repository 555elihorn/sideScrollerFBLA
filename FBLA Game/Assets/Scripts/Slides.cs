using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Slides : MonoBehaviour
{

    //variables
    int currentSlide = 0;
    

    //config
    [SerializeField] Sprite[] slides;
    [SerializeField] GameObject display;
    [SerializeField] TextMeshProUGUI slideCounterText = null;

    // Start is called before the first frame update
    void Start()
    {
        slideCounterText.text = (currentSlide + 1) + " / " + slides.Length;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //forwards the slides of the mechanics tab
    public void ForwardSlides()
    {
        

        if (currentSlide != slides.Length - 1)
        {
            print("Forwarding!");
            currentSlide++;
            slideCounterText.text = (currentSlide + 1) + " / " + slides.Length;

            var img = display.GetComponent<RawImage>();
            img.texture = textureFromSprite(slides[currentSlide]);

            return;
        }
    }

    //reverses the slide in the mechanics tab
    public void ReverseSlides()
    {
        if(currentSlide != 0)
        {
            print("REVERSING");
            currentSlide--;
            slideCounterText.text = (currentSlide + 1) + " / " + slides.Length;

            var img = display.GetComponent<RawImage>();
            img.texture = textureFromSprite(slides[currentSlide]);

            return;
        }
    }

    //helper method that converts sprite to texture
    public static Texture2D textureFromSprite(Sprite sprite)
    {
        if (sprite.rect.width != sprite.texture.width)
        {
            Texture2D newText = new Texture2D((int)sprite.rect.width, (int)sprite.rect.height);
            Color[] newColors = sprite.texture.GetPixels((int)sprite.textureRect.x, (int)sprite.textureRect.y, (int)sprite.textureRect.width, (int)sprite.textureRect.height);
            newText.SetPixels(newColors);
            newText.Apply();
            return newText;
        }
        else
            return sprite.texture;
    }
}
