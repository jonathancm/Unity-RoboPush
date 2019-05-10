using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Rewired;

public class GameAppManager : MonoBehaviour
{
	public enum GameState
	{
		Playing,
		Paused,
		Over
	};

	// Cached References
	SceneLoader sceneLoader = null;
	GameMode gameMode = null;

	// State Variables
	GameState gameState = GameState.Playing;
	Player player1Input = null;
	Player player2Input = null;
	bool startButton = false;

	public GameState GetGameState() { return gameState; }
	public bool IsGamePaused() { return (gameState == GameState.Paused); }

	private void Awake()
	{
		sceneLoader = FindObjectOfType<SceneLoader>();
		SceneManager.sceneLoaded += OnSceneLoaded;
		SceneManager.activeSceneChanged += OnActiveSceneChanged;
	}

	private void Start()
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
				PauseGame();
				break;

			case GameState.Paused:
				UnPauseGame();
				break;

			default:
				break;
		}
		startButton = false;
	}

	/// <summary>
	/// Find all GameTimeObject and call their Pause routine.
	/// </summary>
	public void PauseGame()
	{
		if(!gameMode || gameMode.canBePaused != true)
			return;

		gameState = GameState.Paused;

		GameTimeObject[] timedOjects = FindObjectsOfType<GameTimeObject>();
		foreach(var timedObject in timedOjects)
		{
			timedObject.OnPause();
		}
	}

	/// <summary>
	/// Find all GameTimeObject and call their UnPause routine.
	/// </summary>
	public void UnPauseGame()
	{
		if(!gameMode || gameMode.canBePaused != true)
			return;

		gameState = GameState.Playing;

		GameTimeObject[] timedOjects = FindObjectsOfType<GameTimeObject>();
		foreach(var timedObject in timedOjects)
		{
			timedObject.OnResume();
		}
	}

	/// <summary>
	/// Find all GameTimeObject and call their EndGame routine.
	/// </summary>
	public void EndGame()
	{
		List<Damageable> remainingPlayers = new List<Damageable>();

		gameState = GameState.Over;
		GameTimeObject[] timedOjects = FindObjectsOfType<GameTimeObject>();
		foreach(var timedObject in timedOjects)
		{
			timedObject.OnGameOver();
		}
	}

	/// <summary>
	/// Discard the current scene's game mode. Automatically called by the SceneManager before changing scene.
	/// </summary>
	public void OnActiveSceneChanged(Scene current, Scene next)
	{
		gameMode = null;
	}

	/// <summary>
	/// Get the current scene's game mode. Automatically called by the SceneManager after loading a new scene.
	/// </summary>
	public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		gameMode = FindObjectOfType<GameMode>();
		gameState = GameState.Playing;
	}

	/// <summary>
	/// Exit the software application. Only works for certain plaftorms.
	/// </summary>
	public void ExitApplication()
	{
		Application.Quit();
	}
}
