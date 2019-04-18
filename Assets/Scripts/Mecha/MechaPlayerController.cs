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
	MechaController m_MechaController = null;

	// State variables
	Vector2 throwMovement;
	bool gameIsPaused = false;

	// Public Methods
	GameController GetAssignedGameController() { return gameController; }
	PlayerNumber GetAssignedPlayerNumber() { return playerNumber; }

	// Private Methods
	void Awake()
	{
		m_MechaController = GetComponent<MechaController>();
	}

	private void Start()
	{
		// AssignGameModeDelegates();
	}

	void Update()
	{
		if(!m_MechaController || gameIsPaused)
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

	void GetInputKeyboard()
	{
		throwMovement.x = Input.GetAxis("KB-Turn");
		throwMovement.y = Input.GetAxis("KB-Accelerate");

		if(Input.GetButtonDown("KB-Fire1"))
			m_MechaController.FirePrimaryWeapon();
		if(Input.GetButtonDown("KB-Fire2"))
			m_MechaController.FireSecondaryWeapon();
	}

	void GetInputGamepad1()
	{
		throwMovement.x = Input.GetAxis("GP1-RightStickX");
		throwMovement.y = Input.GetAxis("GP1-LeftStickY");

		if(Input.GetButtonDown("GP1-Fire1"))
			m_MechaController.FirePrimaryWeapon();
		if(Input.GetButtonDown("GP1-Fire2"))
			m_MechaController.FireSecondaryWeapon();
	}

	private void GetInputGamepad2()
	{
		throwMovement.x = Input.GetAxis("GP2-RightStickX");
		throwMovement.y = Input.GetAxis("GP2-LeftStickY");

		if(Input.GetButtonDown("GP2-Fire1"))
			m_MechaController.FirePrimaryWeapon();
		if(Input.GetButtonDown("GP2-Fire2"))
			m_MechaController.FireSecondaryWeapon();
	}

	void FixedUpdate()
	{
		if(!m_MechaController || gameIsPaused)
			return;

		if(throwMovement.magnitude > 0.0f)
			m_MechaController.MoveFlyByWire(throwMovement.normalized.y, throwMovement.normalized.x);
	}

	private void AssignGameModeDelegates()
	{
		GameModeLogic gameModeLogic = FindObjectOfType<GameModeLogic>();
		if(gameModeLogic)
		{
			gameModeLogic.onPause += OnPause;
			gameModeLogic.onResume += OnResume;
		}
	}

	void OnPause()
	{
		gameIsPaused = true;
		Debug.Log("Game is paused: " + gameIsPaused);
	}

	void OnResume()
	{
		gameIsPaused = false;
		Debug.Log("Game is paused: " + gameIsPaused);
	}
}
