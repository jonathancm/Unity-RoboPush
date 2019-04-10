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

	// State Variable
	bool isPlayer1 = true;

	void Awake()
	{
		m_MechaController = GetComponent<MechaController>();
		if(gameObject.tag == "Player2")
			isPlayer1 = false;
	}

	void Update()
	{
		if(!m_MechaController)
			return;

		if(isPlayer1)
			GetInputForPlayer1();
		else
			GetInputForPlayer2();
	}

	private void GetInputForPlayer1()
	{
		throwAccel = Input.GetAxis("IntendAccelerate");
		throwTurn = Input.GetAxis("IntendTurn");

		if(Input.GetButtonDown("P1-Fire1"))
			m_MechaController.FirePrimaryWeapon();
		if(Input.GetButtonDown("P1-Fire2"))
			m_MechaController.FireSecondaryWeapon();
	}

	private void GetInputForPlayer2()
	{
		throwLV = Input.GetAxis("LeftVertical");
		throwRV = Input.GetAxis("RightVertical");

		if(Input.GetButtonDown("P2-Fire1"))
			m_MechaController.FirePrimaryWeapon();
		if(Input.GetButtonDown("P2-Fire2"))
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
