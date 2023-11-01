using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootEnemy : MonoBehaviour
{
    
    public float speed;
    public Transform target;
    public float minDistance;
    public GameObject projectile;
    public float timeBetweenPews;
    private float nextPewTime;

    private void Update() {
        if(Time.time > nextPewTime){
            Instantiate(projectile, transform.position, Quaternion.identity);
            nextPewTime = Time.time + timeBetweenPews;
        }

        if(Vector2.Distance(transform.position, target.position) < minDistance){
            transform.position = Vector2.MoveTowards(transform.position, target.position, -speed * Time.deltaTime);
        }
        else{
            
        }
    }
}
