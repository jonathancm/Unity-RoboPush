using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageDealer : MonoBehaviour
{
	[SerializeField] float damageAmount = 1.0f;

	public void DealDamage(GameObject gameObject)
	{
		Health damageable = gameObject.GetComponentInParent<Health>();
		if(!damageable)
			return;

		damageable.TakeDamage(damageAmount);
	}
}
