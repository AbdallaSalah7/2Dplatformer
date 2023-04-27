using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    // Start is called before the first frame update
    public SpriteRenderer theSr;
    public Sprite cpOn, cpOff;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag.Equals("Player"))
        {
            checkPointController.instance.DeactivateCheckpoints();
            theSr.sprite = cpOn;
            checkPointController.instance.setSpawnPoint(transform.position);
        }
    }
    public void resetCheckPoint()
    {
        theSr.sprite = cpOff;
    }
    

}
