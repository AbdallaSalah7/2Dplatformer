using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Disabletaxi : MonoBehaviour
{
    [SerializeField] GameObject taxi;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.tag.Equals("Player")){
            taxi.SetActive(false);
        }
    }
}
