using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class MechaController : MonoBehaviour
{
	// Configuration Parameters
	[Header("Weapons")]
	[SerializeField] private MechaWeapon m_LeftWeapon = null;
	[SerializeField] private MechaWeapon m_RightWeapon = null;

	[Header("Movement")]
	[SerializeField] List<MechaWheel> wheels;
	[SerializeField] float accelerationTorque = 3000.0f;
	[SerializeField] float turnTorque = 450.0f;

	[Header("Physics")]
	[SerializeField] Rigidbody mainRigidBody = null;
	[SerializeField] Transform CenterOfMass = null;

	private void Awake()
	{
		if(mainRigidBody && CenterOfMass)
			mainRigidBody.centerOfMass = CenterOfMass.localPosition;

		IgnoreSelfCollisions();
	}

	private void IgnoreSelfCollisions()
	{
		Collider[] colliders = GetComponentsInChildren<Collider>();
		for(int i = 0; i < colliders.Length; i++)
		{
			for(int j = i; j < colliders.Length; j++)
			{
				Physics.IgnoreCollision(colliders[i], colliders[j]);
			}
		}
	}

	public void MoveFlyByWire(float throwAccel, float throwTurn)
	{
		// Clamp input values
		throwTurn = Mathf.Clamp(throwTurn, -1.0f, 1.0f);
		throwAccel = Mathf.Clamp(throwAccel, -1.0f, 1.0f);

		DriveWheels(throwAccel, throwTurn);
	}

	private void DriveWheels(float throwAccel, float throwTurn)
	{
		TurnBody(throwTurn, turnTorque);
		for(int i = 0; i < wheels.Count; i++)
		{
			wheels[i].Accelerate(throwAccel, accelerationTorque);
		}
	}

	private void TurnBody(float throwTurn, float torqueAmount)
	{
		if(!mainRigidBody)
			return;

		Vector3 rotationVelocity = transform.up * throwTurn * torqueAmount;
		mainRigidBody.AddTorque(rotationVelocity, ForceMode.Force);
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
