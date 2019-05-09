using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PunchingGlove : MechaWeapon
{
	enum WeaponState
	{
		Ready,
		Firing,
		Rearming,
		CoolingDown
	};

	// Configurable Parameters
	[Header("Damage")]
	[SerializeField] float normalDamage = 1.0f;
	[SerializeField] float chargedDamage = 5.0f;

	[Header("Animation")]
	[SerializeField] Animator animatorController = null;
	[SerializeField] float coolDownPeriodInSeconds = 1.5f;
	[SerializeField] float punchingArmLength = 0.85f;
	[SerializeField] [Range(0.001f, 1.0f)] float punchExtensionSteps = 0.1f;

	[Header("Physics")]
	[SerializeField] Rigidbody mainRigidBody = null;
	[SerializeField] float knockbackStrength = 100.0f;
	[SerializeField] float chargedKnockbackStrength = 250.0f;
	[SerializeField] float recoilStrength = 50.0f;

	[Header("Sound Effects")]
	[SerializeField] AudioClip[] punchSounds = null;
	[Range(0.0f, 1.0f)] [SerializeField] float punchVolume = 0.5f;

	// Cached References
	AudioSource audioSource = null;
	DamageDealer damageDealer = null;
	Rigidbody gloveRigidbody = null;

	// State variables
	WeaponState weaponState = WeaponState.Ready;
	Vector3 originalPosition;
	float timeStamp = 0.0f;
	bool isCharged = false;
	float currentKnockbackStrength = 0.0f;
	float punchDamage = 0.0f;

	private void Awake()
	{
		audioSource = GetComponent<AudioSource>();
		damageDealer = GetComponent<DamageDealer>();
		gloveRigidbody = GetComponent<Rigidbody>();

		currentKnockbackStrength = knockbackStrength;
		punchDamage = normalDamage;
	}

	private void FixedUpdate()
	{
		originalPosition = transform.parent.position;
		switch(weaponState)
		{
			case WeaponState.Firing:
				FiringRoutine();
				break;

			case WeaponState.Rearming:
				RearmingRoutine();
				break;

			case WeaponState.CoolingDown:
				CoolDownRoutine();
				break;

			default:
				break;
		}
	}

	private void FiringRoutine()
	{
		Vector3 targetPosition = originalPosition + (punchingArmLength * transform.forward);

		if(transform.position == targetPosition)
		{
			TransitionToRearmingState();
		}
		else
		{
			float maxDistanceStep = punchingArmLength / punchExtensionSteps;
			transform.position = Vector3.MoveTowards(transform.position, targetPosition, maxDistanceStep * Time.fixedDeltaTime);
		}
	}

	private void RearmingRoutine()
	{
		if(transform.position == originalPosition)
		{
			// State change
			TransitionToCooldownState();
		}
		else
		{
			float maxDistanceStep = punchingArmLength / punchExtensionSteps;
			transform.position = Vector3.MoveTowards(transform.position, originalPosition, maxDistanceStep * Time.fixedDeltaTime);
		}
	}

	private void CoolDownRoutine()
	{
		if(timeStamp <= Time.time)
		{
			timeStamp = 0.0f;
			weaponState = WeaponState.Ready;
		}
	}

	private void TransitionToFiringState()
	{
		if(isCharged)
		{
			currentKnockbackStrength = chargedKnockbackStrength;
			punchDamage = chargedDamage;
		}
		else
		{
			currentKnockbackStrength = knockbackStrength;
			punchDamage = normalDamage;
		}
		weaponState = WeaponState.Firing;
	}

	private void TransitionToRearmingState()
	{
		weaponState = WeaponState.Rearming;
		AnimateCancelCharge();
	}

	private void TransitionToCooldownState()
	{
		// Reset stats
		currentKnockbackStrength = knockbackStrength;
		punchDamage = normalDamage;

		weaponState = WeaponState.CoolingDown;
		timeStamp = Time.time + coolDownPeriodInSeconds;
	}

	private void OnCollisionEnter(Collision other)
	{
		if(weaponState == WeaponState.Firing)
		{
			// Process punch hit
			CreatePhysicalImpact(other);
			PlayPunchHitSound();
			if(damageDealer)
				damageDealer.DealDamage(other.gameObject, punchDamage, isCharged);

			// State change
			AnimateReleaseCharge();
			TransitionToRearmingState();
		}
	}

	private void OnCollisionStay(Collision other)
	{
		if(weaponState == WeaponState.Firing)
		{
			// Process punch hit
			CreatePhysicalImpact(other);
			PlayPunchHitSound();
			if(damageDealer)
				damageDealer.DealDamage(other.gameObject, punchDamage, isCharged);

			// State change
			AnimateReleaseCharge();
			TransitionToRearmingState();
		}
	}

	private void CreatePhysicalImpact(Collision other)
	{
		Rigidbody otherBody = other.rigidbody;

		// Force A -> B
		if(otherBody)
			otherBody.AddForceAtPosition(currentKnockbackStrength * transform.forward, other.contacts[0].point, ForceMode.Impulse);

		// Force A <- B
		if(mainRigidBody)
			mainRigidBody.AddForceAtPosition(recoilStrength * -transform.forward, other.contacts[0].point, ForceMode.Impulse);

		currentKnockbackStrength = knockbackStrength;
	}

	private void PlayPunchHitSound()
	{
		if(!audioSource || punchSounds.Length == 0)
			return;

		int index = Random.Range(0, punchSounds.Length);
		audioSource.volume = punchVolume;
		audioSource.clip = punchSounds[index];
		audioSource.Play();
	}

	private void AnimateChargePunch()
	{
		animatorController.SetTrigger("ChargePunch");
	}

	private void AnimateReleaseCharge()
	{
		if(isCharged)
		{
			isCharged = false;
			animatorController.SetTrigger("ReleaseCharge");
		}
	}

	private void AnimateCancelCharge()
	{
		if(isCharged)
		{
			isCharged = false;
			animatorController.SetTrigger("CancelCharge");
		}
	}

	/// <summary>
	/// Attack with main weapon function.
	/// </summary>
	public override void OnFire()
	{
		if(weaponState == WeaponState.Ready)
		{
			TransitionToFiringState();
		}
	}

	/// <summary>
	/// Charge the weapon.
	/// </summary>
	public override void OnCharge()
	{
		isCharged = true;
		AnimateChargePunch();
	}

	/// <summary>
	/// Attack and release the weapon's charge for more damage.
	/// </summary>
	public override void OnRelease()
	{
		if(weaponState == WeaponState.Ready)
		{
			TransitionToFiringState();
		}
	}
}
