#pragma warning disable 0649
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageTriggers : MonoBehaviour
{
	[SerializeField] private float damage = 2;
	[SerializeField] private bool stun = false;


	private void OnTriggerEnter2D(Collider2D collision)
	{
		IDamageable damageable = collision.GetComponent<IDamageable>();

		if (damageable != null)
		{
			damageable.DealDamage((int)damage, stun);
		}
	}
}
