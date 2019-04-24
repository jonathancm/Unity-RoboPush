using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingCenterMass : MonoBehaviour
{
	// Configurable Parameters
	[Header("Setup")]
	[SerializeField] Rigidbody mainRigidBody = null;
	[SerializeField] Transform centerOfMassLow = null;
	[SerializeField] Transform centerOfMassHigh = null;
	[SerializeField] float kickBackSpeed = 0.5f;
	[SerializeField] float settleBackSpeed = 0.5f;

	private void Awake()
	{
		if(mainRigidBody && centerOfMassLow)
			mainRigidBody.centerOfMass = centerOfMassLow.localPosition;
	}

    void Update()
    {
		SettleBackCenterOfMass();
    }

	void SettleBackCenterOfMass()
	{
		if(!mainRigidBody || !centerOfMassLow)
			return;

		if(mainRigidBody.centerOfMass == centerOfMassLow.localPosition)
			return;

		mainRigidBody.centerOfMass = Vector3.MoveTowards(mainRigidBody.centerOfMass, centerOfMassLow.localPosition, settleBackSpeed * Time.deltaTime);
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
