using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MechaWheel : GameTimeObject
{
	// Configurable Parameters
	[SerializeField] Rigidbody wheelBody = null;
	[SerializeField] Rigidbody springBody = null;
	[SerializeField] float maxWheelRPM = 500.0f;

	// State variable
	Vector3 savedSpringVelocity;
	Vector3 savedSpringAngularVelocity;
	Vector3 savedWheelVelocity;
	Vector3 savedWheelAngularVelocity;

	private void Awake()
	{
		if(wheelBody)
			wheelBody.maxAngularVelocity = (2.0f * Mathf.PI) * (maxWheelRPM / 60.0f);
	}

	public void Accelerate(float throwAccel, float torqueAmount)
	{
		if(wheelBody && throwAccel == 0.0f)
			return;

		Vector3 torqueForce = throwAccel * torqueAmount * wheelBody.transform.right;
		wheelBody.AddTorque(torqueForce, ForceMode.Force);
	}

	public override void OnPause()
	{
		if(springBody)
		{
			savedSpringAngularVelocity = springBody.angularVelocity;
			savedSpringVelocity = springBody.velocity;
			springBody.isKinematic = true;
		}

		if(wheelBody)
		{
			savedWheelAngularVelocity = wheelBody.angularVelocity;
			savedWheelVelocity = wheelBody.velocity;
			wheelBody.isKinematic = true;
		}
	}

	public override void OnResume()
	{
		if(springBody)
		{
			springBody.isKinematic = false;
			springBody.velocity = savedSpringVelocity;
			springBody.angularVelocity = savedSpringAngularVelocity;
		}

		if(wheelBody)
		{
			wheelBody.isKinematic = false;
			wheelBody.velocity = savedWheelVelocity;
			wheelBody.angularVelocity = savedWheelAngularVelocity;
		}
	}
}
