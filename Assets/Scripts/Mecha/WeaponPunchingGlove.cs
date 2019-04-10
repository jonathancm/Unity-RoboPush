using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class WeaponPunchingGlove : MonoBehaviour
{
	enum WeaponState
	{
		Ready,
		Firing,
		CoolingDown
	};

	// Configurable Parameters
	[SerializeField] float punchingArmLength = 0.85f;
	[Range(0.001f, 1.0f)] [SerializeField] float punchExtensionSteps = 0.1f;

	[Header("Physics")]
	[SerializeField] float knockbackStrength = 100.0f;
	[SerializeField] float recoilStrength = 50.0f;
	[SerializeField] Rigidbody mainRigidBody = null;

	[Header("Sound Effects")]
	[SerializeField] AudioClip[] punchSounds = null;
	[Range(0.0f, 1.0f)] [SerializeField] float punchVolume = 0.5f;

	// Cached References
	AudioSource audioSource = null;

	// State variables
	WeaponState weaponState = WeaponState.Ready;
	Vector3 originalPosition;

	public void Fire()
	{
		if(weaponState == WeaponState.Ready)
			weaponState = WeaponState.Firing;
	}

	private void Awake()
	{
		audioSource = GetComponent<AudioSource>();
	}

	private void Update()
	{
		originalPosition = transform.parent.position;
		switch(weaponState)
		{
			case WeaponState.Firing:
				ExtendArm();
				break;

			case WeaponState.CoolingDown:
				RetractArm();
				break;

			default:
				break;
		}
	}

	private void ExtendArm()
	{
		Vector3 targetPosition = originalPosition + (punchingArmLength * transform.forward);

		if(transform.position == targetPosition)
		{
			weaponState = WeaponState.CoolingDown;
		}
		else
		{
			float maxDistanceStep = punchingArmLength / punchExtensionSteps;
			transform.position = Vector3.MoveTowards(transform.position, targetPosition, maxDistanceStep * Time.deltaTime);
		}
	}

	private void RetractArm()
	{
		if(transform.position == originalPosition)
		{
			weaponState = WeaponState.Ready;
		}
		else
		{
			float maxDistanceStep = punchingArmLength / punchExtensionSteps;
			transform.position = Vector3.MoveTowards(transform.position, originalPosition, maxDistanceStep * Time.deltaTime);
		}
	}

	private void OnCollisionEnter(Collision other)
	{
		if(weaponState == WeaponState.Firing)
		{
			CreatePhysicalImpact(other);
			PlayPunchHitSound();

			weaponState = WeaponState.CoolingDown;
		}
	}

	private void CreatePhysicalImpact(Collision other)
	{
		Rigidbody otherBody = other.rigidbody;
		if(otherBody)
			otherBody.AddForceAtPosition(knockbackStrength * transform.forward, other.contacts[0].point, ForceMode.Impulse);

		if(mainRigidBody)
			mainRigidBody.AddForceAtPosition(recoilStrength * -transform.forward, other.contacts[0].point, ForceMode.Impulse);
	}

	private void PlayPunchHitSound()
	{
		if(!audioSource || punchSounds.Length == 0)
			return;

		//if(audioSource.isPlaying)
		//	return;

		int index = Random.Range(0, punchSounds.Length);
		audioSource.volume = punchVolume;
		audioSource.clip = punchSounds[index];
		audioSource.Play();
	}
}
