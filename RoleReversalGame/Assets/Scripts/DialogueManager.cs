using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    [Header("Player Input")]
    [SerializeField] private PlayerInput player;

    [Header("Sock Character Sprite")]
    [SerializeField] private SockCharacterAnim sockCharacterAnim;

    [Header("UI Fields")]
    [SerializeField] private TextMeshProUGUI dialogueTextUI;

    [Header("Internal Vars")]
    private Queue<string> sentences;
    private Queue<Sprite> sockSprites;

    [Header("Animation")]
    [SerializeField] private Animator animtor;

    private AudioSource dialogueAudio;
    public bool endOfCurrentConvo = false;

    // Start is called before the first frame update
    void Awake()
    {
        sentences = new Queue<string>();
        sockSprites = new Queue<Sprite>();
        dialogueAudio = GetComponent<AudioSource>();
        string s = System.Environment.UserName;
    }

    public void StartDialogue(Dialogue dialogue)
    {
        sockCharacterAnim.gameObject.SetActive(true);
        animtor.SetBool("Is Open", true);
        sentences.Clear();
        sockSprites.Clear();

        foreach (string sentence in dialogue.sentences)
            sentences.Enqueue(sentence);

        foreach (Sprite sprite in dialogue.sprites)
            sockSprites.Enqueue(sprite);

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        string sentence = sentences.Dequeue();
        if (sentence.Contains("name"))
            sentence = sentence.Replace("name", System.Environment.UserName);

        Sprite currSprite = sockSprites.Dequeue();
        sockCharacterAnim.ChangeToSprite(currSprite);
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
    }

    IEnumerator TypeSentence(string sentence)
    {
        dialogueTextUI.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            switch (letter)
            {
                default:
                    dialogueTextUI.text += letter;
                    dialogueAudio.PlayOneShot(dialogueAudio.clip);
                    yield return new WaitForSeconds(0.025f);
                    break;

                case '.':
                    dialogueTextUI.text += letter;
                    yield return new WaitForSeconds(0.4f);
                    break;

                case ',':
                    dialogueTextUI.text += letter;
                    dialogueAudio.PlayOneShot(dialogueAudio.clip);
                    yield return new WaitForSeconds(0.1f);
                    break;

                case '!':
                    dialogueTextUI.text += letter;
                    yield return new WaitForSeconds(0.4f);
                    break;

                case '?':
                    dialogueTextUI.text += letter;
                    yield return new WaitForSeconds(0.4f);
                    break;
            }
        }
    }

    private void EndDialogue()
    {
        animtor.SetBool("Is Open", false);
        Debug.Log("End of convo");
        endOfCurrentConvo = true;
    }
}
