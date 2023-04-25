using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickyBullet : MonoBehaviour
{
    public float lifetime = 2f;

    GameObject impactEffect;
    private float speed = 4.5f;
    public bool dir; // 1true -> right 0 -> left

    //private float direction;
    //private bool hit;
    //private BoxCollider2D boxCollider;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, lifetime);
        //boxCollider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
       /* if(hit) return;
        float movementSpeed = speed * Time.deltaTime * direction;
        transform.Translate(movementSpeed, 0, 0);*/
        //transform.position += -transform.right * Time.deltaTime * speed;

        if(dir)
            transform.position += transform.right * Time.deltaTime * speed;
        else
            transform.position += -transform.right * Time.deltaTime * speed;
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        print("Bullet collide: " + other.gameObject.tag);
        // Destroy the bullet when it collides with the wall
        if (other.gameObject.tag == "Wall")
        {
            Destroy(gameObject);
        }
        if (other.gameObject.tag == "platform"/*&& lifetime > 2f*/)
        {
            Destroy(gameObject);
        }
        
    }



    /*public void SetDirection(float _direction){
        direction = _direction;
        gameObject.SetActive(true);
        hit = false;
        boxCollider.enabled = true;
        
        float localScaleX = transform.localScale.x;
        if(Mathf.Sign(localScaleX) != _direction){
            localScaleX =-localScaleX;
        }

        transform.localScale = new Vector3(localScaleX, transform.localScale.y, transform.localScale.z);
    }

    private void Deactivate(){
        gameObject.SetActive(false);
    }*/
}
