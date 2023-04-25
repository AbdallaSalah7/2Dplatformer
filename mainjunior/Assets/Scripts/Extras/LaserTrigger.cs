using UnityEngine;

public class LaserTrigger : MonoBehaviour
{
    public LaserRoom laserRoom;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
          print("yes");
        }
    }
}