using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammerHazard : MonoBehaviour
{
	// Configurable Parameters
	[SerializeField] GameObject hammer = null;
	[SerializeField] float fallAngle = 85.0f;
	[SerializeField] float hitSteps = 2;
	[SerializeField] float cooldownSteps = 2;

	enum HazardState
	{
		Ready,
		Firing,
		CoolingDown
	};

	// State variables
	HazardState hazardState = HazardState.Ready;
	Vector3 initialRotation;

	//void Start()
	//{
	//	initialRotation = transform.eulerAngles;
	//}

	//private void FixedUpdate()
	//{
	//	//initialPosition = transform.parent.transform.position; // Allows object to be moved at runtime

	//	switch(hazardState)
	//	{
	//		case HazardState.Firing:
	//			ExtendPiston();
	//			break;

	//		case HazardState.CoolingDown:
	//			RetractPiston();
	//			break;

	//		default:
	//			break;
	//	}
	//}

	//private void ExtendPiston()
	//{
	//	Vector3 targetPosition = new Vector3();

	//	if(transform.position == targetPosition)
	//	{
	//		hazardState = HazardState.CoolingDown;
	//	}
	//	else
	//	{
	//		float maxDistanceStep = pistonTravelVector.magnitude / hitSteps;
	//		transform.position = Vector3.MoveTowards(transform.position, targetPosition, maxDistanceStep);
	//	}
	//}

	//private void RetractPiston()
	//{
	//	if(transform.position == initialRotation)
	//	{
	//		hazardState = HazardState.Ready;
	//	}
	//	else
	//	{
	//		float maxDistanceStep = pistonTravelVector.magnitude / cooldownSteps;
	//		transform.position = Vector3.MoveTowards(transform.position, initialRotation, maxDistanceStep);
	//	}
	//}

	//private void OnTriggerEnter(Collider other)
	//{
	//	Fire();
	//}

	//private void Fire()
	//{
	//	if(hazardState == HazardState.Ready)
	//		hazardState = HazardState.Firing;
	//}
}
