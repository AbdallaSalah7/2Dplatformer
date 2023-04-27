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
        instance = this;
    }
    void Start()
    {
        // will find all the active checkpoints
        checkpoints = FindObjectsOfType<CheckPoint>();
        spawnPoint = PlayerControl.instance.transform.position;
    }
    public void DeactivateCheckpoints()
    {
        for (int i = 0; i < checkpoints.Length; i++)
        {
            checkpoints[i].resetCheckPoint();
        }
    }
    public void setSpawnPoint(Vector3 newSpawnPoint)
    {
        spawnPoint = newSpawnPoint;
    }
}
