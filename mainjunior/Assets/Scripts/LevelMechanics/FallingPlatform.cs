using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingPlatform : MonoBehaviour
{
    [SerializeField] private float fallDelay = 1f;
    //[SerializeField] private float destroyDelay = 2f;

    private bool falling = false;
    private Vector2 initialPos;
    
    [SerializeField] private Rigidbody2D rb;

    private void Start() {
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        initialPos = transform.position;
    }
    

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
        rb.constraints &= ~RigidbodyConstraints2D.FreezePositionY;
        //rb.bodyType = RigidbodyType2D.Dynamic;
        //Destroy(gameObject, destroyDelay);
        yield return new WaitForSeconds(5f);
        falling = false;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        transform.position = initialPos;
        /* falling = false;
        rb.bodyType = RigidbodyType2D.Kinematic;
        yield return new WaitForSeconds(2f);
        transform.position = initialPos; */
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
