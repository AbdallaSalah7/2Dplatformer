using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        playerPhysicsMovements.instance.transform.position = new Vector2(PlayerPrefs.GetFloat("x"), PlayerPrefs.GetFloat("y"));
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
        playerPhysicsMovements.instance.gameObject.SetActive(false);

        yield return new WaitForSeconds(waitToReswamp);

        //playerPhysicsMovements.instance.gameObject.SetActive(true);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        //set the player position to the stored spawn position
        //playerPhysicsMovements.instance.transform.position = checkPointController.instance.spawnPoint;
        playerPhysicsMovements.instance.transform.position = new Vector2(PlayerPrefs.GetFloat("x"), PlayerPrefs.GetFloat("y"));

        //playerHealthController.instance.currentHealth = playerHealthController.instance.maxHealth;
        //UIController.instance.updateHealthDisplay();
    }
}
