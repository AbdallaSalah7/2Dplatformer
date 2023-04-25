using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : MonoBehaviour
{
    Transform target;
    public Transform borderCheck;
    public static Zombie instance;

    public int enemyHP = 100;
    public Animator animator;
    // Start is called before the first frame update
    private void Awake() {
        instance=this;
    }
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        Physics2D.IgnoreCollision(target.GetComponent<CapsuleCollider2D>(), GetComponent<CapsuleCollider2D>());
    }

    // Update is called once per frame
    void Update()
    {
        if (target.position.x > transform.position.x)
        {
            transform.localScale = new Vector2(0.4f, 0.4f);
        }
        else
        {
            transform.localScale = new Vector2(-0.4f, 0.4f);
        }

    }
    public void TakeDamage(int damageAmount)
    {
        enemyHP -= damageAmount;

        if (enemyHP > 0)
        {
            animator.SetTrigger("hit");
        }
        else
        {
            animator.SetTrigger("death");
            GetComponent<CapsuleCollider2D>().enabled = false;
            Zombie.instance.enabled = false;
        }
    }
    private void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.tag.Equals("Player"))
        {
            print("Hi");

        }
    }
}
