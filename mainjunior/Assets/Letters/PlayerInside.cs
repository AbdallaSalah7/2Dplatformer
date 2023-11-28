using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInside : MonoBehaviour
{
    [Header("Entry 1")]
    [SerializeField] private GameObject visualCue1;
    private bool playerInside = false;
    public GameObject Entry1;

    [Header("Entry 2")]
    [SerializeField] private GameObject visualCue2;
    public GameObject Entry2;
    [Header("Entry 3")]
    [SerializeField] private GameObject visualCue3;
    public GameObject Entry3;
    [Header("Entry 4")]
    [SerializeField] private GameObject visualCue4;
    public GameObject Entry4;
    [Header("Entry 5")]
    [SerializeField] private GameObject visualCue5;
    public GameObject Entry5;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (playerInside && Input.GetButton("Interact"))
        {
            // Activate the "Frame 245" object
            if (Entry1 != null)
            {
                Entry1.SetActive(true);
            }
            if (Entry2 != null)
            {
                Entry2.SetActive(true);
            }
            if (Entry3 != null)
            {
                Entry3.SetActive(true);
            }
            if (Entry4 != null)
            {
                Entry4.SetActive(true);
            }
            if (Entry5 != null)
            {
                Entry5.SetActive(true);
            }
            // if (Input.GetButton("Interact") && Entry1Active == true)
            // {
            //     Entry1.SetActive(false);
            // }
        }
        if (!playerInside)
        {
            // Activate the "Frame 245" object
            Entry1.SetActive(false);
            Entry2.SetActive(false);
            Entry3.SetActive(false);
            Entry4.SetActive(false);
            Entry5.SetActive(false);
        }

    }
    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.gameObject.CompareTag("Trigger"))
        {
            // Player has entered the triggered area
            Debug.Log("Player entered the triggered area!");
            playerInside = true;
            visualCue1.SetActive(true);
            // Add your desired actions or code here
        }
        if (other.gameObject.name.Equals("Trigger2"))
        {
            Debug.Log("Player entered the triggered area!");
            playerInside = true;
            visualCue2.SetActive(true);
            // Add your desired actions or code here
        }
        if (other.gameObject.name.Equals("Trigger3"))
        {
            Debug.Log("Player entered the triggered area!");
            playerInside = true;
            visualCue3.SetActive(true);
            // Add your desired actions or code here
        }
        if (other.gameObject.name.Equals("Trigger4"))
        {
            Debug.Log("Player entered the triggered area!");
            playerInside = true;
            visualCue4.SetActive(true);
            // Add your desired actions or code here
        }
        if (other.gameObject.name.Equals("Trigger5"))
        {
            Debug.Log("Player entered the triggered area!");
            playerInside = true;
            visualCue5.SetActive(true);
            // Add your desired actions or code here
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {


        if (other.gameObject.CompareTag("Trigger"))
        {
            // Player has entered the triggered area
            Debug.Log("Player exit the triggered area!");
            playerInside = false;
            visualCue1.SetActive(false);
            Entry1.SetActive(false);
            // Add your desired actions or code here
        }
        if (other.gameObject.name.Equals("Trigger2"))
        {
            // Player has entered the triggered area
            Debug.Log("Player exit the triggered area!");
            playerInside = false;
            visualCue2.SetActive(false);
            // Add your desired actions or code here
        }
        if (other.gameObject.name.Equals("Trigger3"))
        {
            // Player has entered the triggered area
            Debug.Log("Player exit the triggered area!");
            playerInside = false;
            visualCue3.SetActive(false);
            // Add your desired actions or code here
        }
        if (other.gameObject.name.Equals("Trigger4"))
        {
            // Player has entered the triggered area
            Debug.Log("Player exit the triggered area!");
            playerInside = false;
            visualCue4.SetActive(false);
            // Add your desired actions or code here
        }
        if (other.gameObject.name.Equals("Trigger5"))
        {
            // Player has entered the triggered area
            Debug.Log("Player exit the triggered area!");
            playerInside = false;
            visualCue5.SetActive(false);
            // Add your desired actions or code here
        }

    }

}
