using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammerHazard : MonoBehaviour
{
	// Configurable Parameters
	[SerializeField] GameObject hammer = null;
	[SerializeField] float fallAngle = 85.0f;
	[SerializeField] float fallSteps = 2;
	[SerializeField] float cooldownSteps = 2;

	enum HazardState
	{
		Ready,
		Firing,
		CoolingDown
	};

	// State variables
	HazardState hazardState = HazardState.Ready;
	Vector3 initialDirection;
	Vector3 initialRotation;


	void Start()
	{
		initialDirection = hammer.transform.forward;
		initialRotation = hammer.transform.localEulerAngles;
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
		Vector3 currentRotation = hammer.transform.localEulerAngles;
		Vector3 targetRotation = initialRotation;
		targetRotation.x = fallAngle;

		if(IsSameAngle(currentRotation, targetRotation))
		{
			hazardState = HazardState.CoolingDown;
		}
		else
		{
			float maxStepSize = Mathf.Abs(fallAngle - initialRotation.x) / fallSteps;
			hammer.transform.localEulerAngles = Vector3.RotateTowards(currentRotation, targetRotation, maxStepSize, 1.0f);
		}
	}

	private void RetractHammer()
	{
		Vector3 currentDirection = hammer.transform.localEulerAngles;

		if(IsSameAngle(currentDirection, initialRotation))
		{
			hazardState = HazardState.Ready;
		}
		else
		{
			float maxStepSize = (fallAngle - initialRotation.x) / cooldownSteps;
			hammer.transform.localEulerAngles = Vector3.RotateTowards(currentDirection, initialRotation, maxStepSize, 1.0f);
		}
	}

	bool IsSameDirection(Vector3 a, Vector3 b)
	{
		return Vector3.Dot(a, b) > 0.990f;
	}

	bool IsSameAngle(Vector3 a1, Vector3 a2)
	{
		Quaternion q1 = Quaternion.Euler(a1);
		Quaternion q2 = Quaternion.Euler(a2);

		float angle = Quaternion.Angle(q1, q2);

		return (angle < 0.001f);
	}

	private void OnTriggerStay(Collider other)
	{
		Fire();
	}

	private void Fire()
	{
		if(hazardState == HazardState.Ready)
			hazardState = HazardState.Firing;
	}
}
