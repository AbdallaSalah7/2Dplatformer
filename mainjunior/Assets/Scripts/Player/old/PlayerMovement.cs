using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public PlayerControl pc;
    public Rigidbody2D RB;
    public Animator anim;

    public bool IsFacingRight;
    public float LastOnGroundTime;
    //public bool IsRunning;

    [Header("Run")]
	public float runMaxSpeed = 8f; //Target speed we want the player to reach.
	public float runAcceleration = 4.5f; //The speed at which our player accelerates to max speed, can be set to runMaxSpeed for instant acceleration down to 0 for none at all
	[HideInInspector] public float runAccelAmount; //The actual force (multiplied with speedDiff) applied to the player.
	public float runDecceleration = 4.5f; //The speed at which our player decelerates from their current speed, can be set to runMaxSpeed for instant deceleration down to 0 for none at all
	[HideInInspector] public float runDeccelAmount; //Actual force (multiplied with speedDiff) applied to the player .
	[Space(5)]
	[Range(0f, 1)] public float accelInAir = 1f; //Multipliers applied to acceleration rate when airborne.
	[Range(0f, 1)] public float deccelInAir = 1f;
	[Space(5)]
	public bool doConserveMomentum = true;

    [System.NonSerialized]
    public Vector2 _moveInput; 

    [Header("Checks")] 
	[SerializeField] private Transform _groundCheckPoint;
	//Size of groundCheck depends on the size of your character generally you want them slightly small than width (for ground) and height (for the wall check)
	[SerializeField] private Vector2 _groundCheckSize = new Vector2(0.49f, 0.03f);

    [Header("Layers & Tags")]
	[SerializeField] private LayerMask _groundLayer;
    //public LayerMask stickyWallLayer;
	

    private void Awake() {
        RB = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        pc = GetComponent<PlayerControl>();
    }

    private void Start() {
        IsFacingRight = true;
        
        runAccelAmount = (50 * runAcceleration) / runMaxSpeed;
        runDeccelAmount = (50 * runDecceleration) / runMaxSpeed;

        runAcceleration = Mathf.Clamp(runAcceleration, 0.01f, runMaxSpeed);
		runDecceleration = Mathf.Clamp(runDecceleration, 0.01f, runMaxSpeed);

    }

    private void Update() {
        /* //--------------------------------------------------------STICKY-------------------------------------------------------------
        Collider2D[] colliders = Physics2D.OverlapBoxAll(transform.position, new Vector2(1, 1), 0, stickyWallLayer);
        foreach (Collider2D collider in colliders)
        {
            StickyWall stickyWall = collider.GetComponent<StickyWall>();

            if (stickyWall != null && stickyWall.isSticky)
            {
                //_moveInput.x = 0;
                RB.velocity = Vector2.zero; // set player's velocity to zero to make it stick to the wall it
                //playSticky = true;
                //anim.SetTrigger("isSlimeSticky");
                break;
            }
        } */

        LastOnGroundTime -= Time.deltaTime;
		_moveInput.x = Input.GetAxisRaw("Horizontal");

        if(_moveInput.x != 0){
            CheckDirectionToFace(_moveInput.x > 0);
            anim.SetBool("CanMove", true);
        }
        else
        {
            anim.SetBool("CanMove", false);
        }
        anim.SetFloat("moveSpeed", Mathf.Abs(RB.velocity.x));

        if (Physics2D.OverlapBox(_groundCheckPoint.position, _groundCheckSize, 0, _groundLayer)) //checks if set box overlaps with ground
			LastOnGroundTime = 0.1f;
    }

    private void FixedUpdate() {
        Run();
    }

    private void Run(){
        float targetSpeed = _moveInput.x * runMaxSpeed;

		float accelRate;

        if (LastOnGroundTime > 0)
			accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? runAccelAmount : runDeccelAmount;
		else
			accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? runAccelAmount * accelInAir : runDeccelAmount * deccelInAir;

        /* if ((pc.isJumping|| _isJumpFalling) && Mathf.Abs(RB.velocity.y) < pc.jumpHangTimeThreshold)
		{
            print("testttt");
			accelRate *= pc.jumpHangAccelerationMult;
			targetSpeed *= pc.jumpHangMaxSpeedMult; 
		} */
        
        if(doConserveMomentum && Mathf.Abs(RB.velocity.x) > Mathf.Abs(targetSpeed) && Mathf.Sign(RB.velocity.x) == Mathf.Sign(targetSpeed) && Mathf.Abs(targetSpeed) > 0.01f && LastOnGroundTime < 0){
			//Prevent any deceleration from happening, or in other words conserve are current momentum
			//You could experiment with allowing for the player to slightly increae their speed whilst in this "state"
			accelRate = 0; 
		}

		float speedDif = targetSpeed - RB.velocity.x;
		float movement = speedDif * accelRate;

		RB.AddForce(movement * Vector2.right, ForceMode2D.Force);
    }


    public void CheckDirectionToFace(bool isMovingRight)
	{
		if (isMovingRight != IsFacingRight && !pc.isWallJumping){

            //stores scale and flips the player along the x axis, 
		    Vector3 scale = transform.localScale; 
		    scale.x *= -1;
		    transform.localScale = scale;

		    IsFacingRight = !IsFacingRight;
        }
	}


}
