using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class levelManager : MonoBehaviour
{
    public static levelManager instance;
    public float waitToReswamp;
    public int gemsCollected;
    // Start is called before the first frame update
    private void Awake()
    {
        instance = this;
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void RespawnPlayer()
    {
        StartCoroutine(RespwanCo());
    }
    private IEnumerator RespwanCo()
    {
        //deactivate the player
        PlayerControl.instance.gameObject.SetActive(false);

        yield return new WaitForSeconds(waitToReswamp);

        PlayerControl.instance.gameObject.SetActive(true);

        //set the player position to the stored spawn position
        PlayerControl.instance.transform.position = checkPointController.instance.spawnPoint;

        playerHealthController.instance.currentHealth = playerHealthController.instance.maxHealth;
        UIController.instance.updateHealthDisplay();
    }
}
