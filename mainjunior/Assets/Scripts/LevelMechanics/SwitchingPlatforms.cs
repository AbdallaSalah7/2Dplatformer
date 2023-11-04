using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchingPlatforms : MonoBehaviour
{
     public bool enablePlatform = true;
     public bool isToggle = true;
    // Start is called before the first frame update
    void Start()
    {
        enablePlatform = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(playerPhysicsMovements.playerJumpCounter % 2 == 0 && isToggle == false){
            //print(playerPhysicsMovements.playerJumpCounter);
            TogglePlatform();
        }
        else if(playerPhysicsMovements.playerJumpCounter % 2 != 0 && isToggle == false){
            TogglePlatform();
        }
    }

    void TogglePlatform(){
        enablePlatform = !enablePlatform;
        foreach(Transform child in gameObject.transform){
            if(child.tag != "Player")
            child.gameObject.SetActive(enablePlatform);
        }
        //gameObject.SetActive(enablePlatform);
        isToggle = true;
    }

    public void setToggleToFalse(){
        isToggle = false;
    }

}
