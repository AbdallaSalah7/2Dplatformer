using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecurityEnemy : MonoBehaviour
{
    [Header("Attack Parameters")]
    [SerializeField] private float attackCooldown;
    [SerializeField] private int damage;
    [SerializeField] float range = 1f;

    [Header("Collider Parameters")]
    [SerializeField] private BoxCollider2D boxcollider;

    [SerializeField] float colliderDistance;
    [Header("Player Parameters")]
    [SerializeField] private LayerMask playerLayer;
    private float cooldownTimer = Mathf.Infinity;
    public GameObject player;
    private Animator anim;
    private EnemyPatrol enemypatrol;
    private bool canmove = true;
    // Start is called before the first frame update
    private void Awake()
    {
        anim = GetComponent<Animator>();
        enemypatrol = GetComponentInParent<EnemyPatrol>();
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Health.instance.dead == true)
        {
            print("dead");
            boxcollider.enabled = false;
            canmove = false;
             
        }
        cooldownTimer += Time.deltaTime;

        if(canmove){
            if (PlayerInsight())
        {
            if (cooldownTimer >= attackCooldown)
            {
                cooldownTimer = 0;
                anim.SetTrigger("chase");
                //chase;
            }
        }
        //if enemy doesnt see the player, then he keeps patroling, otherwise stop
        if (enemypatrol != null)
        {
            enemypatrol.enabled = !PlayerInsight();
        }
        }

    }

    
    private bool PlayerInsight()
    {
        RaycastHit2D hit = Physics2D.BoxCast(boxcollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
        new Vector3(boxcollider.bounds.size.x * range, boxcollider.bounds.size.y, boxcollider.bounds.size.z)
        , 0, Vector2.left, 0, playerLayer);
        
        return hit.collider != null;
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(boxcollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
        new Vector3(boxcollider.bounds.size.x * range, boxcollider.bounds.size.y, boxcollider.bounds.size.z));

    }


    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.gameObject.tag == "Player")
        {
            levelManager.instance.RespawnPlayer();
            AudioManager.instance.playSFX(3);
        }
    }
}
