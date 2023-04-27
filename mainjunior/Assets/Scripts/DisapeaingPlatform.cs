using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisapeaingPlatform : MonoBehaviour
{
    public float ToToggle = 2;
    public float currentTime = 0;
    public bool enablePlatform = true;
    // Start is called before the first frame update
    void Start()
    {
        enablePlatform = true;
    }

    // Update is called once per frame
    void Update()
    {
        currentTime += Time.deltaTime;
        if(currentTime >= ToToggle){
            currentTime = 0;
            TogglePlatform();
        }
    }
    void TogglePlatform(){
        enablePlatform = !enablePlatform;
        foreach(Transform child in gameObject.transform){
            if(child.tag != "Player")
            child.gameObject.SetActive(enablePlatform);
        }
    }
}
