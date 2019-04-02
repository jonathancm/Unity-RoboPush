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
	[SerializeField] float accelerationTorque = 800.0f;
	[SerializeField] float brakeTorque = 3000.0f;
	[SerializeField] float turnTorque = 3000.0f;

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

		TurnBody(throwTurn, turnTorque);
		for(int i = 0; i < wheels.Count; i++)
		{
			wheels[i].Accelerate(throwAccel, accelerationTorque);

			if(brakeButton) // TODO: should this be moved out of this function?
				wheels[i].Brake(brakeTorque);

			wheels[i].ApplyLocalPositionToVisuals(); // TODO: is there a way to decouple this mechanism from this function?
		}
	}

	public void MoveManually(float throwLV, float throwRV, bool brakeButton)
	{
		// Clamp input values
		throwLV = Mathf.Clamp(throwLV, -1.0f, 1.0f);
		throwRV = Mathf.Clamp(throwRV, -1.0f, 1.0f);

		float throwAccel = (throwLV + throwRV) / 2.0f;
		float throwTurn = (throwLV - throwRV) / 2.0f;

		TurnBody(throwTurn, turnTorque);
		for(int i = 0; i < wheels.Count; i++)
		{
			wheels[i].Accelerate(throwAccel, accelerationTorque);

			if(brakeButton) // TODO: should this be moved out of this function?
				wheels[i].Brake(brakeTorque);

			wheels[i].ApplyLocalPositionToVisuals(); // TODO: is there a way to decouple this mechanism from this function?
		}
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
