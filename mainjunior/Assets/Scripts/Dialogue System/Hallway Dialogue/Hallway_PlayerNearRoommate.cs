using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hallway_PlayerNearRoomate : MonoBehaviour
{
    public GameObject Hallway_CCanvas;
    public GameObject Player;

    // Update is called once per frame
    void Update()
    {
        float distToRoomate = Vector3.Distance(transform.position, Player.transform.position);
        print("Distance to roomate is: " + distToRoomate);

        if (distToRoomate <= 1.8f && DialogueManager.instance.DialogueEnd == true)
        {
            Hallway_CCanvas.SetActive(true);
            print("WHy is it not working?");
            if (Input.GetKeyDown(KeyCode.C))
            {
                Hallway_CCanvas.SetActive(false);
            }
        }
        if ((distToRoomate > 1.7f || HallwayDialogueTrigger.instance.PressedC))
        {
            Hallway_CCanvas.SetActive(false);
        }
        if (HallwayDialogueManager.instance.HallwayDialogueEnd == true)
        {
            Hallway_CCanvas.SetActive(false);

        }



    }
}
