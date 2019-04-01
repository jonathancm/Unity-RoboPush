using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MechaController : MonoBehaviour
{
	// Configuration Parameters
	[Header("Weapons")]
	[SerializeField] private MechaWeapon m_LeftWeapon = null;
	[SerializeField] private MechaWeapon m_RightWeapon = null;

	[Header("Movement")]
	[SerializeField] List<MechaWheel> wheels;
	[SerializeField] float accelerationTorque;
	[SerializeField] float brakeTorque;
	[SerializeField] float turnTorque;

	[Header("Physics")]
	[SerializeField] Transform CenterOfMass;

	// Cached References
	Rigidbody rigidBody;

	private void Awake()
	{
		rigidBody = GetComponent<Rigidbody>();
		rigidBody.centerOfMass = CenterOfMass.localPosition;
	}

	public void MoveFlyByWire(float throwAccel, float throwTurn, bool brakeButton)
	{
		// Clamp input values
		throwTurn = Mathf.Clamp(throwTurn, -1, 1);
		throwAccel = Mathf.Clamp(throwAccel, -1, 1);

		for(int i = 0; i < wheels.Count; i++)
		{
			wheels[i].Accelerate(throwAccel, accelerationTorque);
			TurnBody(throwTurn, turnTorque);

			if(brakeButton) // TODO: should this be moved out of this function?
				wheels[i].Brake(brakeTorque);

			wheels[i].ApplyLocalPositionToVisuals(); // TODO: is there a way to decouple this mechanism from this function?
		}
	}

	public void MoveManually(float throwLV, float throwRV, bool brakeButton)
	{
		// Clamp input values
		throwLV = Mathf.Clamp(throwLV, -1, 1);
		throwRV = Mathf.Clamp(throwRV, -1, 1);

		//float thrustPerLeftWheel = throwLV * (m_FullTorqueOverAllWheels / (m_LeftWheels.Length + m_RightWheels.Length));
		//float thrustPerRightWheel = throwRV * (m_FullTorqueOverAllWheels / (m_LeftWheels.Length + m_RightWheels.Length));
		//foreach(var wheel in m_LeftWheels)
		//{
		//	wheel.ApplyTorque(thrustPerLeftWheel);
		//}
		//foreach(var wheel in m_RightWheels)
		//{
		//	wheel.ApplyTorque(thrustPerRightWheel);
		//}
	}

	private void TurnBody(float throwTurn, float torqueAmount)
	{
		if(!rigidBody)
			return;

		Vector3 rotationVelocity = transform.up * throwTurn * torqueAmount;
		rigidBody.AddTorque(rotationVelocity, ForceMode.Force);
	}


	public void FirePrimaryWeapon()
	{
		if(!m_LeftWeapon)
			return;

		m_LeftWeapon.OnFire();
	}

	public void FireSecondaryWeapon()
	{
		if(!m_RightWeapon)
			return;

		m_RightWeapon.OnFire();
	}
}
