using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Loadch2 : MonoBehaviour
{
    GameObject player;
    Animation anim;
    Rigidbody2D playerRB;
    Animator anima;
    //public SpriteRenderer theSr;
    // Start is called before the first frame update

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        anim = player.GetComponent<Animation>();
        //anima = player.GetComponent<Animator>();
        playerRB = player.GetComponent<Rigidbody2D>();
    }
    void Start()
    {

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


        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
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
