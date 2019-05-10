using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResumeButton : MonoBehaviour
{
	private void Awake()
	{
		Button button = GetComponent<Button>();
		if(button)
			button.onClick.AddListener(OnClickAction);
	}

	/// <summary>
	/// [Delegate] Resume game from pause.
	/// </summary>
	public void OnClickAction()
	{
		GameAppManager gameAppManager = FindObjectOfType<GameAppManager>();
		if(gameAppManager)
			gameAppManager.UnPauseGame();
	}
}
