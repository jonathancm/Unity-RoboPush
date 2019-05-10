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

	/// <summary>
	/// Load scene by build index value
	/// </summary>
	/// <param name="index">Index value of scene to load. See Project Build Settings.</param>
	/// <param name="useLoadScreen">Set to true to use a loading screen during scene transition.</param>
	public void LoadScene(int index, bool useLoadScreen)
	{
		if(useLoadScreen && CanvasLoadingScreen.instance != null)
			CanvasLoadingScreen.instance.Show(SceneManager.LoadSceneAsync(index));
		else
			SceneManager.LoadScene(index);
	}

	/// <summary>
	/// Load scene by build index value
	/// </summary>
	/// <param name="index">Index value of scene to load. See Project Build Settings.</param>
	/// <param name="useLoadScreen">Set to true to use a loading screen during scene transition.</param>
	public void LoadScene(string sceneName, bool useLoadScreen)
	{
		if(useLoadScreen && CanvasLoadingScreen.instance != null)
			CanvasLoadingScreen.instance.Show(SceneManager.LoadSceneAsync(sceneName));
		else
			SceneManager.LoadScene(sceneName);
	}

	/// <summary>
	/// Load next scene in the scene list
	/// </summary>
	/// <param name="delayInSeconds">Time to wait before loading next scene (seconds).</param>
	public void LoadNextScene(float delayInSeconds)
	{
		int currentScene = SceneManager.GetActiveScene().buildIndex;
		StartCoroutine(WaitAndLoadScene(true, currentScene + 1, delayInSeconds));
	}

	/// <summary>
	/// Reload current scene
	/// </summary>
	/// <param name="delayInSeconds">Time to wait before reloading scene (seconds).</param>
	public void ReloadScene(float delayInSeconds)
	{
		int currentScene = SceneManager.GetActiveScene().buildIndex;
		StartCoroutine(WaitAndLoadScene(true, currentScene, delayInSeconds));
	}

	/// <summary>
	/// Load the Main Menu Scene (Hardcoded)
	/// </summary>
	public void LoadMainMenu()
	{
		int index = 0; // TODO: Replace by soft-coded index
		LoadScene(index, false);
	}

	/// <summary>
	/// Load the Main Settings Scene (Hardcoded)
	/// </summary>
	public void LoadMainSettings()
	{
		int index = 1; // TODO: Replace by soft-coded index
		LoadScene(index, false);
	}

	/// <summary>
	/// Load the Game Map Scene (Harcoded)
	/// </summary>
	public void LoadGameMap()
	{
		int index = 2; // TODO: Replace by soft-coded index
		LoadScene(index, true);
	}
}
