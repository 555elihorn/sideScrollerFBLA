using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Slides : MonoBehaviour
{

    //config
    [SerializeField] int currentslide = 1;
    [SerializeField] Sprite[] slides;
    [SerializeField] TextMeshProUGUI slideCounterText = null;

    // Start is called before the first frame update
    void Start()
    {
        slideCounterText.text = currentslide + " / " + slides.Length;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
