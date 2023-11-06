using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyManager : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] GameObject door;
    public bool isPickedUp;
    private Vector2 vel;
    public float smoothTime;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (isPickedUp)
        {
            

            Vector3 offset = new Vector3(0.5f, 1f, 0f);
            transform.position = Vector2.SmoothDamp(transform.position, player.transform.position + offset, ref vel, smoothTime);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player") && !isPickedUp)
        {
            isPickedUp = true;
            AudioManager.instance.playSFX(4);
            door.GetComponent<door>().KeyPickedUp = true;
        }

    }

}
