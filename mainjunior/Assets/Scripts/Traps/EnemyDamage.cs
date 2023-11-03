using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    [SerializeField] private float damage;
    private void Start() {
        
    }
    public void OnTriggerEnter2D(Collider2D collision) {
        if(collision.tag == "Player")
            collision.GetComponent<Healthp>().TakeDamage(damage);
    }
}