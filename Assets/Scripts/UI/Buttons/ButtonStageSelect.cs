using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class ButtonStageSelect : MonoBehaviour, ISelectHandler, IDeselectHandler
{
	// Configurable Parameter
	[SerializeField] string newSceneName = "";
	[SerializeField] TextMeshProUGUI stageName = null;
	[SerializeField] Color normalTextColor = Color.white;
	[SerializeField] Color highlightedTextColor = Color.red;

	private void Awake()
	{
		if(stageName)
			stageName.color = normalTextColor;
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
			sceneLoader.LoadScene(newSceneName, true);
	}

	public void OnSelect(BaseEventData eventData)
	{
		if(stageName)
			stageName.color = highlightedTextColor;
	}

	public void OnDeselect(BaseEventData eventData)
	{
		if(stageName)
			stageName.color = normalTextColor;
	}
}
