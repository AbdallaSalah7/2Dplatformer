using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerControl : MonoBehaviour
{
    public Rigidbody2D RB;
    public static PlayerControl instance;
    private float horizontalInput;

    //GiveJump
    public Rigidbody2D player;
    bool isGrounded;
    
    
    private float wallJumpCoolDown;
    //bool movingSpeed = false;
    public MovingPlatform MovePlatform;
    
    public Transform groundCheckPoint;
    //public Transform stickyCheckPoint;
    
    //[SerializeField] LayerMask whatIsSticky;
    private Animator anim;
    private SpriteRenderer theSr;

    
    //------------------------------------CHECK VARIABLES------------------------------------------------
    //bool variables for checks if a certain action is possible 
    public bool IsFacingRight { get; private set; }
    public bool IsRunning { get; private set; }

    //----------------------------------TIMERS------------------------------------------------
    //used for coyote time check. It is set to coyote time when player is grounded, otherwise when jumping or in air it starts countdown by Time.deltaTime each frame
    public float LastOnGroundTime { get; private set; }

    //----------------------------------------ASSISTS------------------------------------------------
    [Header("Assists")]
	[Range(0.01f, 0.5f)] public float coyoteTime = 0.2f; //Grace period after falling off a platform, where you can still jump
	[Range(0.01f, 0.5f)] public float jumpInputBufferTime = 0.2f; //Grace period after pressing jump where a jump will be automatically performed once the requirements (eg. being grounded) are met.

    private float KnockBackCounter;

    [Header("Layers & Tags")]
    [SerializeField] LayerMask whatIsGround;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private LayerMask NormalWallLayer;

    [Header("Knockback")]
    public float KnockBackLength = 0.25f;
    public float KnockBackForce = 5;


    [Header("run")]
    public float runMaxSpeed = 4.5f;
    [HideInInspector] public float runAccelAmount;
    public float runAcceleration = 4.5f;
    [HideInInspector] public float runDeccelAmount;
    public float runDecceleration = 4.5f;
    [Range(0f, 1)] public float accelInAir = 1f;
    [Range(0f, 1)] public float deccelInAir = 1f;
	public bool doConserveMomentum = true;
    private Vector2 _moveInput;



    [Header("Jump")]
    [SerializeField] float jumpForce = 10f;
    [SerializeField] float JumpRelease = 2f;
    public bool isWallJumping;
    private float wallJumpingDirection;
    private float WallJumpingTime = 0.2f;
    private float wallJumpingCounter;
    private float wallJumpingDuration = 0.4f;
    private Vector2 wallJumpingPower = new Vector2(4f, 8f);
    public static int PlayerJump = 0;



    [Header("Wall Slide")]
    [SerializeField] Transform wallPoint;
    [SerializeField] LayerMask slideWall;
    public bool isWallSliding;
    public float wallSlidingSpeed = 2f;



    [Header("Dash")]
    [SerializeField] private float dashingVelocity = 14f;
    [SerializeField] private float dashingTime = 0.5f;
    private Vector2 dashingDir;
    private bool isDashing;
    private bool canDash = true;
    private TrailRenderer _trailRenderer;
    private bool isStickjump;



    [Header("Sticky&bullet")]
    public LayerMask stickyWallLayer;
    public Tilemap hitmap;
    public StickyBullet bulletpre;
    public StickyWall _stickywall; 
    public Transform LaunchOffset;

    private BoxCollider2D boxCollider;
    private Vector2 stickjump;
    public bool outOfStickJump = true;
    [SerializeField] private float stickjumpVelocity = 14f;


    
    // Start is called before the first frame update
    private void Awake()
    {
        RB = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        theSr = GetComponent<SpriteRenderer>();
        _trailRenderer = GetComponent<TrailRenderer>();
        instance = this;
        AudioManager.instance.playSFX(5);
        boxCollider = GetComponent<BoxCollider2D>();
        //platform = GetComponent<PlatformMoving>();
        
    }
    void Start()
    {
        IsFacingRight = true;
        runAccelAmount = (50 * runAcceleration) / runMaxSpeed;
        runDeccelAmount = (50 * runDecceleration) / runMaxSpeed;
        runAcceleration = Mathf.Clamp(runAcceleration, 0.01f, runMaxSpeed);
		runDecceleration = Mathf.Clamp(runDecceleration, 0.01f, runMaxSpeed);
    }//LasoOngroundtime
    // Update is called once per frame
    void Update()
    {
        LastOnGroundTime -= Time.deltaTime;
        _moveInput.x = Input.GetAxisRaw("Horizontal");

        
        //--------------------------------------------------------STICKY-------------------------------------------------------------
        Collider2D[] colliders = Physics2D.OverlapBoxAll(transform.position, new Vector2(1,1),0, stickyWallLayer); //we try to make bigger collider? or smoller idk
        foreach (Collider2D collider in colliders)
        {
            StickyWall stickyWall = collider.GetComponent<StickyWall>();
            if (stickyWall != null && stickyWall.isSticky)
            {
                //_moveInput.x = 0;
                RB.velocity = Vector2.zero; // set player's velocity to zero to make it stick to the wall it
                    break;
            }}

            


        if (KnockBackCounter <= 0)
        {
            //If moving left or right
            if (_moveInput.x != 0) {

			    CheckDirectionToFace(_moveInput.x > 0);
            }

            //check if player is grounded
            isGrounded = Physics2D.OverlapCircle(groundCheckPoint.position, 0.2f, whatIsGround);

            if(isGrounded) 
                LastOnGroundTime = coyoteTime; //if so sets the lastGrounded to coyoteTime



            //horizontalInput = Input.GetAxis("Horizontal");

            //player.velocity = new Vector2(5f * _moveInput.x, player.velocity.y);

            // Flip player sprite depending on direction
            /* if (player.velocity.x < 0)
            {
                
                theSr.flipX = true;  //negative -> left
            }
            else if (player.velocity.x > 0)
            {
                theSr.flipX = false;
            } */

            Jump();
            SlimeWallSlide();
            WallJump();



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


            //if (!theSr.flipX)
            if (IsFacingRight)
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
         if(isStickjump){

            RB.velocity = stickjump.normalized * stickjumpVelocity;
            return;
        }
        /*if(Input.GetButtonDown("Fire1")||Input.GetKeyDown(KeyCode.S)){
        Instantiate(ProjectilePrefab, LaunchOffset.position, transform.rotation);
        }*/
    }




    private void FixedUpdate()
    {
        /* if (!isWallJumping)
        {
            player.velocity = new Vector2(6f * _moveInput.x, player.velocity.y);
        } */

        if(!isWallJumping){

            Run(1);
        }
        
    }




    public void KnockBack()
    {
        KnockBackCounter = KnockBackLength;
        player.velocity = new Vector2(0f, KnockBackForce);
        anim.SetTrigger("hurt");
        AudioManager.instance.playSFX(1);
    }



    public void Jump(){

        // Jumping
            if (Input.GetButtonDown("Jump") && isGrounded){

                LastOnGroundTime = 0;

                //RB.velocity = new Vector2(RB.velocity.x, jumpForce); 
                //RB.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);

                if (RB.velocity.y < 0)
			        jumpForce -= RB.velocity.y;

		        RB.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);

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
    private void WallJump()
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
            isWallJumping = false;
        }

        if (Input.GetButtonDown("Jump") && wallJumpingCounter > 0f)
        {
            isWallJumping = true;
            player.velocity = new Vector2(wallJumpingDirection * wallJumpingPower.x, wallJumpingPower.y);
            wallJumpingCounter = 0f;

            if(transform.localScale.x != wallJumpingDirection){

                IsFacingRight = !IsFacingRight;
                Vector3 localScale = transform.localScale;
                localScale.x *= -1f;
                transform.localScale = localScale;
            }

            Invoke(nameof(stopWallJumping), wallJumpingDuration);
        }
    }
    private void stopWallJumping()
    {
        isWallJumping = false;
    }

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


        if (IsFacingRight) 
            {
                hitmap.SendMessage("SetBulletDirectionRight");
                bulletpre.dir = true;
            }
            else
            {
                hitmap.SendMessage("SetBulletDirectionLeft");
                bulletpre.dir = false;
            }
        Instantiate(bulletpre, LaunchOffset.position + new Vector3(0.6f, 0, 0)/* + rotatedOffset*/, transform.rotation);
        
    }

    public void GiveJump(){
        print("sent");
        Jump();
        isStickjump = true;
        stickjump = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        
        isStickjump = false;
    }



    private bool isSlimeWalled(){

        return Physics2D.OverlapCircle(wallPoint.position, 0.2f, wallLayer);
    }



    private void SlimeWallSlide(){
        if(isSlimeWalled() && !isGrounded && _moveInput.x != 0f){

            isWallSliding = true;
            player.velocity = new Vector2(player.velocity.x, Mathf.Clamp(player.velocity.y, -wallSlidingSpeed, float.MaxValue));
        }
        else{

            isWallSliding = false;
        }

        //print("wallslide now " + isWallSliding + " " + isWallJumping);
    }




    /* public bool CanJumpFromSticky(){

        return isSticking=
    } */


    private IEnumerator StopDashing(){
        yield return new WaitForSeconds(dashingTime);
        _trailRenderer.emitting = false;
        isDashing = false;
    }


    private void Run(float lerpAmount)
    {
        //Calculate the direction we want to move in and our desired velocity
        float targetSpeed = _moveInput.x * runMaxSpeed;

        //We can reduce are control using Lerp() this smooths changes to are direction and speed
        //linearly interpolate between the current x velocity and target speed by lerpAmount, which is value in range [0, 1]
        targetSpeed = Mathf.Lerp(RB.velocity.x, targetSpeed, lerpAmount); //lerp = 1f


        //Calculate AccelRate
        float accelRate;
        
        //Gets an acceleration value based on if we are accelerating (includes turning) 
		//or trying to decelerate (stop). As well as applying a multiplier if we're air borne (hung in air).
        if(LastOnGroundTime > 0){
            accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? runAccelAmount : runDeccelAmount;
        }
        else{
			accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? runAccelAmount * accelInAir : runDeccelAmount * deccelInAir;
        }
        
        //Conserve Momentum
		//We won't slow the player down if they are moving in their desired direction but at a greater speed than their maxSpeed
        if(doConserveMomentum && Mathf.Abs(RB.velocity.x) > Mathf.Abs(targetSpeed) && Mathf.Sign(RB.velocity.x) == Mathf.Sign(targetSpeed) && Mathf.Abs(targetSpeed) > 0.01f && LastOnGroundTime < 0)
		{
			accelRate = 0; 
		}

        //Calculate difference between current velocity and desired velocity
        float speedDif = targetSpeed - RB.velocity.x;

        //Calculate force along x-axis to apply to thr player
		float movement = speedDif * accelRate;

        //Convert this to a vector and apply to rigidbody
		RB.AddForce(movement * Vector2.right, ForceMode2D.Force);
    }
    



    public void CheckDirectionToFace(bool isMovingRight)
	{
		if (isMovingRight != IsFacingRight && !isWallJumping){

            //stores scale and flips the player along the x axis, 
		    Vector3 scale = transform.localScale; 
		    scale.x *= -1;
		    transform.localScale = scale; //this flips the bakka sprite

		    IsFacingRight = !IsFacingRight;
        }
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
            //print("initial velocity "+player.velocity.x );
            //print("");
            //if(_moveInput.x != 0)
            //    player.velocity = new Vector2( (player.velocity.x + MovePlatform.moveSpeed), 0);

            //player.velocity = new Vector2( (player.velocity.x + PlatformMoving.instance.speed), 0);

            //print("Velocity after adding "+player.velocity.x);
            //print(player.velocity.x + " " +PlatformMoving.instance.speed );
          
        }
    }
    private void OnCollisionExit2D(Collision2D other)
    {
        

    }
    


}