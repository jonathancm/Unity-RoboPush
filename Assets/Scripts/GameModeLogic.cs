using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameModeLogic : MonoBehaviour
{
	public enum GameState
	{
		Playing,
		Paused,
		Over
	};

	// Configurable Parameters
	[SerializeField] string startAxisName = "UI-Start";

	// State Variables
	GameState gameState;
	MechaController[] players = null;

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
		
    }

	private void Update()
	{
		if(Input.GetAxis(startAxisName) > 0.0f)
		{
			switch(gameState)
			{
				case GameState.Playing:
					// TODO: Un-comment
					//gameState = GameState.Paused;
					//if(onPause != null)
					//	onPause();
					break;

				case GameState.Paused:
					// TODO: un-comment
					//gameState = GameState.Playing;
					//if(onResume != null)
					//	onResume();
					break;

				default:
					break;
			}
		}
	}
}
