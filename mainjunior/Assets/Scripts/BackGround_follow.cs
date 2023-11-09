using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGround_follow : MonoBehaviour
{

    public static BackGround_follow instance;

    public Transform target;

    public Transform farBackground;

    private Vector2 lastPos;

    public float minHeight, maxHeight;

    // Start is called before the first frame update

    private void Awake()
    {
        instance = this;
    }
    
    void Start()
    {
        lastPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(target.position.x, Mathf.Clamp(target.position.y + 2f, minHeight, maxHeight), transform.position.z);

            //float amountToMoveX = transform.position.x - lastXPos;
            Vector2 amountToMove = new Vector2(transform.position.x - lastPos.x, transform.position.y - lastPos.y);

            farBackground.position = farBackground.position + new Vector3(amountToMove.x, amountToMove.y, 0f);

            lastPos = transform.position;
    }
}
