using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonSceneChanger : MonoBehaviour
{
	// Configurable Parameter
	[SerializeField] string newSceneName = "";
	[SerializeField] bool useLoadingScreen = false;

	private void Awake()
	{
		Button button = GetComponent<Button>();
		if(button)
			button.onClick.AddListener(LoadNewScene);
	}

	/// <summary>
	/// [Delegate] Load specified scene.
	/// </summary>
	public void LoadNewScene()
	{
		SceneLoader sceneLoader = FindObjectOfType<SceneLoader>();
		if(sceneLoader)
			sceneLoader.LoadScene(newSceneName, useLoadingScreen);
	}
}
