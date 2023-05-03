using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private Animator anim;
    //[SerializeField] private float AttackCoolDown;
     [SerializeField] private Transform pencilPoint;
     [SerializeField] private GameObject[] pencils;
    public PlayerControl playerControl;
    private float cooldownTimer = Mathf.Infinity;
    private void Awake()
    {
        playerControl = GetComponent<PlayerControl>();
    }
    private void Update()
    {
        /* if (Input.GetMouseButton(0) && cooldownTimer > AttackCoolDown && playerControl.canAttack())
        {
            Attack();
        }
        cooldownTimer += Time.deltaTime; */

    }
    private void Attack()
    {
        cooldownTimer = 0;

        pencils[FindPencils()].transform.position = pencilPoint.position;
        pencils[FindPencils()].GetComponent<Projectile>().SetDirection(Mathf.Sign(transform.localScale.x));
    }
    private int FindPencils()
    {
        for (int i = 0; i < pencils.Length; i++)
        {
            if (!pencils[i].activeInHierarchy)
                return i;
        }
        return 0;
    }

}
