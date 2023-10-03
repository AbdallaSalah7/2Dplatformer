using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dialogueTrigger : MonoBehaviour
{
    [Header("Visual Cue")]
    [SerializeField] private GameObject visualCue;

    [Header("Ink JSON")]
    [SerializeField] public TextAsset inkJSON;
    private bool playerInRange;

    private void Awake()
    {
        playerInRange = false;
        visualCue.SetActive(false);
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (playerInRange)
        {
            visualCue.SetActive(true);
           // if (InputManager.GetInstance().GetInteractPressed())
           // {
                //DialogueManager.GetInstance().EnterDialogueMode(inkJSON, emoteAnimator);
              //  Debug.Log(inkJSON.text);
            //}
        }
        else
        {
            visualCue.SetActive(false);
        }

    }
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            playerInRange = false;
        }
    }
}
