using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    // Start is called before the first frame update
    
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag.Equals("Player"))
        {   
            var xPos = playerPhysicsMovements.instance.transform.position.x;
            var yPos = playerPhysicsMovements.instance.transform.position.y;
            PlayerPrefs.SetFloat("x",xPos);
            PlayerPrefs.SetFloat("y",yPos);
            PlayerPrefs.Save();
            checkPointController.instance.setSpawnPoint(transform.position);
        }
    }
    
    

}
