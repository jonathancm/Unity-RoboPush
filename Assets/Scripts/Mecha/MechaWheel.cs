using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MechaWheel : MonoBehaviour
{
	// Configurable Parameters
	[SerializeField] Rigidbody wheelBody = null;
	[SerializeField] float maxWheelRPM = 500.0f;

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

	public void Brake(float brakeTorque)
	{
		
	}
}
