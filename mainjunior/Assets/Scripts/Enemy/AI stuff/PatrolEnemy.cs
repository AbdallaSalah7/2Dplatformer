using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolEnemy : EnemyDamage
{
    public float speed;
    public Transform[] patrolPoints;
    [SerializeField] private BoxCollider2D boxcollider;
    public float waitTime;
    int currentPointIndex;
    bool once;
    public Animator anim;
    private bool canmove = true;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] float range = 1f;
    [SerializeField] float colliderDistance;
    private float cooldownTimer = Mathf.Infinity;

    private void OnDisable() {
        anim.SetBool("moving", false);
    }

    private void Update() {
        if(canmove){
            if(transform.position != patrolPoints[currentPointIndex].position){
                Vector3 direction = (patrolPoints[currentPointIndex].position - transform.position).normalized;

                if (direction.x < 0)
                    transform.localScale = new Vector3(-1, 1, 1); // Flip to face left
                else if (direction.x > 0)
                    transform.localScale = new Vector3(1, 1, 1); // Face right

                transform.position = Vector2.MoveTowards(transform.position, patrolPoints[currentPointIndex].position, speed * Time.deltaTime);
                anim.SetBool("moving", true);
            }
            else{
                if(once == false){
                    once = true;
                    anim.SetBool("moving", false);
                    StartCoroutine(Wait());
                }
            }}

            if (GetComponent<Health>().dead == true)
            {
                print("test");
                boxcollider.enabled = false;
                canmove = false;
                StartCoroutine(Des());
            }

            cooldownTimer += Time.deltaTime;
            if(canmove){
            if (PlayerInsight())
        {
            if (cooldownTimer >= 2)
            {
                cooldownTimer = 0;
                anim.SetTrigger("chase");
                //chase;
            }
        }
            }
    }

    IEnumerator Wait(){
        yield return new WaitForSeconds(waitTime);
        if(currentPointIndex + 1 < patrolPoints.Length){
            currentPointIndex++;
        }
        else{
            currentPointIndex = 0;
        }
        once = false;
    }

    IEnumerator Des(){
        yield return new WaitForSeconds(3);
        Destroy(gameObject);
    }

    private bool PlayerInsight()
    {
        RaycastHit2D hit = Physics2D.BoxCast(boxcollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
        new Vector3(boxcollider.bounds.size.x * range, boxcollider.bounds.size.y, boxcollider.bounds.size.z)
        , 0, Vector2.left, 0, playerLayer);
        
        return hit.collider != null;
    }
}
