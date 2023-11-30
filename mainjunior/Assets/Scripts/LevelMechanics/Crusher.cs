using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crusher : MonoBehaviour
{
    [SerializeField] private float upSpeed;
    [SerializeField] private float downSpeed;
    [SerializeField] private Transform up;
    [SerializeField] private Transform down;
    bool chop;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y >= up.position.y)
        {
            chop = true;
        }
        if (transform.position.y <= down.position.y)
        {
            chop = false;

        }
        if (chop)
        {
            transform.position = Vector2.MoveTowards(transform.position, down.position, downSpeed * Time.deltaTime);
        }
        else
        {
            transform.position = Vector2.MoveTowards(transform.position, up.position, upSpeed * Time.deltaTime);

        }

    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.gameObject.tag == "Player")
        {
            levelManager.instance.RespawnPlayer();
        }


    }
}
