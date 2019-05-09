using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonRematch : MonoBehaviour
{
	private void Awake()
	{
		Button button = GetComponent<Button>();
		if(button)
			button.onClick.AddListener(LoadNewScene);
	}

	public void LoadNewScene()
	{
		SceneLoader sceneLoader = FindObjectOfType<SceneLoader>();
		if(sceneLoader)
			sceneLoader.ReloadScene(0);
	}
}
