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
    
    bool hasAlreadyBeenConvinced = false;
    bool eButtonEnabled = true;

    Vector2 Pos;
    Vector3 dialogueBoxLocalScaleOriginal;
    Vector3 dialogueTextLocalScaleOriginal;
    Vector3 continueTextLocalScaleOriginal;
    Vector3 dialogueTextRectTransformOriginal;
    Vector3 continueRectTransformOriginal;

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
    [SerializeField] GameObject continueText;
    [SerializeField] GameObject dialogueText;
    [SerializeField] GameObject dialogueBox;
    [SerializeField] GameObject player;

    //Called on the first frame
    void Start()
    {
        //gets component of the NPC game object
        myRigidBody = GetComponent<Rigidbody2D>();
        fader = keyIndicator.GetComponent<ObjectFader>();
        myGameSession = FindObjectOfType<GameSession>();
        SetDialogueBoxActive(false);

        //Records original transform of dialogue box
        dialogueBoxLocalScaleOriginal = dialogueBox.transform.localScale;
        dialogueTextLocalScaleOriginal = dialogueText.transform.localScale;
        dialogueTextRectTransformOriginal = dialogueText.GetComponent<RectTransform>().localPosition;
        continueTextLocalScaleOriginal = continueText.transform.localScale;
        continueRectTransformOriginal = continueText.GetComponent<RectTransform>().localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if(playerWithinDistance) //if the player is within the conversation collider...
        {
            if (player.transform.position.x < myRigidBody.position.x && transform.localScale.x >= 0) //if the player is behind the NPC
            {
                FlipSprite(false, true); //flip the sprite to face the player
            }
            else if(player.transform.position.x > myRigidBody.position.x) //if the player is in front of NPC
            {
                FlipSprite(true, false); //face the player
            }
            else
            {
                //do nothing
            }
        }
       
        if(playerWithinDistance) //if the player is within conversation distance listen for them to press the E key
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
            //after persausion game
            SetPersuaded();
        }

        if (hasAlreadyBeenConvinced)
        {
            //Dialogue text after persuasion mini game
            currentString = postPersuasionSentences[index];
        }
        else
        {
            //normal dialogue
            currentString = sentences[index];
        }
        
        if(transform.localScale.x == 1f)
        {
            if (currentString.Contains("Jason"))
            {
                /*If the next conversation string involves the player,
                 find their position and assign the dialogue box over the player position
                     
                */


                //scales of individual game objects
                var dialogueBoxLocalScale = dialogueBox.transform.localScale;
                var dialogueTextLocalScale = dialogueText.transform.localScale;
                var continueTextLocalScale = continueText.transform.localScale;

                //transforms of game objects
                var dialogueTextRectTransform = dialogueText.GetComponent<RectTransform>();
                var continueTextRectTransform = continueText.GetComponent<RectTransform>();

                //dialogue box
                dialogueBox.transform.localScale = new Vector3((dialogueBoxLocalScale.x * -1), dialogueBoxLocalScale.y, dialogueBoxLocalScale.z);

                //Sets dialoguetext position
                dialogueText.transform.localScale = new Vector3((dialogueTextLocalScale.x * -1), dialogueTextLocalScale.y, dialogueTextLocalScale.z);
                dialogueText.GetComponent<RectTransform>().localPosition =
                    new Vector3((dialogueTextRectTransform.anchoredPosition.x * -1),
                    dialogueTextRectTransform.anchoredPosition.y);

                //Sets dialogue button position
                continueText.transform.localScale = new Vector3((continueTextLocalScale.x * -1), continueTextLocalScale.y, continueTextLocalScale.z);
                continueText.GetComponent<RectTransform>().localPosition =
                    new Vector3((continueTextRectTransform.anchoredPosition.x * -1),
                    continueTextRectTransform.anchoredPosition.y);

                // new position of dialogue box
                Pos = Camera.main.WorldToScreenPoint(player.GetComponent<Rigidbody2D>().position);
                Pos.y += dialgoueBoxOffsetY;
                Pos.x -= 50;

                //sets new dialogue position in conversation
                dialogueBox.transform.position = Pos;

            } //clean
            else if (dialogueBox.transform.localScale.x < 0)
            {
                var dialogueBoxLocalScale = dialogueBox.transform.localScale;
                var dialogueTextLocalScale = dialogueText.transform.localScale;
                var continueTextLocalScale = continueText.transform.localScale;

                var dialogueTextRectTransform = dialogueText.GetComponent<RectTransform>();
                var continueTextRectTransform = continueText.GetComponent<RectTransform>();

                dialogueBox.transform.localScale = new Vector3((dialogueBoxLocalScale.x * -1), dialogueBoxLocalScale.y, dialogueBoxLocalScale.z);

                dialogueText.transform.localScale = new Vector3((dialogueTextLocalScale.x * -1), dialogueTextLocalScale.y, dialogueTextLocalScale.z);
                dialogueText.GetComponent<RectTransform>().localPosition =
                    new Vector3((dialogueTextRectTransform.anchoredPosition.x * -1),
                    dialogueTextRectTransform.anchoredPosition.y);

                continueText.transform.localScale = new Vector3((continueTextLocalScale.x * -1), continueTextLocalScale.y, continueTextLocalScale.z);
                continueText.GetComponent<RectTransform>().localPosition =
                    new Vector3((continueTextRectTransform.anchoredPosition.x * -1),
                    continueTextRectTransform.anchoredPosition.y);

                Pos = Camera.main.WorldToScreenPoint(myRigidBody.position);
                Pos.y += dialgoueBoxOffsetY;
                Pos.x += dialogueBoxOffsetX;

                dialogueBox.transform.position = Pos;
            } //clean
        }
        else if(transform.localScale.x != 1f)
        {
            if (currentString.Contains("Jason") && index != 0) //if player start of conversation
            {
                var dialogueBoxLocalScale = dialogueBox.transform.localScale;
                var dialogueTextLocalScale = dialogueText.transform.localScale;
                var continueTextLocalScale = continueText.transform.localScale;

                var dialogueTextRectTransform = dialogueText.GetComponent<RectTransform>();
                var continueTextRectTransform = continueText.GetComponent<RectTransform>();

                //dialogue box
                dialogueBox.transform.localScale = new Vector3(Mathf.Abs(dialogueBoxLocalScale.x), dialogueBoxLocalScale.y, dialogueBoxLocalScale.z);

                //dialoguetext
                dialogueText.transform.localScale = new Vector3((dialogueTextLocalScale.x * -1), dialogueTextLocalScale.y, dialogueTextLocalScale.z);
                dialogueText.GetComponent<RectTransform>().localPosition =
                    new Vector3((dialogueTextRectTransform.anchoredPosition.x * -1),
                    dialogueTextRectTransform.anchoredPosition.y);

                //dialogue button
                continueText.transform.localScale = new Vector3((continueTextLocalScale.x * -1), continueTextLocalScale.y, continueTextLocalScale.z);
                continueText.GetComponent<RectTransform>().localPosition =
                    new Vector3((continueTextRectTransform.anchoredPosition.x * -1),
                    continueTextRectTransform.anchoredPosition.y);

                Pos = Camera.main.WorldToScreenPoint(player.GetComponent<Rigidbody2D>().position);
                Pos.y += dialgoueBoxOffsetY;
                Pos.x += dialogueBoxOffsetX;

                dialogueBox.transform.position = Pos;
            } //clean
            else if (currentString.Contains("Jason") && index == 0) // clean
            {
                var dialogueBoxLocalScale = dialogueBox.transform.localScale;
                var dialogueTextLocalScale = dialogueText.transform.localScale;
                var continueTextLocalScale = continueText.transform.localScale;

                var dialogueTextRectTransform = dialogueText.GetComponent<RectTransform>();
                var continueTextRectTransform = continueText.GetComponent<RectTransform>();

                //dialogue box
                dialogueBox.transform.localScale = new Vector3(Mathf.Abs(dialogueBoxLocalScale.x), dialogueBoxLocalScale.y, dialogueBoxLocalScale.z);

                //dialoguetext
                dialogueText.transform.localScale = new Vector3((dialogueTextLocalScale.x), dialogueTextLocalScale.y, dialogueTextLocalScale.z);
                dialogueText.GetComponent<RectTransform>().localPosition =
                    new Vector3((dialogueTextRectTransform.anchoredPosition.x),
                    dialogueTextRectTransform.anchoredPosition.y);

                //dialogue button
                continueText.transform.localScale = new Vector3((continueTextLocalScale.x), continueTextLocalScale.y, continueTextLocalScale.z);
                continueText.GetComponent<RectTransform>().localPosition =
                    new Vector3((continueTextRectTransform.anchoredPosition.x),
                    continueTextRectTransform.anchoredPosition.y);

                Pos = Camera.main.WorldToScreenPoint(player.GetComponent<Rigidbody2D>().position);
                Pos.y += dialgoueBoxOffsetY;
                Pos.x += dialogueBoxOffsetX;

                dialogueBox.transform.position = Pos;
            }
            else if (dialogueBox.transform.localScale.x > 0)
            {
                var dialogueBoxLocalScale = dialogueBox.transform.localScale;
                var dialogueTextLocalScale = dialogueText.transform.localScale;
                var continueTextLocalScale = continueText.transform.localScale;

                var dialogueTextRectTransform = dialogueText.GetComponent<RectTransform>();
                var continueTextRectTransform = continueText.GetComponent<RectTransform>();

                //dialogue box
                dialogueBox.transform.localScale = new Vector3((dialogueBoxLocalScale.x * -1), dialogueBoxLocalScale.y, dialogueBoxLocalScale.z);

                //dialoguetext
                dialogueText.transform.localScale = new Vector3((dialogueTextLocalScale.x * -1), dialogueTextLocalScale.y, dialogueTextLocalScale.z);
                dialogueText.GetComponent<RectTransform>().localPosition =
                    new Vector3((dialogueTextRectTransform.anchoredPosition.x * -1),
                    dialogueTextRectTransform.anchoredPosition.y);

                //dialogue button
                continueText.transform.localScale = new Vector3((continueTextLocalScale.x * -1), continueTextLocalScale.y, continueTextLocalScale.z);
                continueText.GetComponent<RectTransform>().localPosition =
                    new Vector3((continueTextRectTransform.anchoredPosition.x * -1),
                    continueTextRectTransform.anchoredPosition.y);

                Pos = Camera.main.WorldToScreenPoint(myRigidBody.position);
                Pos.y += dialgoueBoxOffsetY;
                Pos.x -= 50;

                dialogueBox.transform.position = Pos;
            }//clean
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
        float dialogueTextScale = dialogueTextLocalScaleOriginal.y;
        float continueTextScale = continueTextLocalScaleOriginal.y;

        var dialogueTextRectTransform = dialogueText.GetComponent<RectTransform>();
        var continueTextRectTransform = continueText.GetComponent<RectTransform>();

        dialogueBox.transform.localScale = dialogueBoxLocalScaleOriginal;
        
        dialogueText.transform.localScale = new Vector3(dialogueTextScale, dialogueTextScale);
        dialogueText.GetComponent<RectTransform>().localPosition =
                    new Vector3( (Mathf.Abs(dialogueTextRectTransform.anchoredPosition.x) * -1 ),
                    dialogueTextRectTransform.anchoredPosition.y);

        
        continueText.transform.localScale = new Vector3(continueTextScale, continueTextScale);
        continueText.GetComponent<RectTransform>().localPosition =
                    new Vector3(( Mathf.Abs(continueTextRectTransform.anchoredPosition.x)),
                    continueTextRectTransform.anchoredPosition.y);
    }

    //On exit of collider reset the npc position
    private void OnTriggerExit2D(Collider2D collision)
    {
        playerWithinDistance = false;
        var playerPosX = collision.transform.position.x;

        if (collision.gameObject.name == "Player" && collision.ToString().Contains("Capsule"))
        {
            if (playerPosX < myRigidBody.position.x)
            {
                FlipSprite(true, false); //flip to orginal position
            }
            EndConversation();
        }
    }

    //On enter of collider have the NPC face the player
    private void OnTriggerEnter2D(Collider2D collision)
    {
        var playerPosX = collision.transform.position.x;

        if (collision.gameObject.name == "Player" && collision.ToString().Contains("Capsule"))
        {
            if (playerPosX < myRigidBody.position.x)
            {
                FlipSprite(false, false); //flip towards player
            }
            
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

        print("Conversation has ended");
        player.GetComponent<PlayerMovement>().SetMovement(true);

        SetDialogueBoxActive(false);
        index = 0;
        StopCoroutine(Dialogue());

        if(playerWithinDistance)
        {
            fader.FadeIn();
        }
        else
        {
            fader.FadeOut();
        }
        
    }

    //Start dialogue conversation
    private void StartConversation()
    {
        //sets dialogue box above NPC
        Pos = Camera.main.WorldToScreenPoint(myRigidBody.position);
        Pos.y += dialgoueBoxOffsetY;
        Pos.x += dialogueBoxOffsetX;

        //prepare dialogue box
        SetDialogueBoxActive(true);
        dialogueBox.transform.position = Pos; 
        textDisplay.text = "";

        //lock playermovement
        player.GetComponent<PlayerMovement>().SetMovement(false);

        //Fade out E button suggestion
        fader.FadeOut();

        //start dialogue corutine that generates the sentences
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
        continueText.SetActive(true);

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
                continueText.SetActive(false);
            }
            else
            {
                SetDialogueBoxActive(false);
                EndConversation();
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
        continueText.SetActive(value);
        dialogueText.SetActive(value);
    }

    //Flip NPC Sprite
    private void FlipSprite(bool reset, bool exception)
    {
        
        if(reset)
        {
            transform.localScale = new Vector2(1f, 1f);
            keyIndicator.transform.localScale = new Vector2(1f, 1f);
            return;
        }

        if (player.transform.localScale.x == transform.localScale.x || exception)
        {
            print("SHOULD BE DOING THIS");
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
