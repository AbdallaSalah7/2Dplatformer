using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public static DialogueTrigger instance;
    public Dialogue dialogue;
    public GameObject roommate;
    public BoxCollider2D playerCollider;
    public BoxCollider2D roommateCollider;
    public GameObject DialogueBox;
    public GameObject NextArrow;
    public GameObject Ctext;
    public bool PressedC = false;
    private void Awake()
    {
        DialogueBox.SetActive(false);
        NextArrow.SetActive(false);
        instance = this;
    }
    public void TriggerDialogue()
    {
        DialogueManager.instance.StartDialogue(dialogue);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            NextArrow.SetActive(true);
            // Check if we have a valid reference to the roommate GameObject.
            if (roommate != null)
            {
                PressedC = true;
                //DialogueBox.SetActive(true);
                // Check if the player is colliding with the roommate GameObject.
                if (IsCollidingWithRoommate())
                {
                    // Perform the interaction logic here.
                    Debug.Log("Interacted with roommate!");
                    TriggerDialogue();
                }
            }
        }
    }

    // method to check if the player is colliding with the roommate GameObject.
    public bool IsCollidingWithRoommate()
    {
        if (roommateCollider != null)
        {
            if (playerCollider != null && roommateCollider != null)
            {
                // Check if the player's collider is overlapping with the roommate's collider
                return playerCollider.bounds.Intersects(roommateCollider.bounds);
            }
        }
        return false;
    }


}
