using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonExitGame : MonoBehaviour
{
	private void Awake()
	{
		if(Application.platform == RuntimePlatform.WebGLPlayer || Application.platform == RuntimePlatform.WindowsEditor)
		{
			gameObject.SetActive(false);
		}

		Button button = GetComponent<Button>();
		if(button)
			button.onClick.AddListener(OnClickAction);
	}

	public void OnClickAction()
	{
		GameAppManager gameAppManager = FindObjectOfType<GameAppManager>();
		if(gameAppManager)
			gameAppManager.ExitApplication();
	}
}
