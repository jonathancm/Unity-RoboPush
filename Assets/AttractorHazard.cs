using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttractorHazard : MonoBehaviour
{
	// Configurable Parameters
	[SerializeField] float gravityStrength = 9.81f;

	private void OnTriggerStay(Collider other)
	{
		Rigidbody otherBody = other.attachedRigidbody;
		if(!otherBody)
			return;

		// TODO: Implement pulsing force to give a chance to player to release from grasp

		Vector3 pullDirection = transform.position - other.transform.position;
		float pullStrength = gravityStrength / pullDirection.magnitude;
		Vector3 pullForce = pullDirection.normalized * pullStrength;
		otherBody.AddForce(pullForce, ForceMode.Force);
	}
}
