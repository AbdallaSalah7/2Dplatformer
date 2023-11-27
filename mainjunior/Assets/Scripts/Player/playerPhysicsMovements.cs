using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEditorInternal.Profiling;
using UnityEngine;
using UnityEngine.Tilemaps;

public class playerPhysicsMovements : MonoBehaviour
{

    #region COMPONENTS
    public Rigidbody2D RB;
	public Animator anim;
	private SpriteRenderer theSr;
	public static playerPhysicsMovements instance;
	#endregion
	
	private bool playerInside = false;
	public GameObject frame245;

	//------------------------------------CHECK VARIABLES------------------------------------------------
	//bool variables for checks if a certain action is possible 
	public bool IsFacingRight { get; private set; }
	public bool IsJumping { get; private set; }
    public bool IsRunning { get; private set; }
	public bool IsWallJumping { get; private set; }
	public bool IsSliding { get; private set; }
    private bool _isJumpCut;
	private bool _isJumpFalling;


    //----------------------------------TIMERS------------------------------------------------
    //used for coyote time check. It is set to coyote time when player is grounded, otherwise when jumping or in air it starts countdown by Time.deltaTime each frame
    public float LastOnGroundTime { get; private set; }
	public float LastOnWallTime { get; private set; }
	public float LastOnWallRightTime { get; private set; }
	public float LastOnWallLeftTime { get; private set; }

    //used for jump buffering check. It is set to buffer time threshold when pressing jump key, otherwise is decremented each frame by Time.deltaTime
    public float LastPressedJumpTime { get; private set; }


	//references for explanation:
    //https://youtu.be/RFix_Kg2Di0
    //Coyote time: allow player to jump, for short amount of time after leaving the ground
    //Jump buffering: buffers player's jump and executes as soon as they touch the ground within short amount of time,
    //which allows player to jump before complete landing on ground



    //--------------------------------------RUN VARIABLES-----------------------------------------------------
    [Header("Run")]
	public float runMaxSpeed = 4.5f; //Target speed we want the player to reach.
	public float runAcceleration = 4.5f; //The speed at which our player accelerates to max speed, can be set to runMaxSpeed for instant acceleration down to 0 for none at all
	[HideInInspector] public float runAccelAmount; //The actual force (multiplied with speedDiff) applied to the player.
	public float runDecceleration = 4.5f; //The speed at which our player decelerates from their current speed, can be set to runMaxSpeed for instant deceleration down to 0 for none at all
	[HideInInspector] public float runDeccelAmount; //Actual force (multiplied with speedDiff) applied to the player .
	[Space(5)]
	[Range(0f, 1)] public float accelInAir = 1f; //Multipliers applied to acceleration rate when airborne.
	[Range(0f, 1)] public float deccelInAir = 1f;
	[Space(5)]
	public bool doConserveMomentum = true;




    //--------------------------------------JUMP VARIABLES-------------------------------------------------
	[Header("Jump")]
    public float jumpHeight = 3f; //Height of the player's jump
	public float jumpTimeToApex = 0.5f; //Time between applying the jump force and reaching the desired jump height. These values also control the player's gravity and jump force.
	public float jumpForce = 12f; //The actual force applied (upwards) to the player when they jump.
	private bool isGrounded;
	public static int playerJumpCounter;


    public float jumpHangTimeThreshold = 0f; //Speeds (close to 0) where the player will experience extra "jump hang". The player's velocity.y is closest to 0 at the jump's apex (think of the gradient of a parabola or quadratic function)
    public float jumpCutGravityMult = 3.5f;  //Multiplier to increase gravity if the player releases the jump button while still jumping
    [Range(0f, 1)] public float jumpHangGravityMult = 1f;  //Reduces gravity while close to the apex (desired max height) of the jump
    public float jumpHangAccelerationMult = 1f; 
	public float jumpHangMaxSpeedMult = 1f; 
	




    //----------------------------------------INPUT---------------------------------------
    private Vector2 moveInput;
	

    //--------------------------------------GRAVITY-----------------------------------------------
	[Header("Gravity")]
    [SerializeField] public float gravity = 18f;
    public float gravityStrength;
    [SerializeField] public float fastFallGravityMult = 2f;
    [SerializeField] public float fallGravityMult = 2f;
    [SerializeField] public float maxFastFallSpeed = 18f;
    [SerializeField] public float maxFallSpeed = 18f;




    //----------------------------------------ASSISTS------------------------------------------------
    [Header("Assists")]
	[Range(0.01f, 0.5f)] public float coyoteTime = 0.2f; //Grace period after falling off a platform, where you can still jump
	[Range(0.01f, 0.5f)] public float jumpInputBufferTime = 0.2f; //Grace period after pressing jump where a jump will be automatically performed once the requirements (eg. being grounded) are met.




    //--------------------------------------PLAYER CHECKS---------------------------------------------
    [Header("Checks")] 
	[SerializeField] private Transform groundCheckPoint;
	//Size of groundCheck depends on the size of your character generally you want them slightly small than width (for ground) and height (for the wall check)
	[HideInInspector][SerializeField] private Vector2 groundCheckSize = new Vector2(0.49f, 0.03f);
	[Space(5)]
	[SerializeField] private Transform frontWallCheckPoint;
	[SerializeField] private Transform backWallCheckPoint;
	[SerializeField] private Vector2 wallCheckSize = new Vector2(0.5f, 1f);



    //------------------------------------------LAYERS-------------------------------------------------
    [Header("Layers & Tags")]
	[SerializeField] private LayerMask _groundLayer;
    public LayerMask stickyWallLayer;
	public Tilemap hitmap;


	//--------------------------------------PLATFORM MECHANICS--------------------------------------------
	[Header("Mechanics")]
	[SerializeField] private MovingPlatform MovePlatform;
	private SwitchingPlatforms[] switches;
    private AltSwitchingPlatforms[] altswitches;
	public ParticleSystem dust;
	
	[Space(5)]

	//------------------------------------------STICKY-----------------------------------------------------
	[Header("Sticky")]
	[SerializeField] private float stickjumpVelocity = 14f;
    [SerializeField] bool ch2belowjump;
	public GameObject bulletPrefab;
	public StickyBullet bulletpre;
    public StickyWall _stickywall;
    public Transform LaunchOffset;
	private Vector2 stickjump;
    public bool playSticky;
    public bool outOfStickJump = true;
	public bool jumpboost;
	public float slideSpeed;
	public float slideAccel;

	[Space(5)]


	//---------------------------------------------DASH-----------------------------------------------------
	[Header("Dash")]
	[SerializeField] private float dashingVelocity = 14f;
    [SerializeField] private float dashingTime = 0.5f;
    private Vector2 dashingDir;
    private bool isDashing;
    private bool canDash = true;
    private TrailRenderer _trailRenderer;
    private bool isStickjump;

	//-----------------------------LETTERS----------------------------//
	[Header("Visual Cue")]
	[SerializeField] private GameObject visualCue;

	//--------------------------------------INITIAL FUNCTIONS-----------------------------------------------
	private void Awake()
	{
		RB = GetComponent<Rigidbody2D>();
		anim = GetComponent<Animator>();
		theSr = GetComponent<SpriteRenderer>();
        _trailRenderer = GetComponent<TrailRenderer>();//and here jump? so jump is h okay
        instance = this;
	}


	private void Start()
	{
		//Ensuring important variables are initialized 
		IsFacingRight = true;
		playSticky = false;
		isGrounded = true;
		playerJumpCounter = 0;

		//Physics calculations for gravity, run and jump
        gravityStrength = -(2 * jumpHeight) / (jumpTimeToApex * jumpTimeToApex);
        gravity = gravityStrength / Physics2D.gravity.y;

        runAccelAmount = (50 * runAcceleration) / runMaxSpeed;
        runDeccelAmount = (50 * runDecceleration) / runMaxSpeed;

        jumpForce = Mathf.Abs(gravityStrength) * jumpTimeToApex;

        runAcceleration = Mathf.Clamp(runAcceleration, 0.01f, runMaxSpeed);
		runDecceleration = Mathf.Clamp(runDecceleration, 0.01f, runMaxSpeed);

        
        SetGravityScale(gravity);

		//Initializing arrays of Switching platforms available in the scene
		switches = Object.FindObjectsOfType<SwitchingPlatforms>();

        altswitches = Object.FindObjectsOfType<AltSwitchingPlatforms>();
	}



    // Update is called once per frame
    void Update()
    {
		if (playerInside && Input.GetButton("Interact"))
		{
			// Activate the "Frame 245" object
			if (frame245 != null)
			{
				frame245.SetActive(true);
			}
		}
		if (!playerInside)
		{
			// Activate the "Frame 245" object
			
				frame245.SetActive(false);
			
		}
		//Check game paused
		if (PauseMenu.isPaused){
			return;
		}


		//Timers update------------------------------------------------------
        LastOnGroundTime -= Time.deltaTime;
		LastOnWallTime -= Time.deltaTime;
		LastOnWallRightTime -= Time.deltaTime;
		LastOnWallLeftTime -= Time.deltaTime;
		LastPressedJumpTime -= Time.deltaTime;


        //Sticky code here
		//Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 0.1f, stickyWallLayer);
		Collider2D[] colliders = Physics2D.OverlapBoxAll(transform.position, new Vector2(1, 1), 0, stickyWallLayer);
        foreach (Collider2D collider in colliders)
        {
            StickyWall stickyWall = collider.GetComponent<StickyWall>();
            /* if (stickyWall != null && stickyWall.isSticky)
            {
				//print("test");
                RB.velocity = Vector2.zero; // set player's velocity to zero to make it stick to the wall sure
                break; 
			} */
		}

		//COLLECT ALL INPUTS----------------------------
        CollectInput();


        //COLLISIONS CHECK------------------------------
		CollisionChecks();



        //JUMP CHECKS-----------------------------------
		JumpCheck();



		//SLIDE CHECK-------------------------------------
		SlideCheck();



        //GRAVITY CHECKS------------------------------------
		GravityCheck();



		//ANIMATION CHECKS----------------------------------
		AnimCheck();

    }



    private void FixedUpdate(){

        Run(1);

		if (IsSliding)
			Slide();

		if(InkDialogueManager.GetInstance().dialogueIsPlaying)
		{
			//Freeze player movement
			return;
		}
    }


    private void LateUpdate() {

        //set animations
        //anim.SetBool("isRunning", InAirPlayer() ? false : IsRunning);
        //anim.SetBool("isJumping", InAirPlayer());
    }



    //---------------------------------------FUNCTIONS--------------------------------------
    public void SetGravityScale(float scale)
	{
		RB.gravityScale = scale;
	}


    public void CheckDirectionToFace(bool isMovingRight)
	{
		if (isMovingRight != IsFacingRight){

            //stores scale and flips the player along the x axis, 
		    Vector3 scale = transform.localScale; 
		    scale.x *= -1;
		    transform.localScale = scale;

		    IsFacingRight = !IsFacingRight;
        }
	}


    //---------------------------------CHECKS IF ENABLE MOVEMENTS---------------------------------------------------------
    public bool CanRun() {

		return !IsJumping && !_isJumpFalling;
	}

    private bool CanJump()
    {
		return LastOnGroundTime > 0 && !IsJumping;
    }

    public bool InAirPlayer() {

		return IsJumping || _isJumpFalling; 
	}

	public bool CanSlide()
    {
		if (!IsJumping && LastOnGroundTime <= 0)
			return true;
		else
			return false;
	}


    

	//----------------------------------MAIN CHECK FUNCTIONS IN UPDATE-----------------------------------------------------
    private void CollectInput(){

        moveInput.x = Input.GetAxisRaw("Horizontal");
		moveInput.y = Input.GetAxisRaw("Vertical");

		if (moveInput.x != 0) {

			CheckDirectionToFace(moveInput.x > 0);

			if(CanRun()){
				IsRunning = true;
			}
		    }
		    else {

			    IsRunning = false;
		    }


        //Check jump press down
        if(Input.GetButtonDown("Jump"))
        {
			LastPressedJumpTime = jumpInputBufferTime;
        }

        //check jump release
         if (Input.GetButtonUp("Jump"))
		 {
		 	if (IsJumping && RB.velocity.y > 0)
		 	    _isJumpCut = true;
		 }

		//Check below shooting input
        if (Input.GetButtonDown("Shoot") && !isGrounded && ch2belowjump && jumpboost  && (Input.GetButton("Vertical") || Input.GetAxis("Vertical") < 0) && Input.GetAxisRaw("Vertical") < Mathf.Epsilon)
            {

                // Call the Shoot method
                jumpboost = false;
                //belowshoot = true;
                Shootbelow();
                AudioManager.instance.playSFX(3);
                //belowshoot = false;  
            }

		//Check shooting input 
		else if (Input.GetButtonDown("Shoot")&& !Input.GetButton("Vertical"))
            {

                // Call the Shoot method
                Shoot();
                AudioManager.instance.playSFX(3);
            }

		//check dashing
		if (Input.GetButtonDown("Dash") && canDash)
            {
                isDashing = true;
                canDash = false;
                _trailRenderer.emitting = true;
				Dash();
            }
		
		//Dashing velocity
		if (isDashing)
            {
                RB.velocity = dashingDir.normalized * dashingVelocity;
                return;
            }

    }//end of collectInput()



	public void CollisionChecks(){

		if(!IsJumping){

            if (Physics2D.OverlapBox(groundCheckPoint.position, groundCheckSize, 0, _groundLayer) && !IsJumping) //checks if set box overlaps with ground
			{

				LastOnGroundTime = coyoteTime; //if so sets the lastGrounded to coyoteTime
				jumpboost = true;
				canDash = true;
				isGrounded = true;
            }
        }
		else{

			isGrounded = false;
		}
	}




	public void JumpCheck(){

		if (IsJumping && RB.velocity.y < 0){

            IsJumping = false;
            _isJumpFalling = true;
        }
        
        if (LastOnGroundTime > 0 && !IsJumping)  //if within coyote time
        {
			_isJumpCut = false;

			if(!IsJumping)
				_isJumpFalling = false;
		}

        //Jump
			if (CanJump() && LastPressedJumpTime > 0)  //if within buffer thershold
			{
				IsJumping = true;
				_isJumpCut = false;
				_isJumpFalling = false;
				Jump();

			}
	}

	public void SlideCheck(){
		
		if (CanSlide() && Physics2D.OverlapBox(frontWallCheckPoint.position, wallCheckSize, 0, stickyWallLayer)) 
			IsSliding = true;
		else
			IsSliding = false;
	}


	//Dash
	public void Dash(){

		dashingDir = new Vector2(Input.GetAxisRaw("Horizontal"), 0);

                if (dashingDir == Vector2.zero)
                {
                    dashingDir = new Vector2(transform.localScale.x, 0);
                }
                StartCoroutine(StopDashing());
	}


	//Start coroutine to fade away dash 
	public IEnumerator StopDashing(){

		yield return new WaitForSeconds(dashingTime);
        _trailRenderer.emitting = false;
        isDashing = false;
	}



	public void GravityCheck(){

		if (IsSliding)
		{
			SetGravityScale(0);
		}
			else if (RB.velocity.y < 0 && moveInput.y < 0)
			{
                //runAccelAmount;
				//Much higher gravity if holding down
				SetGravityScale(gravity * fastFallGravityMult);
				//Caps maximum fall speed, so when falling over large distances we don't accelerate to insanely high speeds
				RB.velocity = new Vector2(RB.velocity.x, Mathf.Max(RB.velocity.y, -maxFastFallSpeed));
			}
			else if (_isJumpCut)
			{
				//Higher gravity if jump button released
				SetGravityScale(gravity * jumpCutGravityMult);
				RB.velocity = new Vector2(RB.velocity.x, Mathf.Max(RB.velocity.y, -maxFallSpeed));
			}
			else if ((IsJumping || _isJumpFalling) && Mathf.Abs(RB.velocity.y) < jumpHangTimeThreshold)
			{
				SetGravityScale(gravity * jumpHangGravityMult);
			}
			else if (RB.velocity.y < 0)
			{
				//Higher gravity if falling
				SetGravityScale(gravity * fallGravityMult);
				//Caps maximum fall speed, so when falling over large distances we don't accelerate to insanely high speeds
				RB.velocity = new Vector2(RB.velocity.x, Mathf.Max(RB.velocity.y, -maxFallSpeed));
			}
			else
			{
				//Default gravity if standing on a platform or moving upwards
				SetGravityScale(gravity); 
			}
	}


	//Animations 
	public void AnimCheck(){
		
		//Jump animation
		anim.SetBool("isGrounded", isGrounded);
		
		//Run animation
		//anim.SetBool("isRunning", InAirPlayer() ? false : IsRunning);
        anim.SetFloat("moveSpeed", Mathf.Abs(RB.velocity.x));
	}


	//Create dust particles
	void createDust()
    {
        dust.Play();
    }



	//--------------------------------------------------------ACTION MOVEMENTS-----------------------------------------------------------------------
    private void Run(float lerpAmount)
	{
		//Calculate the direction we want to move in and our desired velocity
		float targetSpeed = moveInput.x * runMaxSpeed;
		//We can reduce are control using Lerp() this smooths changes to are direction and speed
		targetSpeed = Mathf.Lerp(RB.velocity.x, targetSpeed, lerpAmount);

		#region Calculate AccelRate
		float accelRate;

		//Gets an acceleration value based on if we are accelerating (includes turning) 
		//or trying to decelerate (stop). As well as applying a multiplier if we're air borne (hung in air).
		if (LastOnGroundTime > 0)  //if within coyote
			accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? runAccelAmount : runDeccelAmount;
		else
			accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? runAccelAmount * accelInAir : runDeccelAmount * deccelInAir;
		#endregion

		#region Add Bonus Jump Apex Acceleration
		//Increase are acceleration and maxSpeed when at the apex of their jump, makes the jump feel a bit more bouncy, responsive and natural
		if ((IsJumping || _isJumpFalling) && Mathf.Abs(RB.velocity.y) < jumpHangTimeThreshold)
		{
			accelRate *= jumpHangAccelerationMult;
			targetSpeed *= jumpHangMaxSpeedMult;
		}
		#endregion

		#region Conserve Momentum
		//We won't slow the player down if they are moving in their desired direction but at a greater speed than their maxSpeed
		if(doConserveMomentum && Mathf.Abs(RB.velocity.x) > Mathf.Abs(targetSpeed) && Mathf.Sign(RB.velocity.x) == Mathf.Sign(targetSpeed) && Mathf.Abs(targetSpeed) > 0.01f && LastOnGroundTime < 0)
		{
			//Prevent any deceleration from happening, or in other words conserve are current momentum
			//You could experiment with allowing for the player to slightly increae their speed whilst in this "state"
			accelRate = 0; 
		}
		#endregion

		//Calculate difference between current velocity and desired velocity
		float speedDif = targetSpeed - RB.velocity.x;
		//Calculate force along x-axis to apply to thr player

		float movement = speedDif * accelRate;

		//Convert this to a vector and apply to rigidbody
		RB.AddForce(movement * Vector2.right, ForceMode2D.Force);

		/*
		 * For those interested here is what AddForce() will do
		 * RB.velocity = new Vector2(RB.velocity.x + (Time.fixedDeltaTime  * speedDif * accelRate) / RB.mass, RB.velocity.y);
		 * Time.fixedDeltaTime is by default in Unity 0.02 seconds equal to 50 FixedUpdate() calls per second
		*/
	}//end of Run()


    private void Jump()
	{
		//increase jump count for switching platforms
		playerJumpCounter++; 

		//Switch all platforms in the scene
		SwitchPlatforms();

		//Ensures we can't call Jump multiple times from one press
		LastPressedJumpTime = 0;
		LastOnGroundTime = 0;

		#region Perform Jump
		//We increase the force applied if we are falling
		//This means we'll always feel like we jump the same amount 
		//(setting the player's Y velocity to 0 beforehand will likely work the same, but I find this more elegant :D)
		float force = jumpForce;
		if (RB.velocity.y < 0)
			force -= RB.velocity.y;

		RB.AddForce(Vector2.up * force, ForceMode2D.Impulse);
		#endregion

	}//end of Jump()



	private void Slide()
	{
		//Works the same as the Run but only in the y-axis
		//THis seems to work fine, buit maybe you'll find a better way to implement a slide into this system
		float speedDif = slideSpeed - RB.velocity.y;	
		float movement = speedDif * slideAccel;
		//So, we clamp the movement here to prevent any over corrections (these aren't noticeable in the Run)
		//The force applied can't be greater than the (negative) speedDifference * by how many times a second FixedUpdate() is called. For more info research how force are applied to rigidbodies.
		movement = Mathf.Clamp(movement, -Mathf.Abs(speedDif)  * (1 / Time.fixedDeltaTime), Mathf.Abs(speedDif) * (1 / Time.fixedDeltaTime));

		RB.AddForce(movement * Vector2.up);
	}


    
    void Shoot()
    {
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

    }//end of Shoot()



	public void Shootbelow(){

        bulletpre.dirdown = true;

        Instantiate(bulletpre, LaunchOffset.position + new Vector3(0.6f, 0, 0)/* + rotatedOffset*/, transform.rotation);
        
		Jump();

        bulletpre.dirdown = false;

    }//end of Shootbelow()


	public void SwitchPlatforms(){
		
		//SwitchPlatforms
		foreach (SwitchingPlatforms switche in switches)
            {
                switche.setToggleToFalse();
                print("set to false");
            }


		//AltSwitchPlatforms
		foreach (AltSwitchingPlatforms altswitche in altswitches)
            {
                altswitche.setToggleToFalse();
                print("set to false");
            }
	}//end of SwitchingPlatforms()





	//------------------------------------------------------------TRIGGERS-------------------------------------------------------------------------

	private void OnTriggerEnter2D(Collider2D other)
    {

		//Check overlapping with sticky
        if (other.gameObject.CompareTag("SlimeLight"))
        {
            playSticky = true;
            anim.SetBool("isStickySlime", playSticky);
        }
		if (other.gameObject.CompareTag("Trigger"))
		{
			// Player has entered the triggered area
			Debug.Log("Player entered the triggered area!");
			playerInside = true;
			visualCue.SetActive(true);
			// Add your desired actions or code here
		}
	}
	
	private void OnTriggerExit2D(Collider2D other)
    {

		//Check exiting the sticky
        if (other.gameObject.CompareTag("SlimeLight"))
        {
            playSticky = false;
            anim.SetBool("isStickySlime", playSticky);
        }
		if (other.gameObject.CompareTag("Trigger"))
		{
			// Player has entered the triggered area
			Debug.Log("Player exit the triggered area!");
			playerInside = false;
			visualCue.SetActive(false);
			// Add your desired actions or code here
		}

	}




	//--------------------------------------------------------------COLLISIONS-------------------------------------------------------------------------------------

	void OnCollisionEnter2D(Collision2D coll)
    {
		//Check interacting with elevator 
        if(coll.gameObject.tag == "elevator")
		{
            transform.parent = coll.gameObject.transform;
        }
    }


	void OnCollisionExit2D(Collision2D coll)
    {
		//Exiting interaction with elevator
        if (coll.gameObject.tag == "elevator")
        {
            transform.parent = null;
        }
    }

	
}
