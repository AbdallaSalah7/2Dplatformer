using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainRope : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            print("Colliding with player");
            levelManager.instance.RespawnPlayer();
        }
    }
}
