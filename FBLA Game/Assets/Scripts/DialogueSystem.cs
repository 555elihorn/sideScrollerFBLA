using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Threading;
using UnityEngine.SceneManagement;
using System;
using System.Threading.Tasks;

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
    //Vector3 dialogueTextRectTransformOriginal;
    //Vector3 continueRectTransformOriginal;

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
    [SerializeField] Camera mainCamera;

    //Called on the first frame
    void Start()
    {
        //gets component of the NPC game object
        myRigidBody = GetComponent<Rigidbody2D>();
        fader = keyIndicator.GetComponent<ObjectFader>();
        myGameSession = FindObjectOfType<GameSession>();
        SetDialogueBoxActive(false);

        //Records original transform of dialogue box, continue text, and dialogue text
        dialogueBoxLocalScaleOriginal = dialogueBox.transform.localScale;
        dialogueTextLocalScaleOriginal = dialogueText.transform.localScale;
        continueTextLocalScaleOriginal = continueText.transform.localScale;

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
            //if the player is in the persuaded NPC list set them to persuaded
            SetPersuaded();
        }

        if (hasAlreadyBeenConvinced) 
        {
            //if the NPC has been convinced use alternative text
            currentString = postPersuasionSentences[index];
        }
        else
        {
            //else use normal dialogue
            currentString = sentences[index];
        }
        
        //block deals with the scaling, position, and rect transform of the dialogue box
        if(transform.localScale.x == 1f)
        {
            //if the NPC is facing the original position
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

                //Sets continue text position
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

            } 
            else if (dialogueBox.transform.localScale.x < 0)
            {
                /* 
                 * if the dialougebox tranform is less than 0
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

                //Sets continuetext position
                continueText.transform.localScale = new Vector3((continueTextLocalScale.x * -1), continueTextLocalScale.y, continueTextLocalScale.z);
                continueText.GetComponent<RectTransform>().localPosition =
                    new Vector3((continueTextRectTransform.anchoredPosition.x * -1),
                    continueTextRectTransform.anchoredPosition.y);

                // new position of dialogue box
                Pos = Camera.main.WorldToScreenPoint(myRigidBody.position);
                Pos.y += dialgoueBoxOffsetY;
                Pos.x += dialogueBoxOffsetX;

                //sets new dialogue position in conversation
                dialogueBox.transform.position = Pos;
            } 
        }
        else if(transform.localScale.x != 1f)
        {
            //if the NPC is not facing the original position
            if (currentString.Contains("Jason") && index != 0)
            {
                /*If the next conversation string involves the player,
                 find their position and assign the dialogue box over the player position
                     
                */

                //scales of individual game objects
                var dialogueBoxLocalScale = dialogueBox.transform.localScale;
                var dialogueTextLocalScale = dialogueText.transform.localScale;
                var continueTextLocalScale = continueText.transform.localScale;

                //transforms of the game objects
                var dialogueTextRectTransform = dialogueText.GetComponent<RectTransform>();
                var continueTextRectTransform = continueText.GetComponent<RectTransform>();

                //dialogue box
                dialogueBox.transform.localScale = new Vector3(Mathf.Abs(dialogueBoxLocalScale.x), dialogueBoxLocalScale.y, dialogueBoxLocalScale.z);

                //dialoguetext
                dialogueText.transform.localScale = new Vector3((dialogueTextLocalScale.x * -1), dialogueTextLocalScale.y, dialogueTextLocalScale.z);
                dialogueText.GetComponent<RectTransform>().localPosition =
                    new Vector3((dialogueTextRectTransform.anchoredPosition.x * -1),
                    dialogueTextRectTransform.anchoredPosition.y);

                //continue text
                continueText.transform.localScale = new Vector3((continueTextLocalScale.x * -1), continueTextLocalScale.y, continueTextLocalScale.z);
                continueText.GetComponent<RectTransform>().localPosition =
                    new Vector3((continueTextRectTransform.anchoredPosition.x * -1),
                    continueTextRectTransform.anchoredPosition.y);

                // new position of dialogue box
                Pos = Camera.main.WorldToScreenPoint(player.GetComponent<Rigidbody2D>().position);
                Pos.y += dialgoueBoxOffsetY;
                Pos.x += dialogueBoxOffsetX;

                //sets new dialogue position in conversation
                dialogueBox.transform.position = Pos;
            } 
            else if (currentString.Contains("Jason") && index == 0) // clean
            {
                /*If the next conversation string involves the player and the first line involves the player,
                 find their position and assign the dialogue box over the player position
                     
                */

                //scales of individual game objects
                var dialogueBoxLocalScale = dialogueBox.transform.localScale;
                var dialogueTextLocalScale = dialogueText.transform.localScale;
                var continueTextLocalScale = continueText.transform.localScale;

                //transforms of the game objects
                var dialogueTextRectTransform = dialogueText.GetComponent<RectTransform>();
                var continueTextRectTransform = continueText.GetComponent<RectTransform>();

                //dialogue box
                dialogueBox.transform.localScale = new Vector3(Mathf.Abs(dialogueBoxLocalScale.x), dialogueBoxLocalScale.y, dialogueBoxLocalScale.z);

                //dialoguetext
                dialogueText.transform.localScale = new Vector3((dialogueTextLocalScale.x), dialogueTextLocalScale.y, dialogueTextLocalScale.z);
                dialogueText.GetComponent<RectTransform>().localPosition =
                    new Vector3((dialogueTextRectTransform.anchoredPosition.x),
                    dialogueTextRectTransform.anchoredPosition.y);

                //continue text
                continueText.transform.localScale = new Vector3((continueTextLocalScale.x), continueTextLocalScale.y, continueTextLocalScale.z);
                continueText.GetComponent<RectTransform>().localPosition =
                    new Vector3((continueTextRectTransform.anchoredPosition.x),
                    continueTextRectTransform.anchoredPosition.y);

                // new position of dialogue box
                Pos = Camera.main.WorldToScreenPoint(player.GetComponent<Rigidbody2D>().position);
                Pos.y += dialgoueBoxOffsetY;
                Pos.x += dialogueBoxOffsetX;

                //sets new dialogue position in conversation
                dialogueBox.transform.position = Pos;
            }
            else if (dialogueBox.transform.localScale.x > 0)
            {
                /*
                 * else if the dialoguebox position is not equal to 0
                 */

                //scales of individual game objects
                var dialogueBoxLocalScale = dialogueBox.transform.localScale;
                var dialogueTextLocalScale = dialogueText.transform.localScale;
                var continueTextLocalScale = continueText.transform.localScale;

                //transforms of the game objects
                var dialogueTextRectTransform = dialogueText.GetComponent<RectTransform>();
                var continueTextRectTransform = continueText.GetComponent<RectTransform>();

                //dialogue box
                dialogueBox.transform.localScale = new Vector3((dialogueBoxLocalScale.x * -1), dialogueBoxLocalScale.y, dialogueBoxLocalScale.z);

                //dialoguetext
                dialogueText.transform.localScale = new Vector3((dialogueTextLocalScale.x * -1), dialogueTextLocalScale.y, dialogueTextLocalScale.z);
                dialogueText.GetComponent<RectTransform>().localPosition =
                    new Vector3((dialogueTextRectTransform.anchoredPosition.x * -1),
                    dialogueTextRectTransform.anchoredPosition.y);

                //continue text
                continueText.transform.localScale = new Vector3((continueTextLocalScale.x * -1), continueTextLocalScale.y, continueTextLocalScale.z);
                continueText.GetComponent<RectTransform>().localPosition =
                    new Vector3((continueTextRectTransform.anchoredPosition.x * -1),
                    continueTextRectTransform.anchoredPosition.y);

                //new position of dialogue box
                Pos = Camera.main.WorldToScreenPoint(myRigidBody.position);
                Pos.y += dialgoueBoxOffsetY;
                Pos.x -= 50;

                //sets new dialogue position in conversation
                dialogueBox.transform.position = Pos;
            }
        }
        
        //types out the dialogue text
        foreach (char letter in currentString)
        {
            textDisplay.text += letter;
            yield return new WaitForSeconds(typingSpeed); //Typing speed on text

        }


        //e button is re-enabled
        eButtonEnabled = true;

    }

    //Reset dialogue box
    private void ResetDialoguePos()
    {
        //scales
        float dialogueTextScale = dialogueTextLocalScaleOriginal.y;
        float continueTextScale = continueTextLocalScaleOriginal.y;

        //rect transforms
        var dialogueTextRectTransform = dialogueText.GetComponent<RectTransform>();
        var continueTextRectTransform = continueText.GetComponent<RectTransform>();

        //restores dialogue box scale
        dialogueBox.transform.localScale = dialogueBoxLocalScaleOriginal;
        
        //restores dialogue text rect transform and local scale
        dialogueText.transform.localScale = new Vector3(dialogueTextScale, dialogueTextScale);
        dialogueText.GetComponent<RectTransform>().localPosition =
                    new Vector3( (Mathf.Abs(dialogueTextRectTransform.anchoredPosition.x) * -1 ),
                    dialogueTextRectTransform.anchoredPosition.y);

        //restores continue text rect transform and local scale
        continueText.transform.localScale = new Vector3(continueTextScale, continueTextScale);
        continueText.GetComponent<RectTransform>().localPosition =
                    new Vector3(( Mathf.Abs(continueTextRectTransform.anchoredPosition.x)),
                    continueTextRectTransform.anchoredPosition.y);
    }

    //On exit of collider reset the npc position
    private void OnTriggerExit2D(Collider2D collision)
    {
        //player x position
        var playerPosX = collision.transform.position.x;

        if (collision.gameObject.name == "Player" && collision.ToString().Contains("Capsule")) //if the collision is the player and the collider is of type capsule
        {
            if (playerPosX < myRigidBody.position.x) //if the player is behind the NPC
            {
                FlipSprite(true, false); //flip to orginal position
            }

            fader.FadeOut();

            //player is not within the collider
            playerWithinDistance = false;
        }
    }

    //On enter of collider have the NPC face the player
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //player x position
        var playerPosX = collision.transform.position.x;

        if (collision.gameObject.name == "Player" && collision.ToString().Contains("Capsule")) //if the collision is the player and the collider is of type capsule
        {
            if (playerPosX < myRigidBody.position.x) //if the player is behind the NPC
            {
                FlipSprite(false, false); //flip towards player
            }
            
            //fade in the key suggestion
            keyIndicator.SetActive(true);
            fader.FadeIn();

            //player is within the collider
            playerWithinDistance = true;
        }

    }

    //End dialogue conversation
    private void EndConversation()
    {
        //reset dialogue box and clear text
        ResetDialoguePos();
        conversationHasStarted = false;
        textDisplay.text = "";

        //re-enable player movement
        player.GetComponent<PlayerMovement>().SetMovement(true);

        //stop dialogue corutine and set the dialoguebox to inactive
        SetDialogueBoxActive(false);
        index = 0;
        StopCoroutine(Dialogue());

        fader.FadeIn();

    }

    //Start dialogue conversation
    private async void StartConversation()
    {
        //lock playermovement and animation
        player.GetComponent<PlayerMovement>().SetMovement(false);
        player.GetComponent<Animator>().SetBool("Running", false);


        var originalCamSpeed = mainCamera.velocity.x;
        var camSpeed = Mathf.Abs(mainCamera.velocity.x);

        print("INTIAL: " + camSpeed);
        while (camSpeed > 0.0f)
        {
            await WaitOneSecondAsync();
            if(originalCamSpeed > 0.0f)
            {
                camSpeed = camSpeed - 0.2f;
            }
            else
            {
                camSpeed = camSpeed - 0.09f;
            }
            
            print("NOW: " + camSpeed);
        }
        print("FINAL: " + camSpeed);


        //sets dialogue box above NPC
        Pos = Camera.main.WorldToScreenPoint(myRigidBody.position);
        Pos.y += dialgoueBoxOffsetY;
        Pos.x += dialogueBoxOffsetX;


        //prepare dialogue box
        SetDialogueBoxActive(true);
        dialogueBox.transform.position = Pos; 
        textDisplay.text = "";

        //Fade out E button suggestion
        fader.FadeOut();

        //start dialogue corutine that generates the sentences
        StartCoroutine(Dialogue());


    }

    //Listen for the player to press E so that they can start / continue conversation
    private void DialogueButtonListener()
    {
        if (Input.GetKeyDown(KeyCode.E) && !conversationHasStarted && eButtonEnabled) //if the conversation has not been started, start it.
        {
            player.GetComponent<PlayerMovement>().SetMovement(false);
            player.GetComponent<Animator>().SetBool("Running", false);
            conversationHasStarted = true;


            

            StartConversation();
        }
        else if (Input.GetKeyDown(KeyCode.E) && eButtonEnabled) //else go to the next sentence
        {
            NextSentence();
        }
    }

    //Play the next sentence
    public void NextSentence()
    {
        //continue text is activated
        continueText.SetActive(true);

        //if the player is within the persuaded NPC list, set them to persuaded
        if(FindObjectOfType<GameSession>().GetPersuadedNPCList().Contains(transform.position.x.ToString()) == true) 
        {
            SetPersuaded();
        }

        if (hasAlreadyBeenConvinced == false) //if the NPC has not been convinced
        {
            if (index < sentences.Length - 1) //if all dialogue has not been read go to the next sentence
            {
                index++;
                textDisplay.text = "";
                StartCoroutine(Dialogue());
            }
            else if (hasMiniGame) //else if at the end of the conversation, start the mini game
            {
                //sets current level
                FindObjectOfType<GameSession>().SetPreviousScene(SceneManager.GetActiveScene().buildIndex);

                //Records play position and sets the NPC pos
                RecordPlayerPosition();
                FindObjectOfType<GameSession>().SetIfNewLevel(false);

                //get coin list
                FindObjectOfType<ScenePersist>().GetScenePersistCoinList();

                //If the persuaded NPC list does not cotain this NPC, add its position
                if (FindObjectOfType<GameSession>().GetPersuadedNPCList().Contains(transform.position.x.ToString()) != true)
                {
                    FindObjectOfType<GameSession>().AddPersuadedNPC(transform.position.x.ToString());
                }

               
                //load persuasion mini game
                SceneManager.LoadScene("Persuasion_Mini_Game");
                textDisplay.text = "";
                continueText.SetActive(false);
            }
            else
            {
                //end conversation
                SetDialogueBoxActive(false);
                EndConversation();
            }
        }
        else //if the NPC has been persuaded
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
                //end conversation
                SetDialogueBoxActive(false);
                EndConversation();
            }
        }
        
    }

    //Set the dialogue box to active or inactive
    private void SetDialogueBoxActive(bool value)
    {
        dialogueBox.SetActive(value);
        continueText.SetActive(value);
        dialogueText.SetActive(value);
    }

    //Flip NPC Sprite
    private void FlipSprite(bool reset, bool exception)
    {
        
        if(reset) //Reset the Sprite to its original position
        {
            transform.localScale = new Vector2(1f, 1f);
            keyIndicator.transform.localScale = new Vector2(1f, 1f);
            return;
        }

        if (player.transform.localScale.x == transform.localScale.x || exception) //if the player and NPC are facing in the same direction or in special cases, face the player
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

    public void RecordPlayerPosition()
    {
        var temporaryPlayerPosition = FindObjectOfType<PlayerState>().GetComponent<Transform>();
        FindObjectOfType<GameSession>().TemporarilyHoldPlayerPosition(temporaryPlayerPosition);
    }

    private async Task WaitOneSecondAsync()
    {
        await Task.Delay(TimeSpan.FromSeconds(0.001));
    }

}
