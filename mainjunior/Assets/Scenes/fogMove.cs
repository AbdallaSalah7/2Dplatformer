using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fogMove : MonoBehaviour
{
    [SerializeField] GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    
    transform.position = new Vector2(player.transform.position.x, transform.position.y);

    }
}
