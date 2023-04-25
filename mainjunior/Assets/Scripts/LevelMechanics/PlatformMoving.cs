using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMoving : MonoBehaviour
{
    public Transform[] wayPoints;
    public float speed;
    public float minDistance = 0.001f;
    private int _currentWayPointIndex;

    private void MovePlatform()
    {
        Vector3 target = wayPoints[_currentWayPointIndex].position;

        transform.position = Vector3.MoveTowards(current: transform.position, target, maxDistanceDelta: speed * Time.deltaTime);
        if (Vector3.SqrMagnitude(vector: target - transform.position) < minDistance * minDistance)
        {
            _currentWayPointIndex =(_currentWayPointIndex + 1)%wayPoints.Length ;
        }
    }

    // Update is called once per frame
    void Update()
    {
        MovePlatform();
    }
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.CompareTag("Player")){
            other.transform.SetParent(gameObject.transform);
        }
    }
    private void OnTriggerExit2D(Collider2D other) {
        if(other.gameObject.CompareTag("Player")){
            other.transform.SetParent(p:null);
        }
    }
}
