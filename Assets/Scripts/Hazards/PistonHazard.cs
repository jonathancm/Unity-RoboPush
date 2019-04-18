using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class PistonHazard : MonoBehaviour
{
	enum HazardState
	{
		Ready,
		Firing,
		CoolingDown
	};

	// Configurable Parameters
	[SerializeField] Vector3 pistonTravelVector = new Vector3(0.0f, 2.0f, 0.0f);
	[SerializeField] float extendSteps = 20;
	[SerializeField] float retractSteps = 20;

	[Header("Sound Effect")]
	[SerializeField] AudioClip[] pistonSounds = null;
	[SerializeField] float pistonLaunchVolume = 0.5f;

	// Cached References
	AudioSource audioSource = null;
	DamageDealer damageDealer = null;

	// State variables
	HazardState hazardState = HazardState.Ready;
	Vector3 initialPosition;
	bool isPaused = false;

	private void Awake()
	{
		audioSource = GetComponent<AudioSource>();
		damageDealer = GetComponent<DamageDealer>();
	}

	private void Start()
    {
		initialPosition = transform.position;
		//AssignGameModeDelegates();
	}

	private void FixedUpdate()
	{
		if(isPaused)
			return;

		switch(hazardState)
		{
			case HazardState.Firing:
				ExtendPiston();
				break;

			case HazardState.CoolingDown:
				RetractPiston();
				break;

			default:
				break;
		}
	}

	private void ExtendPiston()
	{
		Vector3 targetPosition = initialPosition + pistonTravelVector;

		if(transform.position == targetPosition)
		{
			hazardState = HazardState.CoolingDown;
		}
		else
		{
			float maxDistanceStep = pistonTravelVector.magnitude / extendSteps;
			transform.position = Vector3.MoveTowards(transform.position, targetPosition, maxDistanceStep);
		}
	}

	private void RetractPiston()
	{
		if(transform.position == initialPosition)
		{
			hazardState = HazardState.Ready;
		}
		else
		{
			float maxDistanceStep = pistonTravelVector.magnitude / retractSteps;
			transform.position = Vector3.MoveTowards(transform.position, initialPosition, maxDistanceStep);
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if(hazardState != HazardState.Ready)
			return;

		Fire();
		PlayPistonLaunchSound();
	}

	private void OnTriggerExit(Collider other)
	{
		if(hazardState == HazardState.Ready)
			return;

		if(damageDealer)
			damageDealer.DealDamage(other.gameObject);
	}

	private void Fire()
	{
		hazardState = HazardState.Firing;
	}

	private void PlayPistonLaunchSound()
	{
		if(!audioSource || pistonSounds.Length == 0)
			return;

		if(audioSource.isPlaying)
			return;

		int index = Random.Range(0, pistonSounds.Length);
		audioSource.volume = pistonLaunchVolume;
		audioSource.clip = pistonSounds[index];
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
