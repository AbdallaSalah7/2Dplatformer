using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingPlatform : MonoBehaviour
{
    [SerializeField] private float fallDelay = 1f;
    [SerializeField] private float destroyDelay = 2f;

    private bool falling = false;
    //private Vector2 initialPos;
    
    [SerializeField] private Rigidbody2D rb;

    private void OnCollisionEnter2D(Collision2D collision) {
        if(falling)
        return;

        if(collision.transform.tag == "Player"){
            StartCoroutine(StartFall());
        }
    }

    private IEnumerator StartFall(){
        falling = true;

        yield return new WaitForSeconds(fallDelay);

        rb.bodyType = RigidbodyType2D.Dynamic;
        Destroy(gameObject, destroyDelay);
        yield return new WaitForSeconds(5f);
        /* falling = false;
        rb.bodyType = RigidbodyType2D.Kinematic;
        yield return new WaitForSeconds(2f);
        transform.position = initialPos; */
    }

    // Start is called before the first frame update
    void Start()
    {
        //initialPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
