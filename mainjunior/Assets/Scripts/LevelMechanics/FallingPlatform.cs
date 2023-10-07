using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingPlatform : MonoBehaviour
{
    [SerializeField] private float fallDelay = 1f;
    [SerializeField] private float destroyDelay = 2f;

    private bool falling = false;
    
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
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
