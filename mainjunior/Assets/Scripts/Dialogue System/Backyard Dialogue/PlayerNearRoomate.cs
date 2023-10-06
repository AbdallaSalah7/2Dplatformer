using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNearRoomate : MonoBehaviour
{
    public static PlayerNearRoomate instance;
    public GameObject CCanvas;
    public GameObject Player;
    private void Awake()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        float distToRoomate = Vector3.Distance(transform.position, Player.transform.position);
        //print("Distance to roomate is: " + distToRoomate);

        if (distToRoomate <= 1.8f)
        {
            CCanvas.SetActive(true);
            if (Input.GetKeyDown(KeyCode.C))
            {
                CCanvas.SetActive(false);
            }
        }
        if ((distToRoomate > 1.7f || DialogueTrigger.instance.PressedC))
        {
            CCanvas.SetActive(false);
        }
        if (DialogueManager.instance.DialogueEnd == true)
        {
            CCanvas.SetActive(false);
        }
    }
}
