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
    [Header("Entry 6")]
    [SerializeField] private GameObject visualCue6;
    public GameObject Entry6;
    [Header("Entry 7")]
    [SerializeField] private GameObject visualCue7;
    public GameObject Entry7;
    [Header("Entry 8")]
    [SerializeField] private GameObject visualCue8;
    public GameObject Entry8;
    [Header("Entry 9")]
    [SerializeField] private GameObject visualCue9;
    public GameObject Entry9;

    void Update()
    {
        if (playerInside && Input.GetButton("Interact"))
        {

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
            if (Entry6 != null)
            {
                Entry6.SetActive(true);
            }
            if (Entry7 != null)
            {
                Entry7.SetActive(true);
            }
            if (Entry8 != null)
            {
                Entry8.SetActive(true);
            }
            if (Entry9 != null)
            {
                Entry9.SetActive(true);
            }

            // if (Input.GetButton("Interact") && Entry1Active == true)
            // {
            //     Entry1.SetActive(false);
            // }
        }
        if (!playerInside)
        {
            Entry1.SetActive(false);
            Entry2.SetActive(false);
            Entry3.SetActive(false);
            Entry4.SetActive(false);
            Entry5.SetActive(false);
            Entry6.SetActive(false);
            Entry7.SetActive(false);
            Entry8.SetActive(false);
            Entry9.SetActive(false);

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

        }
        if (other.gameObject.name.Equals("Trigger2"))
        {
            Debug.Log("Player entered the triggered area!");
            playerInside = true;
            visualCue2.SetActive(true);

        }
        if (other.gameObject.name.Equals("Trigger3"))
        {
            Debug.Log("Player entered the triggered area!");
            playerInside = true;
            visualCue3.SetActive(true);

        }
        if (other.gameObject.name.Equals("Trigger4"))
        {
            Debug.Log("Player entered the triggered area!");
            playerInside = true;
            visualCue4.SetActive(true);
        }
        if (other.gameObject.name.Equals("Trigger5"))
        {
            Debug.Log("Player entered the triggered area!");
            playerInside = true;
            visualCue5.SetActive(true);
        }
        if (other.gameObject.name.Equals("Trigger6"))
        {
            Debug.Log("Player entered the triggered area!");
            playerInside = true;
            visualCue6.SetActive(true);
        }
        if (other.gameObject.name.Equals("Trigger7"))
        {
            Debug.Log("Player entered the triggered area!");
            playerInside = true;
            visualCue7.SetActive(true);
        }
        if (other.gameObject.name.Equals("Trigger8"))
        {
            Debug.Log("Player entered the triggered area!");
            playerInside = true;
            visualCue8.SetActive(true);
        }
        if (other.gameObject.name.Equals("Trigger9"))
        {
            Debug.Log("Player entered the triggered area!");
            playerInside = true;
            visualCue9.SetActive(true);
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
        }
        if (other.gameObject.name.Equals("Trigger2"))
        {
            // Player has entered the triggered area
            Debug.Log("Player exit the triggered area!");
            playerInside = false;
            visualCue2.SetActive(false);
        }
        if (other.gameObject.name.Equals("Trigger3"))
        {
            // Player has entered the triggered area
            Debug.Log("Player exit the triggered area!");
            playerInside = false;
            visualCue3.SetActive(false);
        }
        if (other.gameObject.name.Equals("Trigger4"))
        {
            // Player has entered the triggered area
            Debug.Log("Player exit the triggered area!");
            playerInside = false;
            visualCue4.SetActive(false);

        }
        if (other.gameObject.name.Equals("Trigger5"))
        {
            // Player has entered the triggered area
            Debug.Log("Player exit the triggered area!");
            playerInside = false;
            visualCue5.SetActive(false);
        }
        if (other.gameObject.name.Equals("Trigger6"))
        {
            // Player has entered the triggered area
            Debug.Log("Player exit the triggered area!");
            playerInside = false;
            visualCue6.SetActive(false);

        }
        if (other.gameObject.name.Equals("Trigger7"))
        {
            // Player has entered the triggered area
            Debug.Log("Player exit the triggered area!");
            playerInside = false;
            visualCue7.SetActive(false);
        }
        if (other.gameObject.name.Equals("Trigger8"))
        {
            // Player has entered the triggered area
            Debug.Log("Player exit the triggered area!");
            playerInside = false;
            visualCue8.SetActive(false);
        }
        if (other.gameObject.name.Equals("Trigger9"))
        {
            // Player has entered the triggered area
            Debug.Log("Player exit the triggered area!");
            playerInside = false;
            visualCue9.SetActive(false);
        }
    }

}
