using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CanvasGameOver : GameTimeObject
{
	// Configurable parameters
	[SerializeField] Selectable firstSelected = null;
	[SerializeField] TextMeshProUGUI winText = null;

	// Cached References
	Canvas canvas = null;

	private void Awake()
	{
		canvas = GetComponent<Canvas>();
		Hide();
	}

	/// <summary>
	/// Pause game object activity.
	/// </summary>
	public override void OnPause()
	{
		// Do nothing
	}

	/// <summary>
	/// Un-pause game object activity.
	/// </summary>
	public override void OnResume()
	{
		// Do nothing
	}

	/// <summary>
	/// Prepares game object for game end.
	/// </summary>
	public override void OnGameOver()
	{
		DetermineWinner();
		Show();
	}

	private void Show()
	{
		canvas.enabled = true;
		if(firstSelected)
			firstSelected.Select();
	}

	private void Hide()
	{
		canvas.enabled = false;
	}

	private void DetermineWinner()
	{
		List<Damageable> remainingPlayers = new List<Damageable>();

		Damageable[] players = FindObjectsOfType<Damageable>();
		foreach(var player in players)
		{
			if(player.GetCurrentHealth() > 0)
				remainingPlayers.Add(player);
		}

		if(remainingPlayers.Count == 0 || remainingPlayers.Count > 1)
		{
			if(winText)
				winText.text = "It's a draw!";
		}
		else
		{
			if(winText)
				winText.text = remainingPlayers[0].name + " wins!";
		}
	}
}
