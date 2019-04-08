using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class WeaponPunchingGlove : MonoBehaviour
{
	// Configurable Parameters
	[SerializeField] float punchingArmLength = 0.85f;
	[Range(0.001f, 1.0f)] [SerializeField] float punchExtensionSteps = 0.1f;

	[Header("Physics")]
	[SerializeField] float knockbackStrength = 100.0f;
	[SerializeField] float recoilStrength = 50.0f;
	[SerializeField] Rigidbody mainRigidBody = null;

	enum WeaponState
	{
		Ready,
		Firing,
		CoolingDown
	};

	// State variables
	WeaponState weaponState = WeaponState.Ready;
	Vector3 originalPosition;

	private void Update()
	{
		originalPosition = transform.parent.position;
		switch(weaponState)
		{
			case WeaponState.Firing:
				ExtendArm();
				break;

			case WeaponState.CoolingDown:
				RetractArm();
				break;

			default:
				break;
		}
	}

	public void Fire()
	{
		if(weaponState == WeaponState.Ready)
			weaponState = WeaponState.Firing;
	}

	private void ExtendArm()
	{
		Vector3 targetPosition = originalPosition;

		if(transform.position == targetPosition)
		{
			weaponState = WeaponState.CoolingDown;
		}
		else
		{
			float maxDistanceStep = punchingArmLength / punchExtensionSteps;
			transform.position = Vector3.MoveTowards(transform.position, targetPosition, maxDistanceStep * Time.deltaTime);
		}
	}

	private void RetractArm()
	{
		if(transform.position == originalPosition)
		{
			weaponState = WeaponState.Ready;
		}
		else
		{
			float maxDistanceStep = punchingArmLength / punchExtensionSteps;
			transform.position = Vector3.MoveTowards(transform.position, originalPosition, maxDistanceStep * Time.deltaTime);
		}
	}

	private void OnCollisionEnter(Collision other)
	{
		if(weaponState == WeaponState.Firing)
		{
			Rigidbody otherBody = other.rigidbody;
			if(otherBody)
				otherBody.AddForceAtPosition(knockbackStrength * transform.forward, other.contacts[0].point, ForceMode.Impulse);

			if(mainRigidBody)
				mainRigidBody.AddForceAtPosition(recoilStrength * -transform.forward, other.contacts[0].point, ForceMode.Impulse);

			weaponState = WeaponState.CoolingDown;
		}
	}
}
