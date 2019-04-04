using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class SawHazard : MonoBehaviour
{
	// Configurable Parameters
	[SerializeField] GameObject sawMesh = null;
	[SerializeField] float cosmeticTurnsPerSecond = 1.0f;
	[SerializeField] float cuttingForce = 1000.0f;

	void Update()
	{
		if(sawMesh)
			sawMesh.transform.Rotate(cosmeticTurnsPerSecond * 360.0f * Time.deltaTime, 0.0f, 0.0f);
	}

	private void OnTriggerStay(Collider other)
	{
		Rigidbody otherBody = other.attachedRigidbody;
		if(!otherBody)
			return;

		otherBody.AddForceAtPosition(cuttingForce * transform.forward, transform.position, ForceMode.Impulse);
	}
}
