using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BugLight : MonoBehaviour
{
    public Transform[] points;
    public float moveSpeed;
    private int currentPoint;

    public SpriteRenderer BatSR;

    public float distanceToAttackPlayer, chaseSpeed;

    private Vector3 attackTarget;

    public float waitAfterAttack;
    private float attackCounter;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < points.Length; i++)
        {
            points[i].parent = null;
        }
    }

    // Update is called once per frame
    void Update()
    {


        // if (Vector3.Distance(transform.position, PlayerControl.instance.transform.position) > distanceToAttackPlayer)
        // {

        //     attackTarget = Vector3.zero;

        transform.position = Vector3.MoveTowards(transform.position, points[currentPoint].position, moveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, points[currentPoint].position) < .05f)
        {
            currentPoint++;

            if (currentPoint >= points.Length)
            {
                currentPoint = 0;
            }
        }

        // if (transform.position.x < points[currentPoint].position.x)
        // {
        //     BatSR.flipX = true;
        // }
        // else if (transform.position.x > points[currentPoint].position.x)
        // {
        //     BatSR.flipX = false;
        // }
        //}
        // else
        // {
        //     //Attacking the Player

        //     // if (attackTarget == Vector3.zero)
        //     // {
        //     //     attackTarget = PlayerControl.instance.transform.position;
        //     // }

        //    //transform.position = Vector3.MoveTowards(transform.position, attackTarget, chaseSpeed * Time.deltaTime);

        //     // if (Vector3.Distance(transform.position, attackTarget) <= .1f)
        //     // {
        //     //     attackCounter = waitAfterAttack;
        //     //     attackTarget = Vector3.zero;
        //     // }
        // }

    }

}
