using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dialogueManager : MonoBehaviour
{
    public static dialogueManager instance;
    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Found more than one Dialogue Manager in the scene");
        }
        instance = this;
    }
    public static dialogueManager GetInstance()
    {
        return instance;
    }

}
