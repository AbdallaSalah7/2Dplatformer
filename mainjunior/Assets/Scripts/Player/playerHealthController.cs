using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerHealthController : MonoBehaviour
{
    // Start is called before the first frame update

    //all will have the same value
    public static playerHealthController instance;
    public float invincibleLength = 1;
    private float invincibleCount;
    public int currentHealth;
    public int maxHealth = 1;
    private SpriteRenderer theSr;
    public GameObject deathEffect;

    //awake is called before the start
    private void Awake()
    {
        //the instance is set to the current version of the player controller
        instance = this;
    }
    void Start()
    {
        currentHealth = 1;
        theSr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (invincibleCount > 0)
        {
            invincibleCount = invincibleCount - Time.deltaTime;

            /*if (invincibleCount <= 0)
            {
                print("why");
                theSr.color = new Color(theSr.color.r, theSr.color.g, theSr.color.b, 1f);
            }*/
        }
    }
    public void DealDamage()
    {
        if (invincibleCount <= 0)
        {
            currentHealth--;

            //if (currentHealth <= 0)
            //{
                currentHealth = 0;
                //Instantiate(deathEffect,transform.position,transform.rotation);
                //gameObject.SetActive(false);
                levelManager.instance.RespawnPlayer();
            //}
            /* else
            {
                invincibleCount = invincibleLength;
                theSr.color = new Color(theSr.color.r, theSr.color.g, theSr.color.b, 0.5f);
                PlayerControl.instance.KnockBack();
            } */
            //UIController.instance.updateHealthDisplay();
        }
    }

}
