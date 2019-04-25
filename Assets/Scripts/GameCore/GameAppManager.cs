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

	public void EndGame()
	{
		List<Health> remainingPlayers = new List<Health>();

		gameState = GameState.Over;
		GameTimeObject[] timedOjects = FindObjectsOfType<GameTimeObject>();
		foreach(var timedObject in timedOjects)
		{
			timedObject.OnGameOver();
		}
	}

	public void OnActiveSceneChanged(Scene current, Scene next)
	{
		gameMode = null;
	}

	public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		gameMode = FindObjectOfType<GameMode>();
		gameState = GameState.Playing;
	}

	public void ExitApplication()
	{
		Application.Quit();
	}
}
