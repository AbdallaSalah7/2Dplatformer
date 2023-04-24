using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    // Start is called before the first frame update
    public float moveSpeed;
    //we want to move between two spots in our scene,  we want to know the position between two spots -- transfrom 
    public Transform leftPoint, rightPoint;
    private bool movingRight; //detects the character movement, so we know if the enemy should mobe toward the left or right point
    private Rigidbody2D theRB;
    public float moveTime, waitTime;
    private float moveCount, waitCount;
    void Start()
    {
        theRB = GetComponent<Rigidbody2D>();
        leftPoint.parent = null;
        rightPoint.parent = null;
        movingRight = true;
        moveCount = moveTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (moveCount > 0)
        {
            moveCount -= Time.deltaTime;

            if (movingRight)
            {
                theRB.velocity = new Vector2(moveSpeed, theRB.velocity.y);
                if (transform.position.x > rightPoint.transform.position.x)
                {
                    movingRight = false;
                }

            }
            else
            {
                theRB.velocity = new Vector2(-moveSpeed, theRB.velocity.y);
                if (transform.position.x < leftPoint.transform.position.x)
                {
                    movingRight = true;
                }
            }

            if (moveCount <= 0)
            {
                waitCount = waitTime;
            }

        }
        else if (waitCount > 0)
        {
            waitCount -= Time.deltaTime;
            theRB.velocity = new Vector2(0f,theRB.velocity.y);
        }

    }
}
