using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour
{
    public GameObject objectToSwitch;

    private SpriteRenderer UpSprite;
    public Sprite downSprite;

    private bool hasSwitched;

    public bool deactivateOnSwitch;

    // Start is called before the first frame update
    void Start()
    {
        UpSprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" && !hasSwitched)
        {
            if (deactivateOnSwitch)
            {
                
                Destroy(objectToSwitch.gameObject, 0.5f);

            }
            else
            {
                objectToSwitch.SetActive(true);
            }

            UpSprite.sprite = downSprite;
            hasSwitched = true;
        }
    } 
}
