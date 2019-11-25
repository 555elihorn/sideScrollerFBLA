﻿using System.Collections;
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
    Vector2 Pos;
    Vector3 dialogueBoxLocalScaleOriginal;
    Vector3 dialogueTextLocalScaleOriginal;
    Vector3 continueButtonLocalScaleOriginal;
    Vector3 dialogueTextRectTransformOriginal;
    Vector3 continueButtonRectTransformOriginal;

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
        SetDialogueBoxActive(false);

        float temp = dialogueText.GetComponent<RectTransform>().anchoredPosition.x;

        dialogueBoxLocalScaleOriginal = dialogueBox.transform.localScale;
        dialogueTextLocalScaleOriginal = dialogueText.transform.localScale;
        continueButtonLocalScaleOriginal = continueButton.transform.localScale;
        dialogueTextRectTransformOriginal = dialogueText.GetComponent<RectTransform>().localPosition;
        print(dialogueText.GetComponent<RectTransform>().localPosition);
        print(dialogueText.GetComponent<RectTransform>().localPosition.x);
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

                print("check");
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
            print("Facing Left");
            if (currentString.Contains("Player") && index != 0) //if player start of conversation
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

                Pos = Camera.main.WorldToScreenPoint(player.GetComponent<Rigidbody2D>().position);
                Pos.y += dialgoueBoxOffsetY;
                Pos.x += dialogueBoxOffsetX;

                dialogueBox.transform.position = Pos;
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

                Pos = Camera.main.WorldToScreenPoint(player.GetComponent<Rigidbody2D>().position);
                Pos.y += dialgoueBoxOffsetY;
                Pos.x += dialogueBoxOffsetX;

                dialogueBox.transform.position = Pos;
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

    }

    private void ResetDialoguePos()
    {
        float textScale = dialogueTextLocalScaleOriginal.y;
        var dialogueTextRectTransform = dialogueText.GetComponent<RectTransform>();
        var continueButtonRectTransform = continueButton.GetComponent<RectTransform>();

        dialogueBox.transform.localScale = dialogueBoxLocalScaleOriginal;
        
        dialogueText.transform.localScale = new Vector3(textScale, textScale);
        dialogueText.GetComponent<RectTransform>().localPosition =
                    new Vector3( (Mathf.Abs(dialogueTextRectTransform.anchoredPosition.x) * -1 ),
                    dialogueTextRectTransform.anchoredPosition.y);

        
        continueButton.transform.localScale = new Vector3(textScale, textScale);
        continueButton.GetComponent<RectTransform>().localPosition =
                    new Vector3(( Mathf.Abs(continueButtonRectTransform.anchoredPosition.x)),
                    continueButtonRectTransform.anchoredPosition.y);
        

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        playerWithinDistance = false;

        if (collision.gameObject.name == "Player" && collision.ToString().Contains("Capsule"))
        {
            FlipSprite(true);
            EndConversation();
        }
    }

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

    private void EndConversation()
    {
        
        conversationHasStarted = false;
        textDisplay.text = "";
        SetDialogueBoxActive(false);
        index = 0;
        StopCoroutine(Type());
        ResetDialoguePos();
        fader.FadeOut();
    }

    private void StartConversation()
    {
        //sets dialogue box above NPC
        Pos = Camera.main.WorldToScreenPoint(myRigidBody.position);
        Pos.y += dialgoueBoxOffsetY;
        Pos.x += dialogueBoxOffsetX;

        SetDialogueBoxActive(true);
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
            SceneManager.LoadScene("Persuasion_Mini_Game");
            textDisplay.text = "";
            continueButton.SetActive(false);
        }
        else
        {
            SetDialogueBoxActive(false);
        }
    }

    private void SetDialogueBoxActive(bool value)
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
