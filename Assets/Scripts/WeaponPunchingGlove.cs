using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class WeaponPunchingGlove : MonoBehaviour
{
	enum WeaponState
	{
		Ready,
		Firing,
		CoolingDown
	};

	// Cached references
	Rigidbody rigidBody;

	// State variables
	WeaponState weaponState = WeaponState.Ready;
	Vector3 originalPosition;
	float basePunchingArmOffset = 0.8f;
	float punchingArmLength = 0.85f;

	private void Awake()
	{
		rigidBody = GetComponent<Rigidbody>();
	}

	private void Update()
	{
		originalPosition = transform.parent.transform.position + basePunchingArmOffset * transform.forward;
	}

	public void Fire()
	{
		if(weaponState == WeaponState.Ready)
			weaponState = WeaponState.Firing;
	}

	private void FixedUpdate()
	{
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

	private void ExtendArm()
	{
		Vector3 targetPosition = originalPosition + (punchingArmLength * transform.forward.normalized);

		if(transform.position == targetPosition)
		{
			weaponState = WeaponState.CoolingDown;
		}
		else
		{
			float maxDistanceStep = punchingArmLength / 4;
			Vector3 nextIntermediatePosition = Vector3.MoveTowards(transform.position, targetPosition, maxDistanceStep);
			rigidBody.MovePosition(nextIntermediatePosition);
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
			float maxDistanceStep = punchingArmLength / 4;
			Vector3 nextIntermediatePosition = Vector3.MoveTowards(transform.position, originalPosition, maxDistanceStep);
			rigidBody.MovePosition(nextIntermediatePosition);
		}
	}
}
