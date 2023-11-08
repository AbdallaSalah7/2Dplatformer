using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thwomp : EnemyDamage
{
    [SerializeField] private float speed;
    [SerializeField] private float range;
    [SerializeField] private float checkDelay;
    [SerializeField] private LayerMask PlayerLayer;
    public Animator anim;

    private float checkTimer;
    private Vector3 destination;

    private bool attacking;

    private Vector3[] directions = new Vector3[4];

    private void OnEnable() {
        stop();
    }
    private void Awake() {
        anim = GetComponent<Animator>();
    }

    private void Update() {
        if(attacking)
            transform.Translate(destination * Time.deltaTime * speed);
        else{
            checkTimer += Time.deltaTime;
            if(checkTimer > checkDelay)
            CheckForPlayer();
        }
    }

    private void CheckForPlayer(){
        CalculateDirections();

        for(int i = 0; i < directions.Length; i++){
            Debug.DrawRay(transform.position, directions[i], Color.red);
            RaycastHit2D hit = Physics2D.Raycast(transform.position, directions[i], range, PlayerLayer);

            if(hit.collider != null && !attacking){
            anim.SetBool("WakeUp", true);
            attacking = true;
            destination = directions[i];
            checkTimer = 0;
            }
        }
    }

    private void CalculateDirections(){
        directions[0] = transform.right * range;
        directions[1] = -transform.right * range;
        directions[2] = transform.up * range;
        directions[3] = -transform.up * range;
    }

    private void stop(){
        destination = transform.position;
        attacking = false;
    }
    private new void OnTriggerEnter2D(Collider2D collision) {
        base.OnTriggerEnter2D(collision);
        stop();
    }
}
