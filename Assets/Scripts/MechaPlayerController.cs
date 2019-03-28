using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

[RequireComponent(typeof(MechaController))]
public class MechaPlayerController : MonoBehaviour
{
	// Cached references
	MechaController m_MechaController; // the car controller we want to use

	// User Input
	float throwAccel = 0;
	float throwTurn = 0;
	float throwLV = 0;
	// float throwLH = 0; // Horizontal axis not used
	float throwRV = 0;
	// float throwRH = 0; // Horizontal axis not used
	bool isJumpRequested = false;

	void Awake()
	{
		m_MechaController = GetComponent<MechaController>();
	}

	void Update()
	{
		throwAccel = Input.GetAxis("IntendAccelerate");
		throwTurn = Input.GetAxis("IntendTurn");
		throwLV = Input.GetAxis("LeftVertical");
		throwRV = Input.GetAxis("RightVertical");
		isJumpRequested = isJumpRequested || Input.GetButtonDown("Jump");

		if(Input.GetButtonDown("Fire1"))
			m_MechaController.FirePrimaryWeapon();
		if(Input.GetButtonDown("Fire2"))
			m_MechaController.FireSecondaryWeapon();
	}


	void FixedUpdate()
	{
		if(Mathf.Abs(throwAccel) > 0 || Mathf.Abs(throwTurn) > 0)
			m_MechaController.MoveFlyByWire(throwAccel, throwTurn, throwAccel);
		else
			m_MechaController.MoveManually(throwLV, throwRV);
	}
}
