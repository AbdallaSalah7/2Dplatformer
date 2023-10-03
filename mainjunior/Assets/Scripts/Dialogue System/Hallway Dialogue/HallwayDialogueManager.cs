using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HallwayDialogueManager : MonoBehaviour
{
    public static HallwayDialogueManager instance;

    public Text nameText;
    public Text dialogueText;
    public GameObject DialogueCanvas;
    public GameObject CCanvas;
    [SerializeField] public bool HallwayDialogueEnd = false;
    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        instance = this;
    }
    private Queue<string> sentences;

    void Start()
    {
        sentences = new Queue<string>();
    }
    public void StartDialogue(HallwayDialogue dialogue)
    {
        nameText.text = dialogue.name;
        sentences.Clear();

        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }
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
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
    }

    IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(0.05f);
        }
    }
    void EndDialogue()
    {
        Debug.Log("End of convo");
        HallwayDialogueEnd = true;
        DialogueCanvas.SetActive(false);
        //Destroy(DialogueCanvas);
        HallwayDialogueEnd = true;
    }

}
