using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
	// TODO: Idea I have. I could create a little class called SceneTreeNode, 
	//       this class would have previousScene and nextScene as data. 
	//       This way I could link all scenes together no matter the build settings.

	IEnumerator WaitAndLoadScene(bool useLoadScreen, int index, float delayInSeconds)
	{
		yield return new WaitForSecondsRealtime(delayInSeconds);
		if(useLoadScreen && CanvasLoadingScreen.instance != null)
			CanvasLoadingScreen.instance.Show(SceneManager.LoadSceneAsync(index));
		else
			SceneManager.LoadScene(index);
	}

	public void LoadScene(int index, bool useLoadScreen)
	{
		if(useLoadScreen && CanvasLoadingScreen.instance != null)
			CanvasLoadingScreen.instance.Show(SceneManager.LoadSceneAsync(index));
		else
			SceneManager.LoadScene(index);
	}

	public void LoadScene(string sceneName, bool useLoadScreen)
	{
		if(useLoadScreen && CanvasLoadingScreen.instance != null)
			CanvasLoadingScreen.instance.Show(SceneManager.LoadSceneAsync(sceneName));
		else
			SceneManager.LoadScene(sceneName);
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
		LoadScene(index, false);
	}

	public void LoadMainSettings()
	{
		int index = 1; // TODO: Replace by soft-coded index
		LoadScene(index, false);
	}

	public void LoadGameMap()
	{
		int index = 2; // TODO: Replace by soft-coded index
		LoadScene(index, true);
	}
}
