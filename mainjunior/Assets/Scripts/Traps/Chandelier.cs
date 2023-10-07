using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chandelier : EnemyDamage
{
    [SerializeField] private float speed;
    [SerializeField] private float range;
    [SerializeField] private float checkDelay;
    [SerializeField] private LayerMask PlayerLayer;
    private float checkTimer;
    private Vector3 destination;

    private bool attacking;

    private Vector3 direction = new Vector3();

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

        Debug.DrawRay(transform.position, direction, Color.red);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, range, PlayerLayer);

        if(hit.collider != null && !attacking){
            attacking = true;
            destination = direction;
            checkTimer = 0;
        }
    }

    private void CalculateDirections(){
        direction = -transform.up * range;
        
    }
    private void OnTriggerEnter2D(Collider2D collision) {
        base.OnTriggerEnter2D(collision);
        //Destroy();
    }
}
