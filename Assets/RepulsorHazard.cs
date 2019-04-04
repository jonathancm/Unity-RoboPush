using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepulsorHazard : MonoBehaviour
{
	// Configurable Parameters
	[SerializeField] float gravityStrength = 9.81f;

	private void OnTriggerStay(Collider other)
	{
		Rigidbody otherBody = other.attachedRigidbody;
		if(!otherBody)
			return;

		Vector3 pushDirection = other.transform.position - transform.position;
		float pushStrength = gravityStrength / pushDirection.magnitude;
		Vector3 pushForce = pushDirection.normalized * pushStrength;
		otherBody.AddForce(pushForce, ForceMode.Force);
	}
}
