#pragma warning disable 0649
#pragma warning disable 0414
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour, IDamageable
{
	[Header("Movement")]
	[SerializeField] float runSpeed = 40f;
	[SerializeField] private float m_JumpForce = 400f;                          // Amount of force added when the player jumps.
	[Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;  // How much to smooth out the movement
	[SerializeField] private bool m_AirControl = false;                         // Whether or not a player can steer while jumping;
	[SerializeField] private LayerMask m_WhatIsGround;                          // A mask determining what is ground to the character
	[SerializeField] private Transform m_GroundCheck = null;                    // A position marking where to check if the player is grounded.
	[SerializeField] public bool AbilityDoubleJump = true;
	[Range(0.5f, 1.2f)]
	[SerializeField] public float m_DoubleJumpMultiplier = 0.8f;
	

	const float k_GroundedRadius = .2f; // Radius of the overlap circle to determine if grounded
	private bool m_Grounded;            // Whether or not the player is grounded.
	const float k_CeilingRadius = .2f;  // Radius of the overlap circle to determine if the player can stand up
	private Rigidbody2D rb2D;
	bool canDoubleJump;
	public bool m_FacingRight = true;  // For determining which way the player is currently facing.
	private Vector3 m_Velocity = Vector3.zero;
	float horizontalMove = 0f;
	bool jumpRequest = false;
	bool ascending;
	bool running;
	bool m_canReceiveDamage = true;

	[Header("Audio")]
	[SerializeField] AudioClip clip_hurtSmall = null;
	[SerializeField] AudioClip clip_hurtBig = null;
	[SerializeField] AudioClip clip_throw = null;
	[SerializeField] AudioClip clip_jump = null;
	[SerializeField] AudioClip clip_jump2 = null;
	[SerializeField] AudioClip clip_land = null;
	[SerializeField] AudioClip clip_step = null;

	[Header("Visuals")]
	[SerializeField] private ParticleSystem particlesRunning = null;

	[Header("Stats")]
	[SerializeField] public int maxHealth = 6;
	[HideInInspector] public int currentHealth;


	[System.Serializable]
	public class BoolEvent : UnityEvent<bool> { }

	[Header("Events")]
	[Space]
	public UnityEvent OnLandEvent;

	public delegate void OnChangeHealth(int health); public OnChangeHealth changeHealth;

	[Header("Components")]
	Animator animator;
	AudioSource source;
	Camera mainCamera;

	ManagerGame managerGame;

	/* enum State { Idling, Running, Ascending }
	private State state = new State(); */


	private void Awake()
	{

		if (OnLandEvent == null)
			OnLandEvent = new UnityEvent();

		if (m_GroundCheck is null)
			Debug.LogError($"Something is null in {name}!");

		rb2D = GetComponent<Rigidbody2D>();
		animator = GetComponent<Animator>();
		source = GetComponent<AudioSource>();


		currentHealth = maxHealth;
	}

	private void Start()
	{
		changeHealth?.Invoke(currentHealth);
		managerGame = StaticStorage.instance.GameManager.GetComponent<ManagerGame>();
		mainCamera = Camera.main;

	}

	private void Update()
	{
		if (managerGame.playerInput == PlayerInput.Active)
		{

			horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;


			if (Input.GetButtonDown("Jump"))
			{
				jumpRequest = true;
				animator.SetBool("IsJumping", true);
			}
			if (Input.GetKeyDown(KeyCode.U))
			{
				DealDamage(3, true);
			}
		}

		animator.SetBool("IsGrounded", m_Grounded);
		animator.SetBool("IsAscending", ascending);
		animator.SetFloat("Speed", Mathf.Clamp(Mathf.Abs(horizontalMove), 0, 2));
	}

	private void FixedUpdate()
	{
		bool wasGrounded = m_Grounded;
		m_Grounded = false;
		var cached_Velocity = PlayerVelocity(); // Cache velocity for this Physics step

		Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
		for (int i = 0; i < colliders.Length; i++)
		{
			if (colliders[i].gameObject != gameObject)
			{
				m_Grounded = true;
				if (!wasGrounded && cached_Velocity.y <= 0)
				{
					//if (cached_Velocity.y <= -20f)
					//{
					//	DealDamage(cached_Velocity.y <= -35f ? 4 : 2, true);
					//}
					OnLandEvent.Invoke();
				}
			}
		}

		//Debug.Log($"Velocity: {cached_Velocity.y}\n{wasGrounded}");

		Move(horizontalMove * Time.fixedDeltaTime, jumpRequest);
		jumpRequest = false;
	}

	public Vector2 PlayerVelocity()
	{
		return GetComponent<Rigidbody2D>().velocity;
	}

	public void Step()
	{
		source.PlayOneShot(clip_step);
	}
	public void OnLanding()
	{
		animator.SetBool("IsJumping", false);
		source.PlayOneShot(clip_land);
		mainCamera.GetComponent<StressReceiver>().InduceStress(0.15f);

		CreateDust();
	}
	public void StopMomentum()
	{
		rb2D.velocity = Vector2.zero;
		horizontalMove = 0;
	}

	private void CreateDust()
	{
		particlesRunning.Play();
	}

	private void OnDrawGizmosSelected()
	{
		Gizmos.DrawWireSphere(m_GroundCheck.position, k_GroundedRadius);
	}
	


	///
	/// Dealing damage
	/// 

	public void DealDamage(int damage, bool noDirection)
	{
		if (m_canReceiveDamage)
		{
			currentHealth -= damage;
			changeHealth?.Invoke(currentHealth);

			if (!noDirection)
			{
				StopMomentum();
				rb2D.AddForce(new Vector2(-60 * (m_FacingRight == true ? 1 : -1), 8), ForceMode2D.Impulse);
			}
			animator.SetTrigger("Hit");
			source.PlayOneShot(clip_hurtSmall);

			if (noDirection)
				StopCoroutine(Stun()); StartCoroutine(Stun());
		}
	}

	// public void DealDamage(int damage, bool noDirection, bool noStun)
	// {
	// 	currentHealth -= damage;
	// 	changeHealth?.Invoke(currentHealth);

	// 	StopMomentum();
	// 	if (!noDirection)
	// 		rb2D.AddForce(new Vector2(-30 * (m_FacingRight == true ? 1 : -1), 8), ForceMode2D.Impulse);

	// 	animator.SetTrigger("Hit");
	// 	source.PlayOneShot(clip_hurtSmall);

	// 	if (!noStun)
	// 		StopCoroutine(Stun()); StartCoroutine(Stun());
	// }

	IEnumerator Stun(float seconds)
	{
		managerGame.playerInput = PlayerInput.Disabled;
		m_canReceiveDamage = false;
		yield return new WaitForSeconds(seconds);

		managerGame.playerInput = PlayerInput.Active;
		m_canReceiveDamage = true;
		yield return new WaitForEndOfFrame();
	}

	IEnumerator Stun()
	{
		managerGame.playerInput = PlayerInput.Disabled;
		m_canReceiveDamage = false;
		yield return new WaitForSeconds(0.5f);
		
		managerGame.playerInput = PlayerInput.Active;
		m_canReceiveDamage = true;
		yield return new WaitForEndOfFrame();
	}

	public void Move(float move, bool jump)
	{

		//only control the player if grounded or airControl is turned on
		if (m_Grounded || m_AirControl)
		{

			//Move the character by finding the target velocity
			Vector3 targetVelocity = new Vector2(move * 10f, PlayerVelocity().y);
			// And then smoothing it out and applying it to the character
			rb2D.velocity = Vector3.SmoothDamp(rb2D.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);

			if (move > 0 && !m_FacingRight)
			{
				Flip();
			}
			else if (move < 0 && m_FacingRight)
			{
				Flip();
			}
		}
		// If the player should jump...
		if (m_Grounded && jump)
		{
			canDoubleJump = true;
			m_Grounded = !false;
			rb2D.AddForce(new Vector2(0f, m_JumpForce));

			source.PlayOneShot(clip_jump);
		}

		if (!m_Grounded && jump && AbilityDoubleJump && canDoubleJump)
		{
			m_Grounded = false;
			canDoubleJump = false;
			rb2D.velocity = new Vector2(rb2D.velocity.x, 0);
			rb2D.AddForce(new Vector2(0f, m_JumpForce * m_DoubleJumpMultiplier));

			source.PlayOneShot(clip_jump2);
		}

		if (!m_Grounded)
		{
			// Ascending is true if Velocity.y is > 0
			ascending = PlayerVelocity().y <= 0 ? false : true;
		}
	}


	private void Flip()
	{
		// Switch the way the player is labelled as facing.
		m_FacingRight = !m_FacingRight;

		// Multiply the player's x local scale by -1.
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;

		if (m_Grounded) 
			CreateDust();
	}
}