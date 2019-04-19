using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class GameAppManager : MonoBehaviour
{
	public enum GameState
	{
		Playing,
		Paused,
		Over
	};

	public static GameAppManager instance = null;

	// Cached References
	SceneLoader sceneLoader = null;
	GameMode gameMode = null;

	// State Variables
	GameState gameState = GameState.Playing;
	MechaController[] players = null; // Detect player death and trigger victory condition
	Player player1Input = null;
	Player player2Input = null;
	bool startButton = false;

	public GameState GetGameState() { return gameState; }
	public bool IsGamePaused() { return (gameState == GameState.Paused); }

	public void SetGameMode(GameMode newGameMode)
	{
		gameMode = newGameMode;
	}

	private void Awake()
	{
		SetupSingleton();
		sceneLoader = FindObjectOfType<SceneLoader>();
	}

	private void SetupSingleton()
	{
		if(instance == null)
		{
			instance = this;
			DontDestroyOnLoad(gameObject);
		}
		else if(instance != this)
		{
			gameObject.SetActive(false);
			Destroy(gameObject);
		}
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

	private void GetPlayerInput()
	{
		startButton = player1Input.GetButtonDown("Start") || player2Input.GetButtonDown("Start");
	}

	private void ProcessPlayerInput()
	{
		if(!startButton)
			return;

		switch(gameState)
		{
			case GameState.Playing:
				gameState = GameState.Paused;
				PauseAllObjects();
				break;

			case GameState.Paused:
				gameState = GameState.Playing;
				UnPauseAllObjects();
				break;

			default:
				break;
		}
		startButton = false;
	}

	private void PauseAllObjects()
	{
		if(!gameMode || gameMode.canBePaused != true)
			return;

		GameTimeObject[] timedOjects = FindObjectsOfType<GameTimeObject>();
		foreach(var timedObject in timedOjects)
		{
			timedObject.OnPause();
		}
	}

	private void UnPauseAllObjects()
	{
		if(!gameMode || gameMode.canBePaused != true)
			return;

		GameTimeObject[] timedOjects = FindObjectsOfType<GameTimeObject>();
		foreach(var timedObject in timedOjects)
		{
			timedObject.OnResume();
		}
	}
}
