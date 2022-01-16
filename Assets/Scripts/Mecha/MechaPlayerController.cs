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
		if(!m_MechaController) { return; }
		if(!ReInput.isReady) { return; }

		if(!isGamePaused)
		{
			GetPlayerInput();
			ProcessMechaInput();
		}
	}

	private void GetPlayerInput()
	{
        // Movement input
        throwMovement.x = rewiredPlayer.GetAxis("Turn");
        throwMovement.y = rewiredPlayer.GetAxis("Accelerate");

        // Left weapon input
        if (rewiredPlayer.GetButtonLongPressUp("Fire1"))
        {
            fireLeft = MechaWeapon.WeaponFunction.Release;
        }
        else if (rewiredPlayer.GetButtonLongPressDown("Fire1"))
        {
            fireLeft = MechaWeapon.WeaponFunction.Charge;
        }
        else if (rewiredPlayer.GetButtonShortPressDown("Fire1"))
        {
            fireLeft = MechaWeapon.WeaponFunction.Fire;
        }
        else
        {
            fireLeft = MechaWeapon.WeaponFunction.None;
        }

        // Right weapon input
        if (rewiredPlayer.GetButtonLongPressUp("Fire2"))
        {
            fireRight = MechaWeapon.WeaponFunction.Release;
        }
        else if (rewiredPlayer.GetButtonLongPressDown("Fire2"))
        {
            fireRight = MechaWeapon.WeaponFunction.Charge;
        }
        else if (rewiredPlayer.GetButtonShortPressDown("Fire2"))
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
		// Movement
		if(throwMovement.magnitude > 0.0f)
			m_MechaController.Move(throwMovement.y, throwMovement.x);

		// Left weapon
		if(fireLeft != MechaWeapon.WeaponFunction.None)
		{
			m_MechaController.FireLeftWeapon(fireLeft);
			fireLeft = MechaWeapon.WeaponFunction.None;
		}

		// Right weapon
		if(fireRight != MechaWeapon.WeaponFunction.None)
		{
			m_MechaController.FireRightWeapon(fireRight);
			fireRight = MechaWeapon.WeaponFunction.None;
		}
	}

	/// <summary>
	/// Pause game object activity.
	/// </summary>
	public override void OnPause()
	{
		isGamePaused = true;
	}

	/// <summary>
	/// Un-pause game object activity.
	/// </summary>
	public override void OnResume()
	{
		isGamePaused = false;
	}

	/// <summary>
	/// Prepare game object for game end.
	/// </summary>
	public override void OnGameOver()
	{
		isGamePaused = true;
	}
}
