using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public static Health instance;
    [SerializeField] private float startingHealth;
    public float currentHealth { get; private set; }
    private Animator anim;
    public bool dead = false;

    private void Awake()
    {
        instance = this;
        currentHealth = startingHealth;
        anim = GetComponent<Animator>();
    }
    private void Start()
    {
       
    }
    public void TakeDamage(float _damage)
    {
        currentHealth = Mathf.Clamp(currentHealth - _damage, 0, startingHealth);

        if (currentHealth > 0)
        {
            anim.SetTrigger("shot");
            //iframes
        }
        else
        {
            if (!dead)
            {
                anim.SetTrigger("Death");

                if (GetComponentInParent<EnemyPatrol>() != null)
                {
                    GetComponentInParent<EnemyPatrol>().enabled = false;
                }

                /* if (GetComponentInParent<SecurityEnemy>() != null)
                {
                    GetComponent<SecurityEnemy>().enabled = false;
                } */
                dead = true;
            }
            if (dead)
            {
                AudioManager.instance.playSFX(2);
                this.enabled = false;
            }
        }
    }
}
