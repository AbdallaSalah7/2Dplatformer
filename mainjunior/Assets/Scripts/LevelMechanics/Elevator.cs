using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] Transform elevatorSwitch;
    [SerializeField] Transform downpos;
    [SerializeField] Transform upperpos;
    [SerializeField] SpriteRenderer elevator;
    public float speed;
    bool isElevatorDown = false;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        startElevator();
        DisplayColor();

    }
    void startElevator()
    {
        if (Vector2.Distance(player.position, elevatorSwitch.position) < 0.5f && Input.GetButtonDown("Elevator"))
        {
            print("Testttttt");
            if (transform.position.y <= downpos.position.y)
            {
                isElevatorDown = true;
            }
            else if (transform.position.y >= upperpos.position.y)
            {
                isElevatorDown = false;
            }
        }
        if (isElevatorDown)
        {
            transform.position = Vector2.MoveTowards(transform.position, upperpos.position, speed * Time.deltaTime);
        }
        else
        {
            transform.position = Vector2.MoveTowards(transform.position, downpos.position, speed * Time.deltaTime);
        }
    }
    void DisplayColor()
    {
        if (transform.position.y <= downpos.position.y || transform.position.y >= upperpos.position.y)
        {
            elevator.color = Color.white;
        }
        else
        {
            elevator.color = Color.yellow;
        }
    }
}
