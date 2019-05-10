using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphicsQuality : MonoBehaviour
{
	public string qualityString = ""; // example on how to get the quality setting as a string, to display to the user

	private void Start()
	{
		qualityString = QualityString;
	}

	private void Update()
	{
		if(Input.GetKeyDown(KeyCode.Equals)) // PLUS key
		{
			ClickQualityUp();
			qualityString = QualityString;
		}

		if(Input.GetKeyDown(KeyCode.Minus)) // MINUS key
		{
			ClickQualityDown();
			qualityString = QualityString;
		}
	}

	void ClickQualityUp() { QualitySettings.IncreaseLevel(); }

	void ClickQualityDown() { QualitySettings.DecreaseLevel(); }

	string QualityString
	{
		get
		{
			return QualitySettings.names[QualitySettings.GetQualityLevel()];
		}
	}
}
