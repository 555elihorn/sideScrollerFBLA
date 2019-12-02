using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Slides : MonoBehaviour
{

    //variables
    int currentslide = 0;

    //config
    [SerializeField] Sprite[] slides;
    [SerializeField] TextMeshProUGUI slideCounterText = null;

    // Start is called before the first frame update
    void Start()
    {
        slideCounterText.text = (currentslide + 1) + " / " + slides.Length;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ForwardSlides()
    {
        if(currentslide != slides.Length - 1)
        {
            print("Forwarding!");
            currentslide++;
            slideCounterText.text = (currentslide + 1) + " / " + slides.Length;
            return;
        }
    }

    public void ReverseSlides()
    {
        if(currentslide != 0)
        {
            print("REVERSING");
            currentslide--;
            slideCounterText.text = (currentslide + 1) + " / " + slides.Length;
            return;
        }
    }
}
