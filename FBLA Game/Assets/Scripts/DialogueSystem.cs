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

        if(transform.localScale.x == 1f)
        {
            print("Facing Right");
            if (currentString.Contains("Player"))
            {
                print("Player line");
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
            }
        }
        else if(transform.localScale.x != 1f)
        {
            print("Facing Left");
            if (currentString.Contains("Player") && index != 0)
            {
                print("PlayerLine");
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
            }
            else if (currentString.Contains("Player") && index == 0)
            {
                print("PlayerLine");
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

            }
            else if (dialogueBox.transform.localScale.x > 0)
            {
                print("NPC line");
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
            }
        }
        

        foreach (char letter in currentString)
        {
            textDisplay.text += letter;
            yield return new WaitForSeconds(typingSpeed); //Typing speed on text

        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        playerWithinDistance = false;

        if (collision.gameObject.name == "Player" && collision.ToString().Contains("Capsule"))
        {
            print("EXIT");
            FlipSprite(true);
            EndConversation();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.gameObject.name == "Player" && collision.ToString().Contains("Capsule"))
        {
            print("ENTER");
            FlipSprite(false);
            keyIndicator.SetActive(true);
            fader.FadeIn();

            playerWithinDistance = true;
        }
    }

    private void EndConversation()
    {
        print("maybe");
        
        conversationHasStarted = false;
        textDisplay.text = "";
        SetDialogueBox(false);
        index = 0;
        StopCoroutine(Type());
        fader.FadeOut();
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

    private void FlipSprite(bool reset)
    {
        
        if(reset)
        {
            transform.localScale = new Vector2(1f, 1f);
            keyIndicator.transform.localScale = new Vector2(1f, 1f);
            return;
        }
        

        //bool playerHasHorizontalSpeed = Mathf.Abs(myRigidBody.velocity.x) > Mathf.Epsilon;
        if (player.transform.localScale.x == transform.localScale.x)
        {
            transform.localScale = new Vector2(transform.localScale.x * -1, 1f);
            keyIndicator.transform.localScale = new Vector2(transform.localScale.x, 1f);
            return;
        }

        //keyIndicator.transform.localScale = new Vector2(1f, 1f);
    }
}
