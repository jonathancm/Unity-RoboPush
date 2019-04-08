using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MechaPlayerController : MonoBehaviour
{
	// Cached references
	MechaController m_MechaController; // the controller we want to use

	// User Input
	float throwAccel = 0;
	float throwTurn = 0;
	float throwLV = 0;
	float throwRV = 0;
	bool brakeButton = false;

	void Awake()
	{
		m_MechaController = GetComponent<MechaController>();
	}

	void Update()
	{
		if(!m_MechaController)
			return;

		throwAccel = Input.GetAxis("IntendAccelerate");
		throwTurn = Input.GetAxis("IntendTurn");
		throwLV = Input.GetAxis("LeftVertical");
		throwRV = Input.GetAxis("RightVertical");
		brakeButton = Input.GetButton("Jump");

		if(Input.GetButtonDown("Fire1"))
			m_MechaController.FirePrimaryWeapon();
		if(Input.GetButtonDown("Fire2"))
			m_MechaController.FireSecondaryWeapon();
	}


	void FixedUpdate()
	{
		if(!m_MechaController)
			return;

		if(Mathf.Abs(throwAccel) > 0.0f || Mathf.Abs(throwTurn) > 0.0f)
			m_MechaController.MoveFlyByWire(throwAccel, throwTurn, brakeButton);
		else
			m_MechaController.MoveManually(throwLV, throwRV, brakeButton);
	}
}
