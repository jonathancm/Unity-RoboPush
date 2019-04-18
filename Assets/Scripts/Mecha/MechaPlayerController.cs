using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class MechaPlayerController : MonoBehaviour
{
	enum PlayerNumber
	{
		Player1 = 0,
		Player2 = 1,
		Player3 = 2,
		Player4 = 3
	};

	// Configurable Parameter
	[SerializeField] PlayerNumber playerNumber = PlayerNumber.Player1;

	// Cached references
	MechaController m_MechaController = null;

	// State variables
	Vector2 throwMovement;
	private bool fire1;
	private bool fire2;
	bool gameIsPaused = false;
	private Player rewiredPlayer;

	PlayerNumber GetAssignedPlayerNumber()
	{
		return playerNumber;
	}

	private void Awake()
	{
		m_MechaController = GetComponent<MechaController>();
	}

	private void Start()
	{
		InitializeRewired(); // Reinitialize after a recompile in the editor
		// AssignGameModeDelegates();
	}

	private void Update()
	{
		if(!m_MechaController || gameIsPaused) { return; }

		// Rewired
		if(!ReInput.isReady) { return; }
			

		GetPlayerInput();
	}

	private void InitializeRewired()
	{
		// Get the Rewired Player object for this player.
		rewiredPlayer = ReInput.players.GetPlayer((int)playerNumber);
	}

	private void GetPlayerInput()
	{
		throwMovement.x = rewiredPlayer.GetAxis("Turn");
		throwMovement.y = rewiredPlayer.GetAxis("Accelerate");

		fire1 = rewiredPlayer.GetButtonDown("Fire1");
		fire2 = rewiredPlayer.GetButtonDown("Fire2");
	}

	private void FixedUpdate()
	{
		if(!m_MechaController || gameIsPaused)
			return;

		ProcessMechaInput();
	}

	private void ProcessMechaInput()
	{
		if(throwMovement.magnitude > 0.0f)
			m_MechaController.MoveFlyByWire(throwMovement.normalized.y, throwMovement.normalized.x);

		if(fire1)
		{
			m_MechaController.FirePrimaryWeapon();
			fire1 = false;
		}

		if(fire2)
		{
			m_MechaController.FireSecondaryWeapon();
			fire2 = false;
		}
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
	}

	void OnResume()
	{
		gameIsPaused = false;
	}
}
