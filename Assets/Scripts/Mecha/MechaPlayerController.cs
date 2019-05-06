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
	MechaWeapon.WeaponFunction fireLeft;
	MechaWeapon.WeaponFunction fireRight;

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

		if(rewiredPlayer.GetButtonLongPressUp("Fire1"))
		{
			fireLeft = MechaWeapon.WeaponFunction.Release;
		}
		else if(rewiredPlayer.GetButtonLongPressDown("Fire1"))
		{
			fireLeft = MechaWeapon.WeaponFunction.Charge;
		}
		else if(rewiredPlayer.GetButtonShortPressDown("Fire1"))
		{
			fireLeft = MechaWeapon.WeaponFunction.Fire;
		}
		else
		{
			fireLeft = MechaWeapon.WeaponFunction.None;
		}

		if(rewiredPlayer.GetButtonLongPressUp("Fire2"))
		{
			fireRight = MechaWeapon.WeaponFunction.Release;
		}
		else if(rewiredPlayer.GetButtonLongPressDown("Fire2"))
		{
			fireRight = MechaWeapon.WeaponFunction.Charge;
		}
		else if(rewiredPlayer.GetButtonShortPressDown("Fire2"))
		{
			fireRight = MechaWeapon.WeaponFunction.Fire;
		}
		else
		{
			fireRight = MechaWeapon.WeaponFunction.None;
		}
	}

	private void ProcessMechaInput()
	{
		if(throwMovement.magnitude > 0.0f)
			m_MechaController.Move(throwMovement.y, throwMovement.x);

		if(fireLeft != MechaWeapon.WeaponFunction.None)
		{
			m_MechaController.FireLeftWeapon(fireLeft);
			fireLeft = MechaWeapon.WeaponFunction.None;
		}

		if(fireRight != MechaWeapon.WeaponFunction.None)
		{
			m_MechaController.FireRightWeapon(fireRight);
			fireRight = MechaWeapon.WeaponFunction.None;
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

	public override void OnGameOver()
	{
		// Stop getting player input
		isGamePaused = true;
	}
}
