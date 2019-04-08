using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepulsorHazard : MonoBehaviour
{
	// Configurable Parameters
	[SerializeField] float gravityStrength = 900.0f;
	[SerializeField] float pulseFrequency = 1.0f;

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
		Vector3 pushDirection = other.transform.position - transform.position;
		float pullStrength = (gravityStrength * pulseStrength) / (pushDirection.magnitude * pushDirection.magnitude);

		Vector3 pullForce = pushDirection.normalized * pullStrength;
		otherBody.AddForce(pullForce, ForceMode.Force);
	}
}
