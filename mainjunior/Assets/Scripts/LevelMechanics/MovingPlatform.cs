using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform[] points;
    public float moveSpeed;
    public int currentPoint;
    public Transform platform;
    void Start()
    {
        transform.position = points[currentPoint].position;
    }

    // Update is called once per frame
    void Update()
    {
        platform.position = Vector3.MoveTowards(platform.position, points[currentPoint].position, moveSpeed * Time.deltaTime);

        if (Vector3.Distance(platform.position, points[currentPoint].position) < 0.05f)
        {
            currentPoint++;

            if (currentPoint >= points.Length)
            {
                currentPoint = 0;
            }
        }

    }
    //to make player stick to moving platform
    private void OnCollisionEnter2D(Collision2D other)
    {
        other.transform.SetParent(transform);
    }
    private void OnCollisionExit2D(Collision2D other)
    {
        other.transform.SetParent(null);
    }
}
