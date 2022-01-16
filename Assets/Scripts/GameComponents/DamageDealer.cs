using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using EZCameraShake;

public class DamageDealer : MonoBehaviour
{
	/// <summary>
	/// Deal damage to a Damageable game object's health
	/// </summary>
	/// <param name="gameObject">Game object to damage. Only affects objects with Damageable component.</param>
	/// <param name="damageAmount">Amount of damage to take away from the Damageable's health.</param>
	/// <param name="shakeScreen">Triggers a screen shake if set to true.</param>
	public void DealDamage(GameObject gameObject, float damageAmount, bool shakeScreen)
	{
		Damageable damageable = gameObject.GetComponentInParent<Damageable>();
		if(!damageable)
			return;

		damageable.TakeDamage(damageAmount);
		//if(shakeScreen)
		//	CameraShaker.Instance.ShakeOnce(3f, 3f, 0.5f, 0.5f);
	}
}
