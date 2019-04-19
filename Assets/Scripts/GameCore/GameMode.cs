using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMode : MonoBehaviour
{
	// Configurable Parameters
	public bool isOnline = false;
	public bool canBePaused = false;
	public bool isTimed = false;
	public float timeLimitInSeconds = 0.0f;

	// Cached references
	GameAppManager gameAppManager = null;

	private void Awake()
	{
		gameAppManager = FindObjectOfType<GameAppManager>();
		gameAppManager.SetGameMode(this);
	}
}
