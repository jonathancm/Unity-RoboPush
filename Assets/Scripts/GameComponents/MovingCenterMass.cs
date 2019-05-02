using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingCenterMass : MonoBehaviour
{
	// Configurable Parameters
	[Header("Setup")]
	[SerializeField] Rigidbody mainRigidBody = null;
	[SerializeField] Transform centerOfMassHigh = null;
	[SerializeField] float kickBackSpeed = 0.5f;
	[SerializeField] float settleBackSpeed = 0.5f;

	// State variable
	Vector3 initialCenterOfMass;

	private void Start()
	{
		if(mainRigidBody)
			initialCenterOfMass = mainRigidBody.centerOfMass;
	}

    void Update()
    {
		SettleBackCenterOfMass();
    }

	void SettleBackCenterOfMass()
	{
		if(!mainRigidBody)
			return;

		if(mainRigidBody.centerOfMass == initialCenterOfMass)
			return;

		mainRigidBody.centerOfMass = Vector3.MoveTowards(mainRigidBody.centerOfMass, initialCenterOfMass, settleBackSpeed * Time.deltaTime);
	}

	public void KickBackCenterOfMass()
	{
		if(!mainRigidBody || !centerOfMassHigh)
			return;

		if(mainRigidBody.centerOfMass == centerOfMassHigh.localPosition)
			return;

		mainRigidBody.centerOfMass = Vector3.MoveTowards(mainRigidBody.centerOfMass, centerOfMassHigh.localPosition, kickBackSpeed);
	}
}
