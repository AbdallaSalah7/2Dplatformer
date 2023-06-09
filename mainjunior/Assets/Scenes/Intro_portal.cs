using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Intro_portal : MonoBehaviour
{
    string sceneName = "testing2";
    public Transform destination;
    GameObject player;
    Animation anim;
    Rigidbody2D playerRB;
    Animator anima;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        anim = player.GetComponent<Animation>();
        //anima = player.GetComponent<Animator>();
        playerRB = player.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {

            if (Vector2.Distance(player.transform.position, transform.position) > 0.1f)
            {
                StartCoroutine(PortalIn());
            }

        }

    }
    IEnumerator PortalIn()
    {
        playerRB.simulated = false;
        StartCoroutine(MoveInPortal());


        yield return new WaitForSeconds(0.2f);
        SceneManager.LoadScene(sceneName);
        playerRB.velocity = Vector2.zero;

        yield return new WaitForSeconds(0.2f);
        playerRB.simulated = true;
    }
    IEnumerator MoveInPortal()
    {
        float timer = 0;
        while (timer < 0.5f)
        {
            player.transform.position = Vector2.MoveTowards(player.transform.position, transform.position, 3 * Time.deltaTime);
            yield return new WaitForEndOfFrame();
            timer += Time.deltaTime;
        }
    }

}
