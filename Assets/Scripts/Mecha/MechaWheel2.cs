using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MechaWheel2 : MonoBehaviour
{
	// Configurable Parameters
	[SerializeField] WheelCollider wheelCollider = null;
	[SerializeField] GameObject wheelMesh = null;
	[SerializeField] bool isLeftWheel = false;
	[SerializeField] float maxVisibleRPM = 125.0f;
	[SerializeField] float decelerationForce = 150.0f;

	// State Variable
	Vector3 localEuler = Vector3.zero;

	public void ApplyLocalPositionToVisuals()
	{
		Vector3 position;
		Quaternion rotation;

		wheelCollider.GetWorldPose(out position, out rotation);
		UpdateLocalEuler();

		wheelMesh.transform.position = position;
		wheelMesh.transform.localEulerAngles = localEuler;
	}

	void UpdateLocalEuler()
	{
		float degreesPerFixedStep;
		Vector3 deltaLocalEuler;

		if(Mathf.Abs(wheelCollider.rpm) > maxVisibleRPM)
		{
			degreesPerFixedStep = Mathf.Sign(wheelCollider.rpm) * maxVisibleRPM / 60.0f * Time.fixedDeltaTime * 360.0f;
		}
		else
		{
			degreesPerFixedStep = wheelCollider.rpm / 60.0f * Time.fixedDeltaTime * 360.0f;
		}

		deltaLocalEuler = Vector3.right * degreesPerFixedStep;
		localEuler += deltaLocalEuler;
	}

	public void Accelerate(float throwAccel, float torqueAmount)
	{
		if(throwAccel != 0f)
		{
			wheelCollider.brakeTorque = 0;
			wheelCollider.motorTorque = throwAccel * torqueAmount;
		}
		else
		{
			Decelerate(decelerationForce);
		}
	}

	public void Decelerate(float decelerationForce)
	{
		wheelCollider.brakeTorque = decelerationForce;
	}

	public void Brake(float brakeTorque)
	{
		wheelCollider.brakeTorque = brakeTorque;
	}
}
