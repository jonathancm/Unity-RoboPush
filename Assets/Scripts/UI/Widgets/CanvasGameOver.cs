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

	public override void OnPause()
	{
		// Do nothing
	}

	public override void OnResume()
	{
		// Do nothing
	}

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
		List<Health> remainingPlayers = new List<Health>();

		Health[] players = FindObjectsOfType<Health>();
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
