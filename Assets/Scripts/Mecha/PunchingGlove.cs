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
	[Header("Setup")]
	[SerializeField] Rigidbody mainRigidBody = null;

	[Header("Animation")]
	[SerializeField] float coolDownPeriodInSeconds = 1.5f;
	[SerializeField] float punchingArmLength = 0.85f;
	[SerializeField] [Range(0.001f, 1.0f)] float punchExtensionSteps = 0.1f;
	[SerializeField] PunchingGloveEmission emissionController = null;

	[Header("Physics")]
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

	public override void OnFire()
	{
		if(weaponState == WeaponState.Ready)
		{
			currentKnockbackStrength = knockbackStrength;
			weaponState = WeaponState.Firing;
		}
	}

	public override void OnCharge()
	{
		isCharged = true;
		emissionController.StartColorEmission();
	}

	public override void OnRelease()
	{
		if(!isCharged)
			return;

		if(weaponState == WeaponState.Ready)
		{
			currentKnockbackStrength = chargedKnockbackStrength;
			weaponState = WeaponState.Firing;
		}
		isCharged = false;
		emissionController.StopColorEmission();
	}

	private void Awake()
	{
		audioSource = GetComponent<AudioSource>();
		damageDealer = GetComponent<DamageDealer>();
		gloveRigidbody = GetComponent<Rigidbody>();
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
			weaponState = WeaponState.Rearming;
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
			timeStamp = Time.time + coolDownPeriodInSeconds;
			weaponState = WeaponState.CoolingDown;
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

	private void OnCollisionEnter(Collision other)
	{
		if(weaponState == WeaponState.Firing)
		{
			weaponState = WeaponState.Rearming;

			CreatePhysicalImpact(other);
			PlayPunchHitSound();

			if(damageDealer)
				damageDealer.DealDamage(other.gameObject);
		}
	}

	private void OnCollisionStay(Collision other)
	{
		if(weaponState == WeaponState.Firing)
		{
			weaponState = WeaponState.Rearming;

			CreatePhysicalImpact(other);
			PlayPunchHitSound();

			if(damageDealer)
				damageDealer.DealDamage(other.gameObject);
		}
	}

	private void CreatePhysicalImpact(Collision other)
	{
		Rigidbody otherBody = other.rigidbody;
		if(otherBody)
			otherBody.AddForceAtPosition(currentKnockbackStrength * transform.forward, other.contacts[0].point, ForceMode.Impulse);

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
}
