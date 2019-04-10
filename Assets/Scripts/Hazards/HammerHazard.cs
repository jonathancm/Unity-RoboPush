﻿using System.Collections;
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

	// State variables
	HazardState hazardState = HazardState.Ready;
	Quaternion initialRotation;

	void Start()
	{
		audioSource = GetComponent<AudioSource>();
		initialRotation = hammer.transform.rotation;
	}

	private void FixedUpdate()
	{
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

	bool IsSameRotation(Quaternion q1, Quaternion q2)
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
}