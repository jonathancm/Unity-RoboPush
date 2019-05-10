using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasPauseGame : GameTimeObject
{
	// Configurable parameters
	[SerializeField] Selectable firstSelected = null;

	// Cached References
	Canvas canvas = null;

	private void Awake()
	{
		canvas = GetComponent<Canvas>();
		Hide();
	}

	/// <summary>
	/// Enable canvas and display to player.
	/// </summary>
	public override void OnPause()
	{
		Show();
	}

	/// <summary>
	/// Disable canvas and hide from player.
	/// </summary>
	public override void OnResume()
	{
		Hide();
	}

	/// <summary>
	/// Prepare game object for end game.
	/// </summary>
	public override void OnGameOver()
	{
		// Nothing special
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
}
