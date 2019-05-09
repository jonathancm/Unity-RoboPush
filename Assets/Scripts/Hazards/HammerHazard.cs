using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammerHazard : GameTimeObject
{
	enum HazardState
	{
		Ready,
		Firing,
		Rearming,
		CoolingDown
	};

	// Configurable Parameters
	[Header("Setup")]
	[SerializeField] GameObject hammer = null;
	[SerializeField] BoxCollider hazardTrigger = null;

	[Header("Animation")]
	[SerializeField] float fallAngle = 85.0f;
	[SerializeField] float fallSteps = 40;
	[SerializeField] float rearmingSteps = 120;
	[SerializeField] float coolDownPeriodInSeconds = 1.5f;

	[Header("Damage")]
	[SerializeField] float damageAmount = 7.0f;

	[Header("Physics")]
	[SerializeField] float knockbackStrength = 100.0f;

	[Header("Sound Effect")]
	[SerializeField] float hammerHitVolume = 0.5f;

	// Cached References
	AudioSource audioSource = null;
	DamageDealer damageDealer = null;

	// State variables
	HazardState hazardState = HazardState.Ready;
	Quaternion initialRotation;
	float timeStamp = 0.0f;

	private void Awake()
	{
		initialRotation = hammer.transform.rotation;
		audioSource = GetComponent<AudioSource>();
		damageDealer = GetComponent<DamageDealer>();
	}

	private void FixedUpdate()
	{
		switch(hazardState)
		{
			case HazardState.Firing:
				FireHammer();
				break;

			case HazardState.Rearming:
				RetractHammer();
				break;

			case HazardState.CoolingDown:
				CoolDown();
				break;

			default:
				break;
		}
	}

	private void FireHammer()
	{
		Quaternion currentRotation = hammer.transform.rotation;
		Quaternion targetRotation = Quaternion.Euler(new Vector3(fallAngle, initialRotation.eulerAngles.y, initialRotation.eulerAngles.z));

		if(IsSameRotation(currentRotation, targetRotation))
		{
			hazardState = HazardState.Rearming;
		}
		else
		{
			float maxStepDeltaDegrees = Mathf.Abs(fallAngle - initialRotation.eulerAngles.x) / fallSteps;
			hammer.transform.rotation = Quaternion.RotateTowards(currentRotation, targetRotation, maxStepDeltaDegrees);
		}
	}

	private void RetractHammer()
	{
		Quaternion currentRotation = hammer.transform.rotation;

		if(IsSameRotation(currentRotation, initialRotation))
		{
			timeStamp = Time.time + coolDownPeriodInSeconds;
			hazardState = HazardState.CoolingDown;
		}
		else
		{
			float maxStepDeltaDegrees = Mathf.Abs(fallAngle - initialRotation.eulerAngles.x) / rearmingSteps;
			hammer.transform.rotation = Quaternion.RotateTowards(currentRotation, initialRotation, maxStepDeltaDegrees);
		}
	}

	private bool IsSameRotation(Quaternion q1, Quaternion q2)
	{
		return (Quaternion.Angle(q1, q2) < 0.001f);
	}

	private void CoolDown()
	{
		if(timeStamp <= Time.time)
		{
			timeStamp = 0.0f;
			hazardState = HazardState.Ready;
		}
	}

	private void OnTriggerStay(Collider other)
	{
		// Fire
		if(hazardState == HazardState.Ready)
			hazardState = HazardState.Firing;
	}

	private void OnCollisionEnter(Collision collision)
	{
		if(hazardState != HazardState.Firing)
			return;

		CreatePhysicalImpact(collision);
		PlayHammerHitSound();

		if(damageDealer)
			damageDealer.DealDamage(collision.gameObject, damageAmount, false);

		hazardState = HazardState.Rearming;
	}

	private void PlayHammerHitSound()
	{
		if(!audioSource)
			return;

		if(audioSource.isPlaying)
			return;

		audioSource.volume = hammerHitVolume;
		audioSource.Play();
	}

	private void CreatePhysicalImpact(Collision other)
	{
		Rigidbody otherBody = other.rigidbody;
		if(otherBody && hammer)
			otherBody.AddForceAtPosition(knockbackStrength * hammer.transform.forward, other.contacts[0].point, ForceMode.Impulse);
	}

	/// <summary>
	/// Pause game object activity.
	/// </summary>
	public override void OnPause()
	{
		// Disable Update() and FixedUpdate()
		this.enabled = false;

		// Disable trigger volume
		if(hazardTrigger)
			hazardTrigger.enabled = false;

		// Pause Audio
		if(audioSource && audioSource.isPlaying)
			audioSource.Pause();
	}

	/// <summary>
	/// Un-pause game object activity.
	/// </summary>
	public override void OnResume()
	{
		// Enable Update() and FixedUpdate()
		this.enabled = true;

		// Enable trigger volume
		if(hazardTrigger)
			hazardTrigger.enabled = true;

		// Resume Audio
		if(audioSource)
			audioSource.UnPause();
	}

	/// <summary>
	/// Prepare game object for game end.
	/// </summary>
	public override void OnGameOver()
	{
		// Disable Update() and FixedUpdate()
		this.enabled = false;

		// Disable trigger volume
		if(hazardTrigger)
			hazardTrigger.enabled = false;
	}
}
