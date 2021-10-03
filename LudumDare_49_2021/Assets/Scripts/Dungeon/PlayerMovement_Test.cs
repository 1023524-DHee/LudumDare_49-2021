using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(BoxCollider2D))]
public class PlayerMovement_Test : MonoBehaviour
{
	// PLAYER COMPONENTS
	private Rigidbody2D playerRB;
	private BoxCollider2D playerBoxCollider;

	// MOVEMENT VARIABLES
	private float movementInputDirection;
	private int facingDirection = 1;

	// JUMP VARIABLES
	private int currentJumpTimes;
	private float initialJumpHeight;
	private float coyoteTimeCounter;
	private float jumpBufferCounter;
	private bool jumpHeightLimitReached;

	// DASH VARIABLES
	private float dashTimeCounter;
	private float dashCooldownCounter;

	// WALL JUMP VARIABLES
	private int initialWallJumpDirection;
	private float timeOffWallCounter;
	private float wallJumpBufferCounter;
	private bool initialWallTouching;

	// KNOCKBACK VARIABLES
	private int knockbackFacingDirection;
	private float knockbackTimeCounter;
	private float currentXKnockbackForce;
	private float currentYKnockbackForce;
	private bool isInKnockback;

	// CHECKING BOOLEANS
	private bool isFacingRight = true;
	private bool isGrounded;
	private bool isWalking;
	private bool isDashing;
	private bool isTouchingWall;

	// STATUS EFFECTS
	private bool isImmobilised;

	[Header("Movement Variables")]
	[Tooltip("Player movement speed.")]
	[SerializeField] private float movementSpeed;

	[Header("Jump Variables")]

	[Tooltip("Empty GameObject positioned where the feet are to detect ground")]
	[SerializeField] private Transform groundCheck;

	[Tooltip("Maximum height the player can jump")]
	[SerializeField] private float maximumJumpHeight;

	[Tooltip("Initial velocity of the jump.")]
	[SerializeField] private float jumpVelocity;

	[Tooltip("Multiply gravity by this value when falling.")]
	[SerializeField] private float fallMultiplier;

	[Tooltip("Should be higher than initial gravity scale to allow for variable height jumping.")]
	[SerializeField] [Min(0)] private float lowJumpMultiplier;

	[Tooltip("Time in seconds the player can jump after leaving the ground. Suggested amount <= 0.1")]
	[SerializeField] private float maximumCoyoteTime;

	[Tooltip("Time in seconds the player can press jump before landing to jump again. Suggested amount <= 0.1")]
	[SerializeField] private float maximumJumpBufferTime;

	[Tooltip("Maximum number of times the player can jump")]
	[SerializeField] [Min(1)] private int maximumJumpTimes;

	[Header("Dash Variables")]
	[Tooltip("Amount of time the player can dash")]
	[SerializeField] private float maximumDashTime;

	[Tooltip("Initial velocity of player dash")]
	[SerializeField] private float dashVelocity;

	[Tooltip("Amount of time between each dash")]
	[SerializeField] private float dashCooldown;

	[Header("Wall Jump Variables")]

	[Tooltip("Empty GameObject positioned in front of the player to detect wall")]
	[SerializeField] private Transform wallCheck;

	[Tooltip("Distance to project the raycast to find wall.")]
	[SerializeField] private float wallCheckDistance;

	[Tooltip("Slide speed of the player when touching the wall")]
	[SerializeField] private float wallSlideSpeed;

	[Tooltip("Similar to normal jump buffer but for wall jumps. Suggested amount <= 0.1")]
	[SerializeField] private float wallJumpBufferTime;

	[Tooltip("Duration of wall jump. Suggested amount <= 0.1")]
	[SerializeField] private float maximumWallJumpDuration;

	[Tooltip("Force applied in the x direction when wall jumping")]
	[SerializeField] private float xWallForce;

	[Header("VFX")]
	[SerializeField] private GameObject jumpDustFX;
	[SerializeField] private ParticleSystem wallSlideFX;

	[Header("Layer Masks")]
	[SerializeField] private LayerMask groundLayerMask;

	private void Start()
	{
		playerRB = GetComponent<Rigidbody2D>();
		playerBoxCollider = GetComponent<BoxCollider2D>();
	}

	private void Update()
	{
		CheckInput();
		CheckIfGrounded();
		CheckMovementDirection();

		CheckCast();

		CheckJumpHeight();
		CheckHigherJump();
		CheckCoyoteTime();
		CheckJumpBufferTime();

		CheckDashTime();

		CheckTouchingWall();
		CheckWallJumping();

		CheckKnockback();

		//UpdateAnimations();
		//UpdateFX();
	}

	private void FixedUpdate()
	{
		if (!isImmobilised)
		{
			ApplyMovement();
			NormalJump();
			Dash();

			WallSlide();
			WallJump();

		}
		KnockbackPlayer();
	}

	#region Constant Checks Functions

	// Checks User Inputs.
	private void CheckInput()
	{
		if (!isDashing && !isInKnockback)
		{
			movementInputDirection = Input.GetAxisRaw("Horizontal");
		}
	}

	// Checks to see if the player needs to be flipped when moving left or right.
	// Sets isWalking to true or false when moving or stationary.
	private void CheckMovementDirection()
	{
		if (isFacingRight && movementInputDirection < 0f)
		{
			Flip();
		}
		else if (!isFacingRight && movementInputDirection > 0f)
		{
			Flip();
		}

		if (Mathf.Abs(playerRB.velocity.x) > 0f)
		{
			isWalking = true;
		}
		else
		{
			isWalking = false;
		}
	}

	// Updates jumpHeightLimitReached. Checks if the player has reached the maximum jump height allowed.
	private void CheckJumpHeight()
	{
		if (transform.position.y - initialJumpHeight > maximumJumpHeight)
		{
			jumpHeightLimitReached = true;
		}
		else
		{
			jumpHeightLimitReached = false;
		}
	}

	// Checks if the player is holding the jump button for a higher jump.
	private void CheckHigherJump()
	{
		if (playerRB.velocity.y < 0 || jumpHeightLimitReached) // Increase gravity when falling or when reached the height limit.
		{
			playerRB.gravityScale = fallMultiplier;
		}
		else if (playerRB.velocity.y > 0 && !Input.GetButton("Jump")) // Apply a reduced amount of gravity when high jumping.
		{
			playerRB.gravityScale = lowJumpMultiplier;
		}
		else // Reset gravity scale to 1 at other times
		{
			playerRB.gravityScale = 1f;
		}
	}

	// Updates isGrounded, currentJumpTimes. Used to check if the player is on the ground using Boxcast.
	// Also checks if the player has landed.
	private void CheckIfGrounded()
	{
		bool wasGrounded = isGrounded;
		isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.1f, groundLayerMask);

		// Check if landed
		if (!wasGrounded && isGrounded)
		{
			PlayerLanded();
		}
	}

	// Updates coyoteTimeCounter. Used to check if player shuold be able to jump after falling off a ledge.
	// coyoteTimeCounter > 0 at time of jump means can jump. Otherwise, no jump.
	private void CheckCoyoteTime()
	{
		if ((isGrounded && wallJumpBufferCounter <= 0f) || currentJumpTimes > 0) // If on ground && not on wall, keep resetting coyote time
		{
			coyoteTimeCounter = maximumCoyoteTime;
		}
		else
		{
			//Debug.Log(currentJumpTimes);
			if (currentJumpTimes <= 0) // Decrement if the player doesn't have any jumps left
			{
				coyoteTimeCounter -= Time.deltaTime;

				if (Input.GetButtonUp("Jump")) // Set counter to 0 to prevent unintended double jumping during the coyote time duration
				{
					coyoteTimeCounter = 0f;
				}
			}
		}
	}

	// Updates jumpBufferCounter. Used to check if player should be able to jump right before landing.
	// jumpBufferCounter > 0 at time of jump means can jump. Otherwise, no jump.
	private void CheckJumpBufferTime()
	{
		if (Input.GetButtonDown("Jump") && wallJumpBufferCounter <= 0f) // If pressed 'Jump' && not on wall, reset counter to max value
		{
			jumpBufferCounter = maximumJumpBufferTime;
		}
		else
		{
			if (currentJumpTimes <= 0 || currentJumpTimes == maximumJumpTimes) // Decrement if the player doesn't have any jumps left
			{
				jumpBufferCounter -= Time.deltaTime;
			}
		}
	}

	// Updates dashTimeCounter. Used to check if the player can dash.
	// dashTimeCounter > 0 means can dash. Otherwise, cant dash.
	private void CheckDashTime()
	{
		if (Input.GetKeyDown(KeyCode.LeftShift) && isWalking && dashCooldownCounter < 0f) // If pressed dash key, is walking in a direction, is off cooldown
		{
			dashTimeCounter = maximumDashTime;
			dashCooldownCounter = dashCooldown;
		}
		dashTimeCounter -= Time.deltaTime;
		dashCooldownCounter -= Time.deltaTime;
	}

	// Updates isTouchingWall, timeOffWallCounter, initialJumpHeight. Used to check if the player is touching the wall using raycast.
	// timeOffWallCounter starts decrementing when the player leaves the wall.
	private void CheckTouchingWall()
	{
		RaycastHit2D raycastHit = Physics2D.Raycast(wallCheck.position, transform.right, wallCheckDistance, groundLayerMask);

		if (raycastHit && !isGrounded && movementInputDirection != 0)
		{
			isTouchingWall = true;
			timeOffWallCounter = wallJumpBufferTime;

			initialJumpHeight = transform.position.y;
		}
		else
		{
			isTouchingWall = false;
		}
		timeOffWallCounter -= Time.deltaTime;
	}

	// Updates wallJumpBufferCounter. Used to check if the player can jump off the wall after a certain amount of time.
	// wallJumpBufferCounter > 0 means can jump. Otherwise, no jump.
	private void CheckWallJumping()
	{
		if (timeOffWallCounter > 0f && Input.GetButtonDown("Jump"))
		{
			wallJumpBufferCounter = maximumWallJumpDuration;

			initialWallJumpDirection = facingDirection;
			initialWallTouching = isTouchingWall;
		}
		wallJumpBufferCounter -= Time.deltaTime;
	}

	// Updates knockbackTimeCounter. Used to check if the player is being knocked back.
	private void CheckKnockback()
	{
		knockbackTimeCounter -= Time.deltaTime;

		isInKnockback = knockbackTimeCounter > 0f ? true : false;
	}
	#endregion

	#region Jump Functions

	// Normal jumping function is done by checking the coyoteTimeCounter and jumpBufferCounter values.
	// Only allowed to jump if satisfies both conditions.
	// TIME BASED JUMPING
	private void NormalJump()
	{
		//Debug.Log(coyoteTimeCounter);
		if (coyoteTimeCounter > 0f && jumpBufferCounter > 0f)
		{
			playerRB.velocity = new Vector2(playerRB.velocity.x, jumpVelocity);

			currentJumpTimes--;
			jumpBufferCounter = 0;
			initialJumpHeight = transform.position.y;

			//JumpParticleFX();
		}
	}
	#endregion

	#region Wall Jump Functions
	// If player is pressing against the wall, perform wall slide.
	private void WallSlide()
	{
		if (isTouchingWall)
		{
			playerRB.velocity = new Vector2(playerRB.velocity.x, Mathf.Clamp(playerRB.velocity.y, -wallSlideSpeed, float.MaxValue));
		}
	}

	// DISCLAIMER: THERE'S PROBABLY A BETTER WAY TO DO THIS. IF YOU FIND OUT LET ME KNOW.
	// If player jumps off the wall when pressing against it, reverse the x-direction and apply force.
	// If player jumps off the wall while not pressing against it, apply force in the x-direction.
	private void WallJump()
	{
		if (wallJumpBufferCounter > 0f)
		{
			float newXWallForce;
			if (initialWallTouching) newXWallForce = -initialWallJumpDirection * xWallForce;
			else newXWallForce = initialWallJumpDirection * xWallForce;

			playerRB.velocity = new Vector2(newXWallForce, jumpVelocity);
			currentJumpTimes--;
		}
	}
	#endregion

	#region Dash Functions
	private void Dash()
	{
		if (dashTimeCounter > 0f)
		{
			isDashing = true;
			playerRB.velocity = new Vector2(playerRB.velocity.x + (dashVelocity * facingDirection), playerRB.velocity.y);
		}
		else
		{
			isDashing = false;
		}
	}
	#endregion

	#region Knockback Functions
	private void KnockbackPlayer()
	{
		if (knockbackTimeCounter > 0f)
		{
			if (currentXKnockbackForce <= 0f)
			{
				playerRB.velocity = new Vector2(playerRB.velocity.x, currentYKnockbackForce);
			}
			else
			{
				playerRB.velocity = new Vector2(knockbackFacingDirection * currentXKnockbackForce, currentYKnockbackForce);
			}
		}
	}
	#endregion

	#region Misc Functions
	private void ApplyMovement()
	{
		playerRB.velocity = new Vector2(movementSpeed * movementInputDirection, playerRB.velocity.y);
	}

	private void Flip()
	{
		facingDirection *= -1;
		isFacingRight = !isFacingRight;
		transform.Rotate(0f, 180f, 0f);
	}

	private void PlayerLanded()
	{
		currentJumpTimes = maximumJumpTimes;
		initialJumpHeight = transform.position.y;

		//JumpParticleFX();
	}
	#endregion

	#region Animations
	//private void UpdateAnimations()
	//{
	//	playerAnimator.SetFloat("Speed", Mathf.Abs(playerRB.velocity.x));
	//	playerAnimator.SetFloat("VerticalVelocity", playerRB.velocity.y);

	//	playerAnimator.SetBool("IsInAir", !isGrounded);
	//	playerAnimator.SetBool("IsHuggingWall", isTouchingWall);
	//	playerAnimator.SetBool("IsMoving", Mathf.Abs(playerRB.velocity.x) > 0.0f);
	//}

	//private void UpdateFX()
	//{
	//	WallSlideFX();
	//}

	//private void JumpParticleFX()
	//{
	//	Vector3 offset = new Vector3(0.0f, playerBoxCollider.size.y, 0.0f);
	//	Instantiate(jumpDustFX, transform.position - offset, Quaternion.identity);
	//}

	//private void WallSlideFX()
	//{
	//	if (isTouchingWall)
	//	{
	//		wallSlideFX.Play();
	//	}
	//	else
	//	{
	//		wallSlideFX.Stop();
	//	}
	//}
	#endregion


	private void CheckCast()
	{
		if (Input.GetMouseButtonDown(0))
		{
			RaycastHit2D hit = Physics2D.Raycast(transform.position, Camera.main.ScreenToWorldPoint(Input.mousePosition), 10f);

			if (hit.collider != null)
			{
				hit.collider.GetComponent<IPausePlatform>()?.PausePlatform();
			}
		}
	}
}
