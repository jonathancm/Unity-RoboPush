using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class MechaController : GameTimeObject
{
	// Configuration Parameters
	[Header("Weapons")]
	[SerializeField] private MechaWeapon m_LeftWeapon = null;
	[SerializeField] private MechaWeapon m_RightWeapon = null;

	[Header("Movement")]
	[SerializeField] List<MechaWheel> wheels = null;
	[SerializeField] float accelerationTorque = 3000.0f;
	[SerializeField] float turnTorque = 450.0f;

	[Header("Physics")]
	[SerializeField] Rigidbody mainRigidBody = null;

	// State variables
	Vector3 savedVelocity;
	Vector3 savedAngularVelocity;

	private void Awake()
	{
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

	public void Move(float throwAccel, float throwTurn)
	{
		// Clamp input values
		throwAccel = Mathf.Clamp(throwAccel, -1.0f, 1.0f);
		throwTurn = Mathf.Clamp(throwTurn, -1.0f, 1.0f);

		TurnBody(throwTurn, turnTorque);
		DriveWheels(throwAccel, throwTurn);
	}

	private void TurnBody(float throwTurn, float torqueAmount)
	{
		if(!mainRigidBody)
			return;

		Vector3 rotationVelocity = transform.up * throwTurn * torqueAmount;
		mainRigidBody.AddTorque(rotationVelocity, ForceMode.Force);
	}

	private void DriveWheels(float throwAccel, float throwTurn)
	{
		for(int i = 0; i < wheels.Count; i++)
		{
			wheels[i].Accelerate(throwAccel, accelerationTorque);
		}
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

	public override void OnPause()
	{
		if(mainRigidBody)
		{
			savedAngularVelocity = mainRigidBody.angularVelocity;
			savedVelocity = mainRigidBody.velocity;
			mainRigidBody.isKinematic = true;
		}
	}

	public override void OnResume()
	{
		if(mainRigidBody)
		{
			mainRigidBody.isKinematic = false;
			mainRigidBody.velocity = savedVelocity;
			mainRigidBody.angularVelocity = savedAngularVelocity;
		}
	}

	public override void OnGameOver()
	{
		// Nothing special
	}
}
