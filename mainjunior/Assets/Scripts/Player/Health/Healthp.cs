using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Healthp : MonoBehaviour
{
    [SerializeField] private float startingHealth;
    private float currentHealth;
    private bool dead;

    private void Awake() {
        currentHealth=startingHealth;
    }
    
    public void TakeDamage(float _damage){
        currentHealth = Mathf.Clamp(currentHealth - _damage,0, startingHealth);
        if(!dead){
            levelManager.instance.RespawnPlayer();
            dead = true;
        }
    }
}
