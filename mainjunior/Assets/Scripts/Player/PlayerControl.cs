using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerControl : MonoBehaviour
{
    public Rigidbody2D RB;
    public static PlayerControl instance;
    private float horizontalInput;
    [SerializeField] float moveSpeed = 10f;
    //private bool isFacingRight = true;
    //wall sliding
    //private bool isWallSliding;
    //private float wallSlidingSpeed = 2f;
    //wall jumping
    //private float _wallJumpStartTime;
	//private int _lastWallJumpDir;
    //private float wallJumpingDirection;
    //private float WallJumpingTime = 0.2f;
    //private float wallJumpingCounter;
    //private float wallJumpingDuration = 0.4f;
    //private Vector2 wallJumpingPower = new Vector2(8f, 16f);
    public Rigidbody2D player;
    bool isGrounded;
    
    
    private float wallJumpCoolDown;
    bool movingSpeed = false;
    PlatformMoving platform;
    
    public Transform groundCheckPoint;
    //public Transform stickyCheckPoint;
    
    //[SerializeField] LayerMask whatIsSticky;
    private Animator anim;
    private SpriteRenderer theSr;

    
    
    private float KnockBackCounter;

    [Header("Layers & Tags")]
    [SerializeField] LayerMask whatIsGround;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private LayerMask NormalWallLayer;

    [Header("Knockback")]
    public float KnockBackLength = 0.25f;
    public float KnockBackForce = 5;
    [Header("Jump")]
    [SerializeField] float jumpForce = 10f;
    [SerializeField] float JumpRelease = 2f;
    public bool isWallJumping;
    public static int PlayerJump = 0;
    //public float LastOnGroundTime;
    //public float LastPressedJumpTime;

    [Header("Dash")]
    [SerializeField] private float dashingVelocity = 14f;
    [SerializeField] private float dashingTime = 0.5f;
    private Vector2 dashingDir;
    private bool isDashing;
    private bool canDash = true;
    private TrailRenderer _trailRenderer;

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
        _trailRenderer = GetComponent<TrailRenderer>();
    }
    // Update is called once per frame
    void Update()
    {
        Collider2D[] colliders = Physics2D.OverlapBoxAll(transform.position, new Vector2(1,1),0, stickyWallLayer);
        foreach (Collider2D collider in colliders)
        {
            StickyWall stickyWall = collider.GetComponent<StickyWall>();
            if (stickyWall != null && stickyWall.isSticky)
            {
                RB.velocity = Vector2.zero; // set player's velocity to zero to make it stick to the wall
                break;
            }}

        if (KnockBackCounter <= 0)
        {
            isGrounded = Physics2D.OverlapCircle(groundCheckPoint.position, 0.2f, whatIsGround);

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
            if (Input.GetButtonDown("Jump") && isGrounded){
                RB.velocity = new Vector2(RB.velocity.x, jumpForce);
                PlayerJump++;
                SwitchingPlatforms.isToggle = false;
                AltSwitchingPlatforms.isToggle = false;
                print(PlayerJump);
                /*if (isGrounded)
                {

                    player.velocity = new Vector2(player.velocity.x, jumpForce);
                    player.velocity = new Vector2(player.velocity.x, Mathf.Clamp(player.velocity.y, 0f, jumpForce));
                    PlayerJump = true;
                    PlayerJump = !PlayerJump;
                }*/

            }

            if(Input.GetButtonUp("Jump") && RB.velocity.y > 0){
                RB.velocity = new Vector2(RB.velocity.x, RB.velocity.y / JumpRelease);
            }

            if(Input.GetButtonDown("Dash") && canDash){
                isDashing = true;
                canDash = false;
                _trailRenderer.emitting = true;
                dashingDir = new Vector2(Input.GetAxisRaw("Horizontal"),0);
                
                if(dashingDir == Vector2.zero){
                    dashingDir = new Vector2(transform.localScale.x, 0);
                }
                StartCoroutine(StopDashing());
            }

            //anim.SetBool("isDashing", isDashing);

            if(isDashing){
                RB.velocity = dashingDir.normalized * dashingVelocity;
                return;
            }

            if(isGrounded){
                canDash = true;
            }

            if (wallJumpCoolDown > 0.2f)
            {

                if (onWall() && !isGrounded)
                {
                    player.gravityScale = 0;
                    player.velocity = Vector2.zero;
                }
                else
                {
                    player.gravityScale = 1;
                }
            }
            else
            {
                wallJumpCoolDown += Time.deltaTime;
            }
            //wallSlide();
            //wallJump();
            // Play animations
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
        anim.SetBool("isGrounded", isGrounded);
        anim.SetFloat("moveSpeed", Mathf.Abs(player.velocity.x));

         if(!PauseMenu.isPaused){
         if (Input.GetButtonDown("Shoot")/* && cooldownTimer > attackCooldown*/){
            // Call the Shoot method
            Shoot();
            //cooldownTimer += Time.deltaTime;
            }
         }
        /*if(Input.GetButtonDown("Fire1")||Input.GetKeyDown(KeyCode.S)){
        Instantiate(ProjectilePrefab, LaunchOffset.position, transform.rotation);
        }*/
    }
    private void FixedUpdate()
    {
        if (!isWallJumping)
        {
            player.velocity = new Vector2(moveSpeed * horizontalInput, player.velocity.y);
        }
    }
    public void KnockBack()
    {
        KnockBackCounter = KnockBackLength;
        player.velocity = new Vector2(0f, KnockBackForce);
        anim.SetTrigger("hurt");
        AudioManager.instance.playSFX(1);
    }
   /* private bool isWalled()
    {
        return Physics2D.OverlapCircle(wallCheck.position, 0.2f, wallLayer);
    }*/
    /*private void wallSlide()
    {
        if (isWalled() && !isGrounded && horizontalInput != 0f)
        {
            isWallSliding = true;
            player.velocity = new Vector2(player.velocity.x, Mathf.Clamp(player.velocity.y, -wallSlidingSpeed, float.MaxValue));
        }
        else
        {
            isWallSliding = false;
        }
    }*/
    /*private void wallJump()
    {
        if (isWallSliding)
        {
            isWallJumping = false;
            wallJumpingDirection = -transform.localScale.x;
            wallJumpingCounter = WallJumpingTime;

            CancelInvoke(nameof(stopWallJumping));
        }
        else
        {
            wallJumpingCounter -= Time.deltaTime;
        }

        if (Input.GetButtonDown("Jump") && wallJumpingCounter > 0f)
        {
            isWallJumping = true;
            player.velocity = new Vector2(wallJumpingDirection * wallJumpingPower.x, wallJumpingPower.y);


            Invoke(nameof(stopWallJumping), wallJumpingDuration);
        }
    }
    private void stopWallJumping()
    {
        isWallJumping = false;
    }*/

    /*private void Jump()
	{
		LastPressedJumpTime = 0;
		LastOnGroundTime = 0;

		float force = jumpForce;
		if (RB.velocity.y < 0)
			force -= RB.velocity.y;

		RB.AddForce(Vector2.up * force, ForceMode2D.Impulse);
	}*/

    void Shoot()
    {
    	/*Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    	Vector2 direction = ((Vector2)mousePos - (Vector2)transform.position).normalized;
    	GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
    	

		if((mousePos.x - transform.position.x) > 0){  //no mouse bro

			hitmap.SendMessage("SetBulletDirectionRight");
		}
		else if((mousePos.x - transform.position.x) < 0){

			hitmap.SendMessage("SetBulletDirectionLeft");

		}


		bullet.GetComponent<Rigidbody2D>().velocity = direction * 7f;*/
        //anim.SetTrigger("shoot");
        //cooldownTimer = 0;

        //slimeballs[0].transform.position = slimePoint.position;
        //slimeballs[0].GetComponent<StickyBullet>().SetDirection(Mathf.Sign(transform.localScale.x));

        //Vector3 rotatedOffset = LaunchOffset.rotation * LaunchOffset.position;

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
        Instantiate(bulletpre, LaunchOffset.position/* + rotatedOffset*/, transform.rotation);

        
        
    }

    private IEnumerator StopDashing(){
        yield return new WaitForSeconds(dashingTime);
        _trailRenderer.emitting = false;
        isDashing = false;
    }

    private bool onWall()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, new Vector2(transform.localScale.x, 0), 0.1f, NormalWallLayer);
        return raycastHit.collider != null;
    }
    public bool canAttack()
    {
        return horizontalInput == 0 && isGrounded && !onWall();

    }
    /*private bool isGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.down, 0.1f, GroundLayer);
        return raycastHit.collider != null;
    }*/

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