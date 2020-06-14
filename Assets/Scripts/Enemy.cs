#pragma warning disable 0649
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Enemy : MonoBehaviour, IDamageable
{
  [SerializeField] private EnemyStats enemyStats;
  [Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;  // How much to smooth out the movement

  private int currentHealth;

  public float bounceForce = 200f;


  Animator animator;
  AudioSource source;
  Rigidbody2D rb;

  Vector3 m_Velocity = Vector3.zero;
  bool m_FacingRight = true;
  bool isIdle = false;

  bool isDead;

  private void Awake()
  {
    isDead = false;
  }
  private void Start()
  {
    rb = GetComponent<Rigidbody2D>();
    animator = GetComponent<Animator>();
    source = StaticStorage.instance.GameManager.GetComponent<AudioSource>();

    currentHealth = enemyStats.maxHealth;
  }

  Vector2 CurrentVelocity()
  {
    return rb.velocity;
  }

  // private void Update()
  // {
		
  // }

  private void FixedUpdate()
  {
    if (!isIdle)
    {
      int directionMod = m_FacingRight ? 1 : -1;
			// print($"Is facing right? {directionMod}");

      // Vector3 targetVelocity = new Vector2(2.5f, CurrentVelocity().y);
      // rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);


      //Vector2 initCheck = new Vector2(transform.position.x + directionMod, transform.position.y);
      //var groundCheck = Physics2D.Raycast(initCheck, Vector2.down, 1.5f);
      //if (groundCheck.collider == false)
      //	StartCoroutine(StateStay(2f));

      //LayerMask layerMask = LayerMask.NameToLayer("Default");
      //collider.isTrigger = false;
      //collider.enabled = false;
      //gameObject.GetComponent<SpriteRenderer>().sortingOrder = -3;
      //rb.Sleep();
      // rb.isKinematic = true;

      var direction = new Vector3(transform.position.x + 2, transform.position.y);
			// print($"Direction: {direction}");
      var wallCheck = Physics2D.RaycastAll(transform.position, direction, 3);
      //var wallCheck = Physics2D.Raycast(transform.position, direction, 2);
      //var wallCheck = Physics2D.Raycast(transform.position, direction, 2, layerMask);
      //Debug.Log($"{wallCheck.collider}, {direction}");

      foreach (RaycastHit2D obj in wallCheck)
      {
        if (obj.transform.gameObject.layer == LayerMask.NameToLayer("Default"))
        {
          StartCoroutine(StateStay(2f));
        }
      }
    }
  }

  void OnDrawGizmos()
  {
		int directionMod = m_FacingRight ? 1 : -1;
    Gizmos.DrawLine(transform.position, new Vector3(transform.position.x + 2.5f * directionMod, transform.position.y));
  }

  IEnumerator StateStay(float waitTime)
  {
    isIdle = true;
    yield return new WaitForSeconds(waitTime);

		Flip();
    isIdle = false;
    yield return null;
  }

	void Flip()
	{
		m_FacingRight = !m_FacingRight;
		transform.rotation = Quaternion.Euler(0, m_FacingRight ? 0 : 180, 0);
	}

  private void OnTriggerEnter2D(Collider2D collision)
  {
    if (collision.gameObject.layer == LayerMask.NameToLayer("Player") || collision.gameObject.layer == LayerMask.NameToLayer("Default"))
    {
      var col = collision.GetComponent<Rigidbody2D>();
      col.velocity = new Vector2(col.velocity.x, 0);
      col.AddForce(Vector2.up * bounceForce);

      DealDamage(9999, false);
    }
  }

  public void DealDamage(int damage, bool stun)
  {
    if (!isDead)
    {
      currentHealth -= damage;
      animator.SetTrigger("Hit");
      source.PlayOneShot(enemyStats.hurtSmall);

      if (currentHealth <= 0)
        Die();
    }
  }

  void Die()
  {
    foreach (Collider2D collider in gameObject.GetComponents<Collider2D>())
    {
      collider.isTrigger = false;
      collider.enabled = false;
      gameObject.GetComponent<SpriteRenderer>().sortingOrder = -3;
    }
    rb.Sleep();
    rb.isKinematic = true;

    isDead = true;
    source.PlayOneShot(enemyStats.hurtBig);
    animator.SetBool("Dead", isDead);

    StartCoroutine(FadeAway());
  }

  IEnumerator FadeAway()
  {
    yield return new WaitForSeconds(3);
    gameObject.LeanAlpha(0, 2.5f);
    Destroy(gameObject, 3f);

    yield return null;
  }
}
