using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class playerPhysicsMovements : MonoBehaviour
{

    #region COMPONENTS
    public Rigidbody2D RB;
	public Animator anim;
	#endregion

    //------------------------------------CHECK VARIABLES------------------------------------------------
    //bool variables for checks if a certain action is possible 
    public bool IsFacingRight { get; private set; }
	public bool IsJumping { get; private set; }
    public bool IsRunning { get; private set; }
    private bool _isJumpCut;
	private bool _isJumpFalling;


    //----------------------------------TIMERS------------------------------------------------
    //used for coyote time check. It is set to coyote time when player is grounded, otherwise when jumping or in air it starts countdown by Time.deltaTime each frame
    public float LastOnGroundTime { get; private set; }

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
	[SerializeField] private Vector2 groundCheckSize = new Vector2(0.49f, 0.03f);
	[Space(5)]
	[SerializeField] private Transform frontWallCheckPoint;
	[SerializeField] private Transform backWallCheckPoint;
	[SerializeField] private Vector2 wallCheckSize = new Vector2(0.5f, 1f);



    //------------------------------------------LAYERS-------------------------------------------------
    [Header("Layers & Tags")]
	[SerializeField] private LayerMask _groundLayer;
    public LayerMask stickyWallLayer;
    public GameObject bulletPrefab;

	public Tilemap hitmap;

    private void Awake()
	{
		RB = GetComponent<Rigidbody2D>();
		anim = GetComponent<Animator>();
	}

	private void Start()
	{
		
		IsFacingRight = true;

        gravityStrength = -(2 * jumpHeight) / (jumpTimeToApex * jumpTimeToApex);
        gravity = gravityStrength / Physics2D.gravity.y;

        runAccelAmount = (50 * runAcceleration) / runMaxSpeed;
        runDeccelAmount = (50 * runDecceleration) / runMaxSpeed;

        jumpForce = Mathf.Abs(gravityStrength) * jumpTimeToApex;

        runAcceleration = Mathf.Clamp(runAcceleration, 0.01f, runMaxSpeed);
		runDecceleration = Mathf.Clamp(runDecceleration, 0.01f, runMaxSpeed);

        
        SetGravityScale(gravity);
	}

    // Update is called once per frame
    void Update()
    {
        //Sticky code here
		Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 0.1f, stickyWallLayer);
        foreach (Collider2D collider in colliders)
        {
            StickyWall stickyWall = collider.GetComponent<StickyWall>();
            if (stickyWall != null && stickyWall.isSticky)
            {
                RB.velocity = Vector2.zero; // set player's velocity to zero to make it stick to the wall
                break;
			}
		}


        CollectInput();

        //COLLISIONS CHECK------------------------------
		CollisionChecks();



        //JUMP CHECKS-----------------------------------
		JumpCheck();



        //GRAVITY CHECKS------------------------------------
		GravityCheck();

    }



    private void FixedUpdate(){

        Run(1);
    }


    private void LateUpdate() {

        //set animations
        anim.SetBool("isRunning", (InAirPlayer() ? false : IsRunning));
        anim.SetBool("isJumping", InAirPlayer());
    }



    //---------------------------------------FUNCTIONS--------------------------------------boo
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


    //CHECKS IF ENABLE MOVEMENTS---------------------------------------------------------
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


    

	//MAIN CHECK FUNCTIONS IN UPDATE-----------------------------------------------------
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
        if(Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.C) || Input.GetKeyDown(KeyCode.J))
        {
			LastPressedJumpTime = jumpInputBufferTime;
        }

        //check jump release
         if (Input.GetKeyUp(KeyCode.Space) || Input.GetKeyUp(KeyCode.C) || Input.GetKeyUp(KeyCode.J))
		 {
		 	if (IsJumping && RB.velocity.y > 0)
		 	    _isJumpCut = true;
		 }


        if (Input.GetKeyDown(KeyCode.S))
    	{
        	// Call the Shoot method
        	Shoot();
    	}

    }


	public void CollisionChecks(){

		if(!IsJumping){

            if (Physics2D.OverlapBox(groundCheckPoint.position, groundCheckSize, 0, _groundLayer) && !IsJumping) //checks if set box overlaps with ground
			{

				LastOnGroundTime = coyoteTime; //if so sets the lastGrounded to coyoteTime
            }
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



	public void GravityCheck(){

		if (RB.velocity.y < 0 && moveInput.y < 0)
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



	//ACTION MOVEMENTS-----------------------------------------------------------------------
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
	}


    private void Jump()
	{
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
	}
    
    void Shoot()
    {
    	Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    	Vector2 direction = ((Vector2)mousePos - (Vector2)transform.position).normalized;
    	GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
    	

		if((mousePos.x - transform.position.x) > 0){

			hitmap.SendMessage("SetBulletDirectionRight");
		}
		else if((mousePos.x - transform.position.x) < 0){

			hitmap.SendMessage("SetBulletDirectionLeft");

		}


		bullet.GetComponent<Rigidbody2D>().velocity = direction * 7f;
    }
}
