using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Threading;
using UnityEngine.SceneManagement;
using System;

public class DialogueSystem : MonoBehaviour
{

    //variables
    private int index;
    bool playerWithinDistance = false;
    bool conversationHasStarted = false;
    bool firstCollision = true;
    bool hasAlreadyBeenConvinced = false;
    bool eButtonEnabled = true;
    Vector2 Pos;
    Vector3 dialogueBoxLocalScaleOriginal;
    Vector3 dialogueTextLocalScaleOriginal;
    Vector3 continueButtonLocalScaleOriginal;
    Vector3 dialogueTextRectTransformOriginal;
    Vector3 continueButtonRectTransformOriginal;

    //cache
    public TextMeshProUGUI textDisplay;
    public string[] sentences;
    public string[] postPersuasionSentences;
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

    //Called on the first frame
    void Start()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
        fader = keyIndicator.GetComponent<ObjectFader>();
        myGameSession = FindObjectOfType<GameSession>();
        SetDialogueBoxActive(false);

        float temp = dialogueText.GetComponent<RectTransform>().anchoredPosition.x;

        dialogueBoxLocalScaleOriginal = dialogueBox.transform.localScale;
        dialogueTextLocalScaleOriginal = dialogueText.transform.localScale;
        continueButtonLocalScaleOriginal = continueButton.transform.localScale;
        dialogueTextRectTransformOriginal = dialogueText.GetComponent<RectTransform>().localPosition;
        continueButtonRectTransformOriginal = continueButton.GetComponent<RectTransform>().localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if(playerWithinDistance && firstCollision)
        {
            DialogueButtonListener();

        }
    }

    //Corutine that handles dialogue
    IEnumerator Dialogue()
    {
        string currentString = null;
        eButtonEnabled = false;

        if (FindObjectOfType<GameSession>().GetPersuadedNPCList().Contains(transform.position.x.ToString()) == true)
        {
            SetPersuaded();
        }

        if (hasAlreadyBeenConvinced)
        {
            currentString = postPersuasionSentences[index];
        }
        else
        {
            currentString = sentences[index];
        }
        
        if(transform.localScale.x == 1f)
        {
            if (currentString.Contains("Jason"))
            {
               var dialogueBoxLocalScale = dialogueBox.transform.localScale;
                var dialogueTextLocalScale = dialogueText.transform.localScale;
                var continueButtonLocalScale = continueButton.transform.localScale;

                var dialogueTextRectTransform = dialogueText.GetComponent<RectTransform>();
                var continueButtonRectTransform = continueButton.GetComponent<RectTransform>();

                //dialogue box
                dialogueBox.transform.localScale = new Vector3((dialogueBoxLocalScale.x * -1), dialogueBoxLocalScale.y, dialogueBoxLocalScale.z);

                //dialoguetext
                dialogueText.transform.localScale = new Vector3((dialogueTextLocalScale.x * -1), dialogueTextLocalScale.y, dialogueTextLocalScale.z);
                dialogueText.GetComponent<RectTransform>().localPosition =
                    new Vector3((dialogueTextRectTransform.anchoredPosition.x * -1),
                    dialogueTextRectTransform.anchoredPosition.y);

                //dialogue button
                continueButton.transform.localScale = new Vector3((continueButtonLocalScale.x * -1), continueButtonLocalScale.y, continueButtonLocalScale.z);
                continueButton.GetComponent<RectTransform>().localPosition =
                    new Vector3((continueButtonRectTransform.anchoredPosition.x * -1),
                    continueButtonRectTransform.anchoredPosition.y);

                Pos = Camera.main.WorldToScreenPoint(player.GetComponent<Rigidbody2D>().position);
                Pos.y += dialgoueBoxOffsetY;
                Pos.x -= 50;

                dialogueBox.transform.position = Pos;

            }
            else if (dialogueBox.transform.localScale.x < 0)
            {
                var dialogueBoxLocalScale = dialogueBox.transform.localScale;
                var dialogueTextLocalScale = dialogueText.transform.localScale;
                var continueButtonLocalScale = continueButton.transform.localScale;

                var dialogueTextRectTransform = dialogueText.GetComponent<RectTransform>();
                var continueButtonRectTransform = continueButton.GetComponent<RectTransform>();

                dialogueBox.transform.localScale = new Vector3((dialogueBoxLocalScale.x * -1), dialogueBoxLocalScale.y, dialogueBoxLocalScale.z);

                dialogueText.transform.localScale = new Vector3((dialogueTextLocalScale.x * -1), dialogueTextLocalScale.y, dialogueTextLocalScale.z);
                dialogueText.GetComponent<RectTransform>().localPosition =
                    new Vector3((dialogueTextRectTransform.anchoredPosition.x * -1),
                    dialogueTextRectTransform.anchoredPosition.y);

                continueButton.transform.localScale = new Vector3((continueButtonLocalScale.x * -1), continueButtonLocalScale.y, continueButtonLocalScale.z);
                continueButton.GetComponent<RectTransform>().localPosition =
                    new Vector3((continueButtonRectTransform.anchoredPosition.x * -1),
                    continueButtonRectTransform.anchoredPosition.y);

                Pos = Camera.main.WorldToScreenPoint(myRigidBody.position);
                Pos.y += dialgoueBoxOffsetY;
                Pos.x += dialogueBoxOffsetX;

                dialogueBox.transform.position = Pos;
            }
        }
        else if(transform.localScale.x != 1f)
        {
            if (currentString.Contains("Jason") && index != 0) //if player start of conversation
            {
                var dialogueBoxLocalScale = dialogueBox.transform.localScale;
                var dialogueTextLocalScale = dialogueText.transform.localScale;
                var continueButtonLocalScale = continueButton.transform.localScale;

                var dialogueTextRectTransform = dialogueText.GetComponent<RectTransform>();
                var continueButtonRectTransform = continueButton.GetComponent<RectTransform>();

                //dialogue box
                dialogueBox.transform.localScale = new Vector3(Mathf.Abs(dialogueBoxLocalScale.x), dialogueBoxLocalScale.y, dialogueBoxLocalScale.z);

                //dialoguetext
                dialogueText.transform.localScale = new Vector3((dialogueTextLocalScale.x * -1), dialogueTextLocalScale.y, dialogueTextLocalScale.z);
                dialogueText.GetComponent<RectTransform>().localPosition =
                    new Vector3((dialogueTextRectTransform.anchoredPosition.x * -1),
                    dialogueTextRectTransform.anchoredPosition.y);

                //dialogue button
                continueButton.transform.localScale = new Vector3((continueButtonLocalScale.x * -1), continueButtonLocalScale.y, continueButtonLocalScale.z);
                continueButton.GetComponent<RectTransform>().localPosition =
                    new Vector3((continueButtonRectTransform.anchoredPosition.x * -1),
                    continueButtonRectTransform.anchoredPosition.y);

                Pos = Camera.main.WorldToScreenPoint(player.GetComponent<Rigidbody2D>().position);
                Pos.y += dialgoueBoxOffsetY;
                Pos.x += dialogueBoxOffsetX;

                dialogueBox.transform.position = Pos;
            }
            else if (currentString.Contains("Jason") && index == 0)
            {
                var dialogueBoxLocalScale = dialogueBox.transform.localScale;
                var dialogueTextLocalScale = dialogueText.transform.localScale;
                var continueButtonLocalScale = continueButton.transform.localScale;

                var dialogueTextRectTransform = dialogueText.GetComponent<RectTransform>();
                var continueButtonRectTransform = continueButton.GetComponent<RectTransform>();

                //dialogue box
                dialogueBox.transform.localScale = new Vector3(Mathf.Abs(dialogueBoxLocalScale.x), dialogueBoxLocalScale.y, dialogueBoxLocalScale.z);

                //dialoguetext
                dialogueText.transform.localScale = new Vector3((dialogueTextLocalScale.x), dialogueTextLocalScale.y, dialogueTextLocalScale.z);
                dialogueText.GetComponent<RectTransform>().localPosition =
                    new Vector3((dialogueTextRectTransform.anchoredPosition.x),
                    dialogueTextRectTransform.anchoredPosition.y);

                //dialogue button
                continueButton.transform.localScale = new Vector3((continueButtonLocalScale.x), continueButtonLocalScale.y, continueButtonLocalScale.z);
                continueButton.GetComponent<RectTransform>().localPosition =
                    new Vector3((continueButtonRectTransform.anchoredPosition.x),
                    continueButtonRectTransform.anchoredPosition.y);

                Pos = Camera.main.WorldToScreenPoint(player.GetComponent<Rigidbody2D>().position);
                Pos.y += dialgoueBoxOffsetY;
                Pos.x += dialogueBoxOffsetX;

                dialogueBox.transform.position = Pos;
            }
            else if (dialogueBox.transform.localScale.x > 0)
            {
                var dialogueBoxLocalScale = dialogueBox.transform.localScale;
                var dialogueTextLocalScale = dialogueText.transform.localScale;
                var continueButtonLocalScale = continueButton.transform.localScale;

                var dialogueTextRectTransform = dialogueText.GetComponent<RectTransform>();
                var continueButtonRectTransform = continueButton.GetComponent<RectTransform>();

                //dialogue box
                dialogueBox.transform.localScale = new Vector3((dialogueBoxLocalScale.x * -1), dialogueBoxLocalScale.y, dialogueBoxLocalScale.z);

                //dialoguetext
                dialogueText.transform.localScale = new Vector3((dialogueTextLocalScale.x * -1), dialogueTextLocalScale.y, dialogueTextLocalScale.z);
                dialogueText.GetComponent<RectTransform>().localPosition =
                    new Vector3((dialogueTextRectTransform.anchoredPosition.x * -1),
                    dialogueTextRectTransform.anchoredPosition.y);

                //dialogue button
                continueButton.transform.localScale = new Vector3((continueButtonLocalScale.x * -1), continueButtonLocalScale.y, continueButtonLocalScale.z);
                continueButton.GetComponent<RectTransform>().localPosition =
                    new Vector3((continueButtonRectTransform.anchoredPosition.x * -1),
                    continueButtonRectTransform.anchoredPosition.y);

                Pos = Camera.main.WorldToScreenPoint(myRigidBody.position);
                Pos.y += dialgoueBoxOffsetY;
                Pos.x -= 50;

                dialogueBox.transform.position = Pos;
            }
        }
        
        foreach (char letter in currentString)
        {
            textDisplay.text += letter;
            yield return new WaitForSeconds(typingSpeed); //Typing speed on text

        }

        eButtonEnabled = true;

    }

    //Reset dialogue box
    private void ResetDialoguePos()
    {
        float textScale = dialogueTextLocalScaleOriginal.y;
        float continueTextScale = dialogueTextLocalScaleOriginal.y;

        var dialogueTextRectTransform = dialogueText.GetComponent<RectTransform>();
        var continueButtonRectTransform = continueButton.GetComponent<RectTransform>();

        dialogueBox.transform.localScale = dialogueBoxLocalScaleOriginal;
        
        dialogueText.transform.localScale = new Vector3(textScale, textScale);
        dialogueText.GetComponent<RectTransform>().localPosition =
                    new Vector3( (Mathf.Abs(dialogueTextRectTransform.anchoredPosition.x) * -1 ),
                    dialogueTextRectTransform.anchoredPosition.y);

        
        continueButton.transform.localScale = new Vector3(continueTextScale, continueTextScale);
        continueButton.GetComponent<RectTransform>().localPosition =
                    new Vector3(( Mathf.Abs(continueButtonRectTransform.anchoredPosition.x)),
                    continueButtonRectTransform.anchoredPosition.y);
    }

    //On exit of collider reset the npc position
    private void OnTriggerExit2D(Collider2D collision)
    {
        playerWithinDistance = false;

        if (collision.gameObject.name == "Player" && collision.ToString().Contains("Capsule"))
        {
            FlipSprite(true);
            EndConversation();
        }
    }

    //On enter of collider have the NPC face the player
    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.gameObject.name == "Player" && collision.ToString().Contains("Capsule"))
        {
            FlipSprite(false);
            keyIndicator.SetActive(true);
            fader.FadeIn();

            playerWithinDistance = true;
        }
    }

    //End dialogue conversation
    private void EndConversation()
    {
        ResetDialoguePos();
        conversationHasStarted = false;
        textDisplay.text = "";
        SetDialogueBoxActive(false);
        index = 0;
        StopCoroutine(Dialogue());
        fader.FadeOut();
    }

    //Start dialogue conversation
    private void StartConversation()
    {
        //sets dialogue box above NPC
        Pos = Camera.main.WorldToScreenPoint(myRigidBody.position);
        Pos.y += dialgoueBoxOffsetY;
        Pos.x += dialogueBoxOffsetX;

        SetDialogueBoxActive(true);
        dialogueBox.transform.position = Pos; 
        textDisplay.text = "";

        print("Startconversation: " + hasAlreadyBeenConvinced);

        fader.FadeOut();

        StartCoroutine(Dialogue());
    }

    //Listen for the player to press E so that they can start / continue conversation
    private void DialogueButtonListener()
    {
        if (Input.GetKeyDown(KeyCode.E) && !conversationHasStarted && eButtonEnabled)
        {
            conversationHasStarted = true;
            StartConversation();
        }
        else if (Input.GetKeyDown(KeyCode.E) && eButtonEnabled)
        {
            NextSentence();
        }
    }

    //Play the next sentence
    public void NextSentence()
    {
        continueButton.SetActive(true);

        if(FindObjectOfType<GameSession>().GetPersuadedNPCList().Contains(transform.position.x.ToString()) == true) 
        {
            SetPersuaded();
        }

        if (hasAlreadyBeenConvinced == false)
        {
            if (index < sentences.Length - 1)
            {
                index++;
                textDisplay.text = "";
                StartCoroutine(Dialogue());
            }
            else if (hasMiniGame)
            {
                //if at the end of the conversation, start the mini game
                myGameSession.SetPreviousScene(SceneManager.GetActiveScene().buildIndex);

                FindObjectOfType<PlayerState>().RecordPlayerPosition();
                FindObjectOfType<ScenePersist>().GetScenePersistChildren();
                print("hasMiniGame:" + hasAlreadyBeenConvinced);

                //If the persuadedNPCList does not have this NPC, add them
                if (FindObjectOfType<GameSession>().GetPersuadedNPCList().Contains(transform.position.x.ToString()) != true)
                {
                    FindObjectOfType<GameSession>().AddPersuadedNPC(transform.position.x.ToString());
                }

               
                SceneManager.LoadScene("Persuasion_Mini_Game");
                textDisplay.text = "";
                continueButton.SetActive(false);
            }
            else
            {
                SetDialogueBoxActive(false);
            }
        }
        else
        {
            //Use Alternative text
            if (index < postPersuasionSentences.Length - 1)
            {
                index++;
                textDisplay.text = "";
                StartCoroutine(Dialogue());
            }
            else
            {
                SetDialogueBoxActive(false);
            }
        }
        
    }

    //Set the dialogue box active or inactive
    private void SetDialogueBoxActive(bool value)
    {
        dialogueBox.SetActive(value);
        continueButton.SetActive(value);
        dialogueText.SetActive(value);
    }

    //Flip NPC Sprite
    private void FlipSprite(bool reset)
    {
        
        if(reset)
        {
            transform.localScale = new Vector2(1f, 1f);
            keyIndicator.transform.localScale = new Vector2(1f, 1f);
            return;
        }
        
        if (player.transform.localScale.x == transform.localScale.x)
        {
            transform.localScale = new Vector2(transform.localScale.x * -1, 1f);
            keyIndicator.transform.localScale = new Vector2(transform.localScale.x, 1f);
            return;
        }

    }

    //Set the NPC to persuaded
    void SetPersuaded()
    {
        hasAlreadyBeenConvinced = true;
    }
}
