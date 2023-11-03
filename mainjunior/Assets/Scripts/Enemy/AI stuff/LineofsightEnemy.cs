using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineofsightEnemy : MonoBehaviour
{
    public float rotationSpeed;
    public float visionDis;

    private void Update() {
        transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);

         RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, transform.right, visionDis);
         
         if(hitInfo.collider != null){
            Debug.DrawLine(transform.position, hitInfo.point, Color.red);
            if(hitInfo.collider.tag == "Player"){

            }
         }
         else{
            Debug.DrawLine(transform.position, transform.position + transform.right * visionDis, Color.green);
         }
    }
}
