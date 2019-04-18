using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammerHazard : MonoBehaviour
{
	enum HazardState
	{
		Ready,
		Firing,
		CoolingDown
	};

	// Configurable Parameters
	[SerializeField] GameObject hammer = null;
	[SerializeField] float fallAngle = 85.0f;
	[SerializeField] float fallSteps = 40;
	[SerializeField] float cooldownSteps = 40;

	[Header("Sound Effect")]
	[SerializeField] float hammerHitVolume = 0.5f;

	// Cached References
	AudioSource audioSource = null;
	DamageDealer damageDealer = null;

	// State variables
	HazardState hazardState = HazardState.Ready;
	Quaternion initialRotation;
	bool isPaused = false;

	private void Awake()
	{
		initialRotation = hammer.transform.rotation;
		audioSource = GetComponent<AudioSource>();
		damageDealer = GetComponent<DamageDealer>();
	}

	private void Start()
	{
		//AssignGameModeDelegates();
	}

	private void FixedUpdate()
	{
		if(isPaused)
			return;

		switch(hazardState)
		{
			case HazardState.Firing:
				FireHammer();
				break;

			case HazardState.CoolingDown:
				RetractHammer();
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
			hazardState = HazardState.CoolingDown;
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
			hazardState = HazardState.Ready;
		}
		else
		{
			float maxStepDeltaDegrees = Mathf.Abs(fallAngle - initialRotation.eulerAngles.x) / cooldownSteps;
			hammer.transform.rotation = Quaternion.RotateTowards(currentRotation, initialRotation, maxStepDeltaDegrees);
		}
	}

	private bool IsSameRotation(Quaternion q1, Quaternion q2)
	{
		return (Quaternion.Angle(q1, q2) < 0.001f);
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

		PlayHammerHitSound();
		if(damageDealer)
			damageDealer.DealDamage(collision.gameObject);
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

	private void AssignGameModeDelegates()
	{
		GameModeLogic gameModeLogic = FindObjectOfType<GameModeLogic>();
		if(gameModeLogic)
		{
			gameModeLogic.onPause += OnPause;
			gameModeLogic.onResume += OnResume;
		}
	}

	private void OnPause()
	{
		isPaused = true;
	}

	private void OnResume()
	{
		isPaused = false;
	}
}
