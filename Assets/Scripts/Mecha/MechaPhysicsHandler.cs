using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MechaPhysicsHandler : GameTimeObject
{
	// Configurable Parameters
	[Header("Setup")]
	[SerializeField] Rigidbody mainRigidBody = null;

	[Header("Physics")]
	[SerializeField] Transform centerOfMass = null;
	[SerializeField] float raycastLength = 0.05f;
	[SerializeField] float highDrag = 50f;
	[SerializeField] float lowDrag = 0.5f;

	// State variable
	bool isGamePaused = false;

	private void Awake()
	{
		if(mainRigidBody && centerOfMass)
			mainRigidBody.centerOfMass = centerOfMass.localPosition;
	}

	private bool IsGrounded()
	{
		int layerMask = 1<<LayerMask.NameToLayer("Floor");
		return Physics.Raycast(mainRigidBody.transform.position, -mainRigidBody.transform.up, raycastLength, layerMask);
	}

	private void Update()
	{
		if(isGamePaused)
			return;

		if(IsGrounded())
		{
			mainRigidBody.angularDrag = highDrag;
		}
		else
		{
			mainRigidBody.angularDrag = lowDrag;
		}
	}

	/// <summary>
	/// Pause game object activity.
	/// </summary>
	public override void OnPause()
	{
		isGamePaused = true;
	}

	/// <summary>
	/// Un-pause game object activity.
	/// </summary>
	public override void OnResume()
	{
		isGamePaused = false;
	}

	/// <summary>
	/// Prepare game object for game end.
	/// </summary>
	public override void OnGameOver()
	{
		// Nothing special
	}
}
