using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
	IEnumerator WaitAndLoadScene(bool useLoadScreen, int index, float delayInSeconds)
	{
		yield return new WaitForSecondsRealtime(delayInSeconds);
		if(useLoadScreen && CanvasLoadingScreen.instance != null)
			CanvasLoadingScreen.instance.Show(SceneManager.LoadSceneAsync(index));
		else
			SceneManager.LoadScene(index);
	}

	void LoadScene(bool useLoadScreen, int index)
	{
		if(useLoadScreen && CanvasLoadingScreen.instance != null)
			CanvasLoadingScreen.instance.Show(SceneManager.LoadSceneAsync(index));
		else
			SceneManager.LoadScene(index);
	}

	public void LoadNextScene(float delayInSeconds)
	{
		int currentScene = SceneManager.GetActiveScene().buildIndex;
		StartCoroutine(WaitAndLoadScene(true, currentScene + 1, delayInSeconds));
	}

	public void ReloadScene(float delayInSeconds)
	{
		int currentScene = SceneManager.GetActiveScene().buildIndex;
		StartCoroutine(WaitAndLoadScene(true, currentScene, delayInSeconds));
	}

	public void LoadMainMenu()
	{
		int index = 0; // TODO: Replace by soft-coded index
		LoadScene(false, index);
	}

	public void LoadMainSettings()
	{
		int index = 1; // TODO: Replace by soft-coded index
		LoadScene(false, index);
	}

	public void LoadGameMap()
	{
		int index = 2; // TODO: Replace by soft-coded index
		LoadScene(true, index);
	}
}
