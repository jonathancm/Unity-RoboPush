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

	/// <summary>
	/// Rotate wheel to generate vehicle acceleration.
	/// </summary>
	/// <param name="torqueAmount">Amount of torque provided by motor. More torque means more movement power.</param>
	public void Accelerate(float torqueAmount)
	{
		if(!wheelBody || torqueAmount == 0.0f)
			return;

		Vector3 torqueForce = torqueAmount * wheelBody.transform.right;
		wheelBody.AddTorque(torqueForce, ForceMode.Force);
	}

	/// <summary>
	/// Pause game object activity.
	/// </summary>
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

	/// <summary>
	/// Un-pause game object activity.
	/// </summary>
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

	/// <summary>
	/// Prepare game object for game end.
	/// </summary>
	public override void OnGameOver()
	{
		// Nothing special
	}
}
