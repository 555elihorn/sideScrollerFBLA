using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Threading;

public class DialgoueSystem : MonoBehaviour
{

    bool playerIsTouching = false;

    public TextMeshProUGUI textDisplay;
    public string[] sentences;
    private int index;

    [SerializeField] float typingSpeed = 0.02f;
    public GameObject continueButton;



    void Start()
    {
        continueButton.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        //continueButton.SetActive(true);
    }

    IEnumerator Type()
    {
        string currentString = sentences[index];
        print(currentString);

        foreach (char letter in currentString)
        {
            textDisplay.text += letter;
            yield return new WaitForSeconds(typingSpeed); //Typing speed on text

        }

    }

    public void NextSentence()
    {
        continueButton.SetActive(true);

        if (index < sentences.Length - 1)
        {
            index++;
            textDisplay.text = "";
            StartCoroutine(Type());
        }
        else
        {
            textDisplay.text = "";
            continueButton.SetActive(false);
        }
    }

 
    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.gameObject.name == "Player")
        {
            continueButton.SetActive(true);
            StartCoroutine(Type());
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            print("FOUND");
            continueButton.SetActive(false);
            textDisplay.text = "";
            index = 0;
            StopCoroutine(Type());
        }
    }
}
