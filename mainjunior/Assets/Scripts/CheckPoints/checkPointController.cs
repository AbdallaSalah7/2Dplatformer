using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class checkPointController : MonoBehaviour
{
    // Start is called before the first frame update
    public static checkPointController instance;
    public CheckPoint[] checkpoints;
    public Vector3 spawnPoint;
    private void Awake()
    {
        if (instance != null && instance != this) {
            Destroy(gameObject);
        }
        else {
            DontDestroyOnLoad( transform.root);
            instance = this;
        }
    }
    void Start()
    {
        // will find all the active checkpoints
        checkpoints = FindObjectsOfType<CheckPoint>();
        // spawnPoint = playerPhysicsMovements.instance.transform.position;
    }
    
    public void setSpawnPoint(Vector3 newSpawnPoint)
    {
        spawnPoint = newSpawnPoint;
    }
}
