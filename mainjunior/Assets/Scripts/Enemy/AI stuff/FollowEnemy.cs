using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowEnemy : EnemyDamage
{
    
    public float speed;
    public Transform target;
    public float minDistance;

    public Animator anim;
    [SerializeField] private BoxCollider2D boxcollider;

    private void Awake() {
        anim = GetComponent<Animator>();
    }

    private void Update() {
        // Determine the direction to the target
        Vector3 direction = (target.position - transform.position).normalized;

        // Update the character's scale to flip in the correct direction
        if (direction.x < 0)
            transform.localScale = new Vector3(-1, 1, 1); // Flip to face left
        else if (direction.x > 0)
            transform.localScale = new Vector3(1, 1, 1); // Face right

        if(Vector2.Distance(transform.position, target.position) > minDistance){
            transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
            anim.SetBool("chase", true);
        }
        else{
            anim.SetBool("chase", false);
        }

        if (GetComponent<Health>().dead == true)
            {
                print("test");
                boxcollider.enabled = false;
                speed = 0;
                StartCoroutine(Des());
            }

            IEnumerator Des(){
        yield return new WaitForSeconds(3);
        Destroy(gameObject);
    }
        
    }
}
