using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasPauseGame : GameTimeObject
{
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

	private void Show()
	{
		canvas.enabled = true;
	}

	private void Hide()
	{
		canvas.enabled = false;
	}
}
