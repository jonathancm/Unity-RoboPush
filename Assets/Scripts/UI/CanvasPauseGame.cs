using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasPauseGame : MonoBehaviour
{
	// State variables
	bool isPaused = false;

	private void Awake()
	{

	}

	private void Start()
	{
		AssignGameModeDelegates();
		Hide();
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

	private void OnPause()
	{
		isPaused = true;
		Show();
	}

	private void OnResume()
	{
		isPaused = false;
		Hide();
	}

	private void Show()
	{
		gameObject.SetActive(true);
	}

	private void Hide()
	{
		gameObject.SetActive(false);
	}
}
