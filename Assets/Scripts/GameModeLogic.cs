using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class GameModeLogic : MonoBehaviour
{
	public enum GameState
	{
		Playing,
		Paused,
		Over
	};

	// State Variables
	GameState gameState;
	MechaController[] players = null;
	Player player1Input = null;
	Player player2Input = null;
	bool startButton = false;

	// Delegates and events
	public delegate void OnPauseAction();
	public event OnPauseAction onPause;
	public delegate void OnResumeAction();
	public event OnResumeAction onResume;

	private void Awake()
	{
		players = FindObjectsOfType<MechaController>();
	}

	void Start()
    {
		player1Input = ReInput.players.GetPlayer(0);
		player2Input = ReInput.players.GetPlayer(1);
	}

	private void Update()
	{
		GetPlayerInput();
		ProcessPlayerInput();
	}

	private void ProcessPlayerInput()
	{
		if(startButton)
		{
			switch(gameState)
			{
				case GameState.Playing:
					// TODO: Un-comment
					gameState = GameState.Paused;
					Debug.Log("Game is paused: " + gameState.ToString());
					if(onPause != null)
						onPause();
					break;

				case GameState.Paused:
					// TODO: un-comment
					gameState = GameState.Playing;
					Debug.Log("Game is paused: " + gameState.ToString());
					if(onResume != null)
						onResume();
					break;

				default:
					break;
			}
			startButton = false;
		}
	}

	private void GetPlayerInput()
	{
		startButton = player1Input.GetButtonDown("Start") || player2Input.GetButtonDown("Start");
	}
}
