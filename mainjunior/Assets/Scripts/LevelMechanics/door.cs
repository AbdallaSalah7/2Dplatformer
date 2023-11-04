using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class door : MonoBehaviour
{
    public bool locked;
    [SerializeField] GameObject myDoor;
    public bool KeyPickedUp;
    // Start is called before the first frame update
    void Start()
    {
        locked = true;
    }

    // Update is called once per frame
    void Update()
    {

    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Key") && KeyPickedUp)
        {
            locked = false;
            Destroy(myDoor.gameObject, 0.6f);
            other.gameObject.SetActive(false);
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Key") && KeyPickedUp)
        {
            locked = true;
        }
    }
}
