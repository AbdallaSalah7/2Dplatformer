using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] Transform farBackground, middleBackground;

    //To move camera vertically
    //[SerializeField] float minHeight = -1.5f, maxHeight = 2.5f;

    private Vector2 lastPos;
    // Start is called before the first frame update
    void Start()
    {
        lastPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {

        /*transform.position = new Vector3(target.position.x, target.position.y, transform.position.z);
        //float clapmedY = Mathf.Clamp(transform.position.y, minHeight, maxHeight);
        transform.position = new Vector3(target.position.x, clapmedY, transform.position.z);*/

        //transform.position = new Vector3(target.position.x, Mathf.Clamp(target.position.y, minHeight, maxHeight), transform.position.z);

        //float amountToMoveX = transform.position.x - lastPos;
        Vector2 amountToMove = new Vector2(transform.position.x - lastPos.x, transform.position.y - lastPos.y);

        farBackground.position = farBackground.position + new Vector3(amountToMove.x, amountToMove.y, 0f);
        middleBackground.position = middleBackground.position + new Vector3(amountToMove.x, amountToMove.y, 0f) * 0.5f;

        //lastPos = transform.position.x;
        lastPos = transform.position;
    }
}
