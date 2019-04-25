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

	public override void OnPause()
	{
		Show();
	}

	public override void OnResume()
	{
		Hide();
	}

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
