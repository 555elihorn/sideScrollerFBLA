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
    Vector2 Pos;

    //cache
    public TextMeshProUGUI textDisplay;
    public string[] sentences;
    public string[] sample;
    ObjectFader fader;
    GameSession myGameSession;
    Rigidbody2D myRigidBody;

    //configs
    [SerializeField] int dialgoueBoxOffsetY;
    [SerializeField] int dialogueBoxOffsetX;
    [SerializeField] float typingSpeed = 0.02f;
    [SerializeField] bool hasMiniGame;
    [SerializeField] GameObject keyIndicator;
    [SerializeField] GameObject continueButton;
    [SerializeField] GameObject dialogueText;
    [SerializeField] GameObject dialogueBox;
    [SerializeField] GameObject player;


    void Start()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
        fader = keyIndicator.GetComponent<ObjectFader>();
        myGameSession = FindObjectOfType<GameSession>();
        SetDialogueBox(false);

        float temp = dialogueText.GetComponent<RectTransform>().anchoredPosition.x;


        print(temp);
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

        if(currentString.Contains("Player"))
        {
            float temp = dialogueText.GetComponent<RectTransform>().anchoredPosition.x;


            print("first check: " + temp);

            //dialogue box
            dialogueBox.transform.localScale = new Vector3((dialogueBox.transform.localScale.x * -1), dialogueBox.transform.localScale.y, dialogueBox.transform.localScale.z);

            //dialoguetext
            dialogueText.transform.localScale = new Vector3((dialogueText.transform.localScale.x * -1), dialogueText.transform.localScale.y, dialogueText.transform.localScale.z);
            var tempTransform = dialogueText.GetComponent<RectTransform>();
            dialogueText.GetComponent<RectTransform>().localPosition = 
                new Vector3((temp * -1),
                dialogueText.GetComponent<RectTransform>().anchoredPosition.y);



        }
        else if(dialogueBox.transform.localScale.x < 0)
        {
            float temp = dialogueText.GetComponent<RectTransform>().anchoredPosition.x;

            dialogueBox.transform.localScale = new Vector3((dialogueBox.transform.localScale.x * -1), dialogueBox.transform.localScale.y, dialogueBox.transform.localScale.z);
            dialogueText.transform.localScale = new Vector3((dialogueText.transform.localScale.x * -1), dialogueText.transform.localScale.y, dialogueText.transform.localScale.z);

            dialogueText.GetComponent<RectTransform>().localPosition =
                new Vector3((temp * -1),
                dialogueText.GetComponent<RectTransform>().anchoredPosition.y);

        }

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
        
        conversationHasStarted = false;
        textDisplay.text = "";
        SetDialogueBox(false);
        index = 0;
        StopCoroutine(Type());
    }

    private void StartConversation()
    {
        //sets dialogue box above NPC
        Pos = Camera.main.WorldToScreenPoint(myRigidBody.position);
        Pos.y += dialgoueBoxOffsetY;
        Pos.x += dialogueBoxOffsetX;

        SetDialogueBox(true);
        dialogueBox.transform.position = Pos; 
        textDisplay.text = "";

        fader.FadeOut();

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
        else if(hasMiniGame)
        {
            //if at the end of the conversation, start the mini game
            myGameSession.SetPreviousScene(SceneManager.GetActiveScene().buildIndex);
            SceneManager.LoadScene(2);
            textDisplay.text = "";
            continueButton.SetActive(false);
        }
        else
        {
            SetDialogueBox(false);
        }
    }

    private void SetDialogueBox(bool value)
    {
        dialogueBox.SetActive(value);
        continueButton.SetActive(value);
        dialogueText.SetActive(value);
    }
}
