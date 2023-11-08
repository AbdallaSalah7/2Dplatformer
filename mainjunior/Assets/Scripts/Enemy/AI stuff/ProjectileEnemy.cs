using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileEnemy : MonoBehaviour
{
    Vector3 targetPos;
    public float speed;

    private void Start() {
        targetPos = FindObjectOfType<playerPhysicsMovements>().transform.position;
    }

    private void Update() {
        transform.position = Vector2.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
        if(transform.position == targetPos){
            Destroy(gameObject);
        }
    }
}
