using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;

public class DamageDealer : MonoBehaviour
{
	public void DealDamage(GameObject gameObject, float damageAmount, bool shakeScreen)
	{
		Health damageable = gameObject.GetComponentInParent<Health>();
		if(!damageable)
			return;

		damageable.TakeDamage(damageAmount);
		if(shakeScreen)
			CameraShaker.Instance.ShakeOnce(3f, 2f, 0.5f, 0.5f);
	}
}
