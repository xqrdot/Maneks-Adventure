using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Throwable : MonoBehaviour
{
	public float rotationSpeed = 300f;
	[HideInInspector] public int damage = 2;

	[SerializeField] AudioClip sfx_onStick = null;

	bool canStick = true;
	bool canRotate = true;

	Rigidbody2D rb;
	AudioSource source;


	private void Awake()
	{
		rb = GetComponent<Rigidbody2D>();
		rotationSpeed *= Random.Range(0.7f, 1.3f);
		source = StaticStorage.instance.GameManager.GetComponent<AudioSource>();
	}
	private void Start()
	{
		StartCoroutine(FadeAway(4f));
	}

	private void FixedUpdate()
	{
		if (canRotate)
			rb.rotation -= rotationSpeed * Time.fixedDeltaTime;
	}
	private void OnCollisionEnter2D(Collision2D collision)
	{
		//if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
		if (collision.gameObject.GetComponent<IDamageable>() != null)
		{
			collision.gameObject.GetComponent<IDamageable>().DealDamage(damage);
			Destroy(gameObject);
		}
		//else if (collision.gameObject.CompareTag("Platform"))
		//{
		//	Physics2D.IgnoreCollision(gameObject.GetComponent<BoxCollider2D>(), collision.collider);
		//}
		else if (canStick) {
			gameObject.transform.SetParent(collision.transform);
			Stick();
		}
	}

	void Stick()
	{
		canRotate = false;
		rb.Sleep();
		rb.isKinematic = true;
		GetComponent<Collider2D>().enabled = false;

		if (sfx_onStick != null)
			source.PlayOneShot(sfx_onStick);
	}

	IEnumerator FadeAway(float seconds = 4, float fadeTime = 2.5f)
	{
		yield return new WaitForSeconds(seconds);

		gameObject.LeanAlpha(0, fadeTime);
		yield return new WaitForSeconds(fadeTime);

		yield return null;
		Destroy(gameObject);
	}
}

