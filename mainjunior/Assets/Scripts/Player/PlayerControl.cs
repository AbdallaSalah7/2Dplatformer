using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerControl : MonoBehaviour
{

    public Rigidbody2D RB;
    bool movingSpeed = false;
    PlatformMoving platform;
    public static PlayerControl instance;
    private float horizontalInput;
    [SerializeField] float moveSpeed = 10f;
    //private bool isFacingRight = true;

    public Rigidbody2D player;
    // bool isGrounded;

    private float wallJumpCoolDown;


    public Transform groundCheckPoint;
    //public Transform stickyCheckPoint;

    //[SerializeField] LayerMask whatIsSticky;
    private Animator anim;
    private SpriteRenderer theSr;



    private float KnockBackCounter;

    [Header("Layers & Tags")]
    [SerializeField] LayerMask GroundLayer;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private LayerMask NormalWallLayer;
    [SerializeField] private Transform wallCheck;
    [Header("Knockback")]
    public float KnockBackLength = 0.25f;
    public float KnockBackForce = 5;
    [Header("Jump")]
    [SerializeField] float jumpForce;
    //public bool canDoubleJump;
    //public bool isWallJumping;



    [Header("Sticky&bullet")]
    //[SerializeField] private float attackCooldown;
    //[SerializeField] private Transform slimePoint;
    //[SerializeField] private GameObject[] slimeballs;
    //private float cooldownTimer = Mathf.Infinity;
    public LayerMask stickyWallLayer;
    //public GameObject bulletPrefab;
    public Tilemap hitmap;
    public StickyBullet bulletpre;
    public Transform LaunchOffset;

    private BoxCollider2D boxCollider;
    // Start is called before the first frame update
    private void Awake()
    {
        instance = this;
        AudioManager.instance.playSFX(5);
        boxCollider = GetComponent<BoxCollider2D>();
        //platform = GetComponent<PlatformMoving>();

    }
    void Start()
    {
        RB = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        theSr = GetComponent<SpriteRenderer>();
    }
    // Update is called once per frame
    void Update()
    {

        Collider2D[] colliders = Physics2D.OverlapBoxAll(transform.position, new Vector2(1, 1), 0, stickyWallLayer);
        foreach (Collider2D collider in colliders)
        {
            StickyWall stickyWall = collider.GetComponent<StickyWall>();
            if (stickyWall != null && stickyWall.isSticky)
            {
                RB.velocity = Vector2.zero; // set player's velocity to zero to make it stick to the wall
                break;
            }
        }
        if (KnockBackCounter <= 0)
        {
            horizontalInput = Input.GetAxis("Horizontal");
            
            player.velocity = new Vector2(moveSpeed * horizontalInput, player.velocity.y);

            // Flip player sprite depending on direction
            if (player.velocity.x < 0)
            {
                theSr.flipX = true;
            }
            else if (player.velocity.x > 0)
            {
                theSr.flipX = false;
            }
            // Jumping
            if (Input.GetButtonDown("Jump"))
            {
                if (isGrounded())
                {

                    player.velocity = new Vector2(player.velocity.x, jumpForce);
                    player.velocity = new Vector2(player.velocity.x, Mathf.Clamp(player.velocity.y, 0f, jumpForce));
                }
            }
            //wall jump logic
            if (wallJumpCoolDown > 0.2f)
            {

                if (onWall() && !isGrounded())
                {
                    player.gravityScale = 0;
                    player.velocity = Vector2.zero;
                }
                else
                {
                    player.gravityScale = 1;
                }

                if (Input.GetButtonDown("Jump"))
                {
                    jump();
                }
            }
            else
            {
                wallJumpCoolDown += Time.deltaTime;
            }
        }
        else
        {
            KnockBackCounter -= Time.deltaTime;
            if (!theSr.flipX)
            {
                player.velocity = new Vector2(-KnockBackForce, player.velocity.y);
            }
            else
            {
                player.velocity = new Vector2(KnockBackForce, player.velocity.y);
            }
        }
        anim.SetBool("isGrounded", isGrounded());
        anim.SetFloat("moveSpeed", Mathf.Abs(player.velocity.x));

        if (Input.GetKeyDown(KeyCode.S)/* && cooldownTimer > attackCooldown*/)
        {
            // Call the Shoot method
            Shoot();
            //cooldownTimer += Time.deltaTime;
        }
        /*if(Input.GetButtonDown("Fire1")||Input.GetKeyDown(KeyCode.S)){
            Instantiate(ProjectilePrefab, LaunchOffset.position, transform.rotation);
        }*/
    }
    private void jump()
    {
        if (isGrounded())
        {
            player.velocity = new Vector2(player.velocity.x, jumpForce);
            player.velocity = new Vector2(player.velocity.x, Mathf.Clamp(player.velocity.y, 0f, jumpForce));
        }
        else if (onWall() && !isGrounded())
        {
            if (horizontalInput == 0)
            {
                player.velocity = new Vector2(Mathf.Sign(transform.localScale.x) * 10, 0);
                transform.localScale = new Vector3(Mathf.Sign(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
            else
            {
                player.velocity = new Vector2(Mathf.Sign(transform.localScale.x) * 3, 6);
            }
            wallJumpCoolDown = 0;
        }
    }

    public void KnockBack()
    {
        KnockBackCounter = KnockBackLength;
        player.velocity = new Vector2(0f, KnockBackForce);
        anim.SetTrigger("hurt");
        AudioManager.instance.playSFX(1);
    }


    void Shoot()
    {

        //Vector3 rotatedOffset = LaunchOffset.rotation * LaunchOffset.position;
        Instantiate(bulletpre, LaunchOffset.position/* + rotatedOffset*/, transform.rotation);

        if (!theSr.flipX)
        {
            hitmap.SendMessage("SetBulletDirectionRight");
            bulletpre.dir = true;
        }
        else
        {
            hitmap.SendMessage("SetBulletDirectionLeft");
            bulletpre.dir = false;
        }


    }

    private bool isGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.down, 0.1f, GroundLayer);
        return raycastHit.collider != null;
    }
    private bool onWall()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, new Vector2(transform.localScale.x, 0), 0.1f, NormalWallLayer);
        return raycastHit.collider != null;
    }
    public bool canAttack()
    {
        return horizontalInput == 0 && isGrounded() && !onWall();

    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("PlatformMechanics"))
        {
            print("initial velocity "+player.velocity.x );
            print("");
            player.velocity = new Vector2( (player.velocity.x + PlatformMoving.instance.speed), 0);
            print("Velocity after adding "+player.velocity.x);
            print(player.velocity.x + " " +PlatformMoving.instance.speed );
          
        }
    }
    private void OnCollisionExit2D(Collision2D other)
    {
        
    }



}