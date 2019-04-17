using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MechaPlayerController : MonoBehaviour
{
	enum GameController
	{
		None,
		Keyboard,
		Gamepad1,
		Gamepad2,
		Gamepad3,
		Gamepad4
	};

	enum PlayerNumber
	{
		Player1 = 1,
		Player2 = 2,
		Player3 = 3,
		Player4 = 4
	};

	// Configurable Parameter
	[SerializeField] GameController gameController = GameController.None;
	[SerializeField] PlayerNumber playerNumber = PlayerNumber.Player1;

	// Cached references
	MechaController m_MechaController;

	// User Input
	Vector2 throwMovement;

	void Awake()
	{
		m_MechaController = GetComponent<MechaController>();
	}

	void Update()
	{
		if(!m_MechaController)
			return;

		switch(gameController)
		{
			case GameController.Keyboard:
				GetInputKeyboard();
				break;

			case GameController.Gamepad1:
				GetInputGamepad1();
				break;

			case GameController.Gamepad2:
				GetInputGamepad2();
				break;

			case GameController.Gamepad3:
				// Not supported
				break;

			case GameController.Gamepad4:
				// Not supported
				break;

			default:
				break;
		}
	}

	private void GetInputKeyboard()
	{
		throwMovement.x = Input.GetAxis("KB-Turn");
		throwMovement.y = Input.GetAxis("KB-Accelerate");

		if(Input.GetButtonDown("KB-Fire1"))
			m_MechaController.FirePrimaryWeapon();
		if(Input.GetButtonDown("KB-Fire2"))
			m_MechaController.FireSecondaryWeapon();
	}

	private void GetInputGamepad1()
	{
		//float leftStickY = Input.GetAxis("GP1-LeftStickY");
		//float rightStickY = Input.GetAxis("GP1-RightStickY");

		//// Remap gamepad joysticks to user intention
		//throwMovement.x = (leftStickY - rightStickY);
		//throwMovement.y = (leftStickY + rightStickY);

		float leftStickY = Input.GetAxis("GP1-LeftStickY");
		float rightStickX = Input.GetAxis("GP1-RightStickX");

		// Remap gamepad joysticks to user intention
		throwMovement.x = rightStickX;
		throwMovement.y = leftStickY;

		if(Input.GetButtonDown("GP1-Fire1"))
			m_MechaController.FirePrimaryWeapon();
		if(Input.GetButtonDown("GP1-Fire2"))
			m_MechaController.FireSecondaryWeapon();
	}

	private void GetInputGamepad2()
	{
		float leftStickY = Input.GetAxis("GP2-LeftStickY");
		float rightStickY = Input.GetAxis("GP2-RightStickY");

		// Remap gamepad joysticks to user intention
		throwMovement.x = (leftStickY - rightStickY);
		throwMovement.y = (leftStickY + rightStickY);

		if(Input.GetButtonDown("GP2-Fire1"))
			m_MechaController.FirePrimaryWeapon();
		if(Input.GetButtonDown("GP2-Fire2"))
			m_MechaController.FireSecondaryWeapon();
	}

	void FixedUpdate()
	{
		if(!m_MechaController)
			return;

		if(throwMovement.magnitude > 0.0f)
			m_MechaController.MoveFlyByWire(throwMovement.normalized.y, throwMovement.normalized.x);
	}

	GameController GetAssignedGameController() { return gameController; }
	PlayerNumber GetAssignedPlayerNumber() { return playerNumber; }
}
