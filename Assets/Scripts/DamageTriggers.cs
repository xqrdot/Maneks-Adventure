#pragma warning disable 0649
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageTriggers : MonoBehaviour
{
	// Start is called before the first frame update
	private void OnTriggerEnter2D(Collider2D collision)
	{
		IDamageable damageable = collision.GetComponent<IDamageable>();

		if (damageable != null)
		{
			damageable.DealDamage(2, false);
		}
	}
}
