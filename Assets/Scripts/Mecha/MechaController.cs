using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MechaController : MonoBehaviour
{
	// Configurable Parameters
	[Header("Weapons")]
	[SerializeField] private MechaWeapon m_LeftWeapon = null;
	[SerializeField] private MechaWeapon m_RightWeapon = null;

	[Header("Movement")]
	[SerializeField] private MechaWheel[] m_LeftWheels = null;
	[SerializeField] private MechaWheel[] m_RightWheels = null;
	[SerializeField] private float m_FullTorqueOverAllWheels = 100.0f;
	//[SerializeField] private Vector3 m_CentreOfMassOffset = new Vector3(0.0f,0.0f, 0.0f);

	public void MoveFlyByWire(float throwAccel, float throwTurn, float throwBrake)
	{
		// Clamp input values
		throwTurn = Mathf.Clamp(throwTurn, -1, 1);
		throwAccel = Mathf.Clamp(throwAccel, -1, 1);
		throwBrake = -1 * Mathf.Clamp(throwBrake, -1, 0);

		ApplyDriveFlyByWire(throwAccel, throwTurn);
	}

	void ApplyDriveFlyByWire(float throwAccel, float throwTurn)
	{
		float thrustPerWheel = m_FullTorqueOverAllWheels / (m_LeftWheels.Length + m_RightWheels.Length);
		if(Mathf.Abs(throwTurn) > 0.25f)
			Turn(throwTurn * thrustPerWheel);
		else
			Accelerate(throwAccel * thrustPerWheel);
	}

	private void Accelerate(float thrustPerWheel)
	{
		foreach(var wheel in m_LeftWheels)
		{
			wheel.ApplyTorque(thrustPerWheel);
		}
		foreach(var wheel in m_RightWheels)
		{
			wheel.ApplyTorque(thrustPerWheel);
		}
	}

	private void Turn(float thrustPerWheel)
	{
		foreach(var wheel in m_LeftWheels)
		{
			wheel.ApplyTorque(thrustPerWheel);
		}

		foreach(var wheel in m_RightWheels)
		{
			wheel.ApplyTorque(-thrustPerWheel);
		}
	}

	public void MoveManually(float throwLV, float throwRV)
	{
		float thrustPerLeftWheel = throwLV * (m_FullTorqueOverAllWheels / (m_LeftWheels.Length + m_RightWheels.Length));
		float thrustPerRightWheel = throwRV * (m_FullTorqueOverAllWheels / (m_LeftWheels.Length + m_RightWheels.Length));
		foreach(var wheel in m_LeftWheels)
		{
			wheel.ApplyTorque(thrustPerLeftWheel);
		}
		foreach(var wheel in m_RightWheels)
		{
			wheel.ApplyTorque(thrustPerRightWheel);
		}
	}

	public void FirePrimaryWeapon()
	{
		m_LeftWeapon.OnFire();
	}

	public void FireSecondaryWeapon()
	{
		m_RightWeapon.OnFire();
	}
}
