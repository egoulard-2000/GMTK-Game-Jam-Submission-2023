using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerInput : MonoBehaviour
{
    [Header("Dialogue Controls")]
    [SerializeField] private DialgoueTrigger[] allConversations;

    [Header("UI Stuff")]
    [SerializeField] private GameObject title;
    [SerializeField] private TextMeshProUGUI clickAnywhereToBegin;

    [Header("Sounds")]
    [SerializeField] private AudioSource monkeyMusic;
    [SerializeField] private AudioSource childrenScream;
    [SerializeField] private AudioSource doorSlam;
    [SerializeField] private AudioSource booSound;
    [SerializeField] private AudioSource slideClick;
    [SerializeField] private AudioSource sonicMusic;

    [Header("Monkey Images")]
    [SerializeField] private GameObject funnyMonkey1;
    [SerializeField] private GameObject funnyMonkey2;
    [SerializeField] private GameObject creepyMonkey;
    [SerializeField] private GameObject monkeyInASuit;
    [SerializeField] private GameObject stagReel;

    [Header("Ending Part")]
    [SerializeField] private GameObject theEnd;
    [SerializeField] private GameObject sockerton;
    [SerializeField] private GameObject scaryGuy;

    [SerializeField] private DialogueManager dialogueManager;

    [Header("Track Dialogue Status")]
    private bool conversationStarted = false;
    private int convoTracker;
    private bool endOfGame = false;

    void Awake()
    {
        convoTracker = 0;
        theEnd.SetActive(false);
        scaryGuy.SetActive(false);
        title.SetActive(true);

        // Gags
        funnyMonkey1.SetActive(false);
        funnyMonkey2.SetActive(false);
        creepyMonkey.SetActive(false);
        monkeyInASuit.SetActive(false);
        stagReel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        CheckIfCurrentConvoEnded();
        ProgressThroughDialogue();
    }

    private void CheckIfCurrentConvoEnded()
    {
        if (dialogueManager.endOfCurrentConvo && !endOfGame)
        {
            convoTracker++;
            conversationStarted = false;
            dialogueManager.endOfCurrentConvo = false;

            if (convoTracker == 2)
                childrenScream.Play();

            if (convoTracker == 4)
                sonicMusic.Stop();

            if (convoTracker == 10)
            {
                slideClick.Play();
                stagReel.SetActive(true);
            }

            if (convoTracker == 11)
            {
                slideClick.Play();
                stagReel.SetActive(false);
                funnyMonkey2.SetActive(true);
            }

            if (convoTracker == 12)
            {
                slideClick.Play();
                funnyMonkey2.SetActive(false);
                creepyMonkey.SetActive(true);
            }

            if (convoTracker == 13)
            {
                slideClick.Play();
                creepyMonkey.SetActive(false);
                monkeyInASuit.SetActive(true);
            }

            if (convoTracker == 14)
            {
                slideClick.Play();
                monkeyInASuit.SetActive(false);
                funnyMonkey1.SetActive(true);
            }

            if (convoTracker == 16)
                monkeyMusic.Play();

            if (convoTracker == 18)
                funnyMonkey1.SetActive(false);

            if (convoTracker == 23)
            {
                convoTracker++;
                endOfGame = true;
                doorSlam.Play();
                sockerton.SetActive(false);
                theEnd.SetActive(true);
                dialogueManager.gameObject.SetActive(false);
                StopAllCoroutines();
                StartCoroutine(WaitUntilProgramClose());
            }
        }
    }

    private IEnumerator WaitUntilProgramClose()
    {
        yield return new WaitForSeconds(5f);
        booSound.PlayOneShot(booSound.clip);
        yield return new WaitForSeconds(0.6f);
        scaryGuy.SetActive(true);
        yield return new WaitForSeconds(1f);
        Application.Quit();
    }

    private void ProgressThroughDialogue()
    {
        if (Input.GetMouseButtonDown(0) && !conversationStarted)
        {
            if (!sonicMusic.isPlaying && convoTracker == 0)
                sonicMusic.Play();

            title.SetActive(false);
            clickAnywhereToBegin.text = "";
            conversationStarted = true;
            allConversations[convoTracker].TriggerDialogue();
        }
        else if (Input.GetMouseButtonDown(0) && conversationStarted)
        {
            dialogueManager.DisplayNextSentence();
        }
    }

    public bool PressedClick()
    {
        if (Input.GetMouseButtonDown(0))
            return true;

        return false;
    }
}
