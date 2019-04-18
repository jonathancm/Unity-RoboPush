using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasPauseGame : MonoBehaviour
{
	// Cached References
	Canvas canvas = null;

	// State variables
	bool isPaused = false;

	private void Awake()
	{
		// Enable and Hide
		AssignGameModeDelegates();

		canvas = GetComponent<Canvas>();
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

	void OnPause()
	{
		isPaused = true;
		Show();
	}

	void OnResume()
	{
		isPaused = false;
		Hide();
	}

	private void Show()
	{
		gameObject.SetActive(true);
		canvas.enabled = true;
	}

	private void Hide()
	{
		canvas.enabled = false;
		gameObject.SetActive(false);
	}
}
