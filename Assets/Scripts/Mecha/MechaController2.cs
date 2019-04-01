using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MechaController2 : MonoBehaviour
{
	// Configuration Parameters
	[Header("Movement")]
	[SerializeField] List<MechaWheel2> wheels;
	[SerializeField] float accelerationTorque;
	[SerializeField] float brakeTorque;
	[SerializeField] float turnTorque;

	[Header("Physics")]
	[SerializeField] Transform CenterOfMass;

	// Cached References
	Rigidbody rigidBody;

	// State Variables
	float throwAccel;
	float throwTurn;
	//float throwBrake;

	private void Awake()
	{
		rigidBody = GetComponent<Rigidbody>();
		rigidBody.centerOfMass = CenterOfMass.localPosition;
	}

	// Update values related to physics
	void FixedUpdate()
	{
		throwAccel = Input.GetAxis("IntendAccelerate");
		throwTurn = Input.GetAxis("IntendTurn");
		//throwBrake = Input.GetAxis("IntendBrake");

		for(int i = 0; i < wheels.Count; i++)
		{
			wheels[i].Accelerate(throwAccel, accelerationTorque);
			TurnBody(throwTurn, turnTorque);
			if(Input.GetKey(KeyCode.Space))
				wheels[i].Brake(brakeTorque);

			wheels[i].ApplyLocalPositionToVisuals();
		}
	}

	private void TurnBody(float throwTurn, float torqueAmount)
	{
		if(!rigidBody)
			return;

		Vector3 rotationVelocity = transform.up * throwTurn * torqueAmount;
		rigidBody.AddTorque(rotationVelocity, ForceMode.Force);
	}

}
