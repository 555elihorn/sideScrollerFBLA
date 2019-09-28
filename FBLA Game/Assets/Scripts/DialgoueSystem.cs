using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Threading;

public class DialgoueSystem : MonoBehaviour
{

    //variables
    bool playerIsTouching = false;
    private int index;
    bool playerWithinDistance = false;
    bool conversationHasStarted = false;

    //cache
    public TextMeshProUGUI textDisplay;
    public string[] sentences;
    

    //configs
    [SerializeField] float typingSpeed = 0.02f;
    [SerializeField] GameObject keyIndicator;
    [SerializeField] GameObject continueButton;
    [SerializeField] GameObject DialogueText;



    void Start()
    {
        continueButton.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
        if(playerWithinDistance)
        {
            dialogueButtonListener();
        }
        //continueButton.SetActive(true);
    }

    IEnumerator Type()
    {
        string currentString = sentences[index];

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

    private void dialogueButtonListener()
    {
        if (Input.GetKeyDown(KeyCode.E) && !conversationHasStarted)
        {
            conversationHasStarted = true;

            print("ENTER");
            textDisplay.text = "";
            continueButton.SetActive(true);
            DialogueText.SetActive(true);
            StartCoroutine(Type());
        }
        else if(Input.GetKeyDown(KeyCode.E))
        {
            NextSentence();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player" && collision.ToString().Contains("Capsule"))
        {
            playerWithinDistance = true;
        }
        /*
        print(collision.ToString());
        
            
        }
        */

    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        playerWithinDistance = false;

        if (collision.gameObject.name == "Player" && collision.ToString().Contains("Capsule"))
        {
            print("EXIT");
            textDisplay.text = "";
            continueButton.SetActive(false);
            DialogueText.SetActive(false);
            index = 0;
            StopCoroutine(Type());
        }
    }
}
