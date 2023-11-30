using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using Mono.Cecil;
using UnityEngine;

public class Head : MonoBehaviour
{
    [SerializeField] GameObject target;
    Rigidbody2D rb;
    bool isDrop = false;
    Vector3 pos;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        pos = rb.position;
    }

    // Update is called once per frame
    void Update()
    {
        Checkforplayer();

    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if ((other.gameObject.tag == "Player" || other.gameObject.layer == LayerMask.NameToLayer("Platform")) && isDrop)
        {
            rb.gravityScale = -0.1f;
            
            if (Vector3.Distance(rb.position, pos) < 0.1f)
            {
                isDrop = false;
                print("test");
            }
            if(other.gameObject.tag == "Player"){
                levelManager.instance.RespawnPlayer();
            }
        }
        
    }

    void Checkforplayer()
    {
        float distance = Mathf.Abs(target.transform.position.x - transform.position.x);
        print(distance);
        if (distance < 3 && !isDrop)
        {
            rb.gravityScale = 10;
            isDrop = true;
        }

    }
}
