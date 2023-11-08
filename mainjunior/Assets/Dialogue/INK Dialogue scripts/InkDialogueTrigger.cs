using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InkDialogueTrigger : MonoBehaviour
{
    [Header("Visual Cue")]
    [SerializeField] private GameObject visualCue;

    [Header("Emote Animator")]
    [SerializeField] private Animator emoteAnimator;

    [Header("Ink JSON")]
    [SerializeField] private TextAsset inkJSON;

    bool DialogueIsDone = false;
    BoxCollider2D TriggerCollider;

    private bool playerInRange;

    private void Awake()
    {
        playerInRange = false;
        visualCue.SetActive(false);
        TriggerCollider = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        if (playerInRange && !InkDialogueManager.GetInstance().dialogueIsPlaying)
        {
            visualCue.SetActive(true);
            if (Input.GetButton("Interact"))
            {
                InkDialogueManager.GetInstance().EnterDialogueMode(inkJSON, emoteAnimator);
                DialogueIsDone = true;
                TriggerCollider.enabled = false;
            }
        }
        else
        {
            visualCue.SetActive(false);
            
        }
        if (DialogueIsDone){

        }
    }
    //Detect player
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
