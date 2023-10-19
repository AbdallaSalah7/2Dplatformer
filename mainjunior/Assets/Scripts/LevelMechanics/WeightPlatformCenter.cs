using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeightPlatformCenter : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player") )
        {
            WeightPlatform.instance.shouldPlatformRotate = false;
            // WeightPlatform.instance.Player = col.gameObject.transform;
        }

        else
        {
            WeightPlatform.instance.shouldPlatformRotate = true;
            //PlayerControl.instance.anim.SetBool("CanMove", true);
        }
    }
}
