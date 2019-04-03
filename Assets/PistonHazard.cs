﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PistonHazard : MonoBehaviour
{
	// Configurable Parameters
	[SerializeField] Vector3 pistonTravelVector = new Vector3(0.0f, 2.0f, 0.0f);
	[SerializeField] float extendSteps = 2;
	[SerializeField] float retractSteps = 2;

	enum HazardState
	{
		Ready,
		Firing,
		CoolingDown
	};

	// State variables
	HazardState hazardState = HazardState.Ready;
	Vector3 initialPosition;

    void Start()
    {
		initialPosition = transform.position;
    }

	private void FixedUpdate()
	{
		//initialPosition = transform.parent.transform.position; // Allows object to be moved at runtime

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
		Fire();
	}

	private void Fire()
	{
		if(hazardState == HazardState.Ready)
			hazardState = HazardState.Firing;
	}
}