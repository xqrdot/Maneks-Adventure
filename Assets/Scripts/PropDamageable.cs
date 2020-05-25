using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropDamageable : MonoBehaviour, IDamageable
{
	[SerializeField] int health = 1;
	[SerializeField] AudioClip sfx_onBreak = null;

	AudioSource source;

	private void Start()
	{
		source = StaticStorage.instance.GameManager.GetComponent<AudioSource>();
	}

	public void DealDamage(int damage)
	{
		health -= damage;

		if (health <= 0)
		{
			if (sfx_onBreak != null)
				source.PlayOneShot(sfx_onBreak);

			Destroy(gameObject);
		}
	}
}
