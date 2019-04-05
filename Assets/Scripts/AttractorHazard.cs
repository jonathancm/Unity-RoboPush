using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using NaughtyAttributes;

public class AttractorHazard : MonoBehaviour
{
	// Configurable Parameters
	[SerializeField] float gravityStrength = 9.81f;
	[SerializeField] float pulseFrequency = 0.5f;

	// State Variable
	float pulseStrength = 1.0f;

	private void CalculatePulseStrength()
	{
		if(pulseFrequency > 0.0f)
			pulseStrength = 0.5f * Mathf.Cos(Time.time * pulseFrequency) + 0.5f;
		else
			pulseStrength = 1.0f;
	}

	private void OnTriggerStay(Collider other)
	{
		Rigidbody otherBody = other.attachedRigidbody;
		if(!otherBody)
			return;

		CalculatePulseStrength();

		// F_grav = 1 / d^2
		Vector3 pullDirection = transform.position - other.transform.position;
		float pullStrength = (gravityStrength * pulseStrength) / (pullDirection.magnitude * pullDirection.magnitude);

		Vector3 pullForce = pullDirection.normalized * pullStrength;
		otherBody.AddForce(pullForce, ForceMode.Force);
	}
}
