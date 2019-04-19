using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class MechaPlayerController : GameTimeObject
{
	public enum PlayerNumber
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
	bool isGamePaused = false;
	Player rewiredPlayer;
	Vector2 throwMovement;
	bool fire1;
	bool fire2;

	public PlayerNumber GetAssignedPlayerNumber() { return playerNumber; }

	private void Awake()
	{
		m_MechaController = GetComponent<MechaController>();
		rewiredPlayer = ReInput.players.GetPlayer((int)playerNumber);
	}

	private void FixedUpdate()
	{
		if(!m_MechaController || isGamePaused) { return; }
		if(!ReInput.isReady) { return; }

		GetPlayerInput();
		ProcessMechaInput();
	}

	private void GetPlayerInput()
	{
		throwMovement.x = rewiredPlayer.GetAxis("Turn");
		throwMovement.y = rewiredPlayer.GetAxis("Accelerate");

		fire1 = rewiredPlayer.GetButtonDown("Fire1");
		fire2 = rewiredPlayer.GetButtonDown("Fire2");
	}

	private void ProcessMechaInput()
	{
		if(throwMovement.magnitude > 0.0f)
			m_MechaController.Move(throwMovement.normalized.y, throwMovement.normalized.x);

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

	public override void OnPause()
	{
		isGamePaused = true;
	}

	public override void OnResume()
	{
		isGamePaused = false;
	}
}
