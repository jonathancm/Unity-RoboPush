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
	[SerializeField] float turnSpeed = 5.0f;

	[Header("Physics")]
	[SerializeField] Rigidbody mainRigidBody = null;
	[SerializeField] Transform centerOfMass = null;

	// State variables
	Vector3 savedVelocity;
	Vector3 savedAngularVelocity;

	private void Awake()
	{
		if(mainRigidBody && centerOfMass)
			mainRigidBody.centerOfMass = centerOfMass.localPosition;

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

	/// <summary>
	/// Connect player input into the movement controls.
	/// </summary>
	/// <param name="throwAccel">Amount of acceleration to apply. Provided by player input.</param>
	/// <param name="throwAccel">Amount of turn to apply. Provided by player input.</param>
	public void Move(float throwAccel, float throwTurn)
	{
		// Clamp input values
		throwAccel = Mathf.Clamp(throwAccel, -1.0f, 1.0f);
		throwTurn = Mathf.Clamp(throwTurn, -1.0f, 1.0f);

		TurnBody(throwTurn);
		DriveWheels(throwAccel);
	}

	private void TurnBody(float throwTurn)
	{
		if(!mainRigidBody)
			return;

		Vector3 rotationVelocity = mainRigidBody.transform.up * throwTurn * turnSpeed;
		mainRigidBody.AddTorque(rotationVelocity, ForceMode.VelocityChange);
	}

	private void DriveWheels(float throwAccel)
	{
		for(int i = 0; i < wheels.Count; i++)
		{
			wheels[i].Accelerate(throwAccel * accelerationTorque);
		}
	}

	/// <summary>
	/// Connect player input to the left weapon controls.
	/// </summary>
	/// <param name="function">Type of weapon action to perform.</param>
	public void FireLeftWeapon(MechaWeapon.WeaponFunction function)
	{
		if(!m_LeftWeapon)
			return;

		switch(function)
		{
			case MechaWeapon.WeaponFunction.Fire:
				m_LeftWeapon.OnFire();
				break;

			case MechaWeapon.WeaponFunction.Charge:
				m_LeftWeapon.OnCharge();
				break;

			case MechaWeapon.WeaponFunction.Release:
				m_LeftWeapon.OnRelease();
				break;
		}
	}

	/// <summary>
	/// Connect player input to the right weapon controls.
	/// </summary>
	/// <param name="function">Type of weapon action to perform.</param>
	public void FireRightWeapon(MechaWeapon.WeaponFunction function)
	{
		if(!m_RightWeapon)
			return;

		switch(function)
		{
			case MechaWeapon.WeaponFunction.Fire:
				m_RightWeapon.OnFire();
				break;

			case MechaWeapon.WeaponFunction.Charge:
				m_RightWeapon.OnCharge();
				break;

			case MechaWeapon.WeaponFunction.Release:
				m_RightWeapon.OnRelease();
				break;
		}
	}

	/// <summary>
	/// Pause game object activity.
	/// </summary>
	public override void OnPause()
	{
		if(mainRigidBody)
		{
			savedAngularVelocity = mainRigidBody.angularVelocity;
			savedVelocity = mainRigidBody.velocity;
			mainRigidBody.isKinematic = true;
		}
	}

	/// <summary>
	/// Un-pause game object activity.
	/// </summary>
	public override void OnResume()
	{
		if(mainRigidBody)
		{
			mainRigidBody.isKinematic = false;
			mainRigidBody.velocity = savedVelocity;
			mainRigidBody.angularVelocity = savedAngularVelocity;
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
