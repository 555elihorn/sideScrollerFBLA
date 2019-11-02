using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Threading;
using UnityEngine.SceneManagement;

public class DialogueSystem : MonoBehaviour
{

    //variables
    private int index;
    bool playerWithinDistance = false;
    bool conversationHasStarted = false;
    bool firstCollision = true;

    //cache
    public TextMeshProUGUI textDisplay;
    public string[] sentences;
    ObjectFader fader;

    //configs
    [SerializeField] float typingSpeed = 0.02f;
    [SerializeField] GameObject keyIndicator;
    [SerializeField] GameObject continueButton;
    [SerializeField] GameObject DialogueText;

    void Start()
    {

        fader = keyIndicator.GetComponent<ObjectFader>();
        continueButton.SetActive(false);
        //keyIndicator.SetActive(false);
        
    }

    // Update is called once per frame
    void Update()
    {
        
        if(playerWithinDistance && firstCollision)
        {
            DialogueButtonListener();

        }
        
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

    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player" && collision.ToString().Contains("Capsule"))
        {
            keyIndicator.SetActive(true);
            fader.FadeIn();

            playerWithinDistance = true;
        }
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        playerWithinDistance = false;
        
        if (collision.gameObject.name == "Player" && collision.ToString().Contains("Capsule"))
        {
            EndConversation();
        }
    }

    private void EndConversation()
    {
        fader.FadeOut();
        
        //keyIndicator.SetActive(false);
        conversationHasStarted = false;
        textDisplay.text = "";
        continueButton.SetActive(false);
        DialogueText.SetActive(false);
        index = 0;
        StopCoroutine(Type());
    }

    private void StartConversation()
    {
        textDisplay.text = "";
        continueButton.SetActive(true);
        DialogueText.SetActive(true);
        StartCoroutine(Type());
    }

    private void DialogueButtonListener()
    {
        

        if (Input.GetKeyDown(KeyCode.E) && !conversationHasStarted)
        {
            conversationHasStarted = true;
            StartConversation();
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            NextSentence();
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
            //IF at the end of the conversation, start the mini game
            SceneManager.LoadScene(2);
            textDisplay.text = "";
            continueButton.SetActive(false);
        }
    }
}
