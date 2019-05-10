using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class VideoSettingsController : MonoBehaviour
{
	Vector2Int[] supportedResolutions =
	{
		new Vector2Int(640, 480), // 4:3
		new Vector2Int(800, 600), // 4:3
		new Vector2Int(1024, 768), // 4:3
		new Vector2Int(1280, 720), // 16:9
		new Vector2Int(1920, 1080), // 16:9
		new Vector2Int(2560, 1440) // 16:9
	};

	// Configurable Parameters
	[Header("PlayerPrefs")]
	[SerializeField] string screenResolutionName = "screenResolution";
	[SerializeField] string fullscreenModeName = "fullscreenMode";
	[SerializeField] string graphicsQualityName = "graphicsQuality";

	[Header("Settings Labels")]
	[SerializeField] TextMeshProUGUI resolutionLabel = null;
	[SerializeField] TextMeshProUGUI fullscreenLabel = null;
	[SerializeField] TextMeshProUGUI graphicsQualityLabel = null;

	// State Variable
	int qualityLevel = 0;
	int resolutionIndex = 0;
	FullScreenMode fullscreenMode = FullScreenMode.Windowed;

	/// <summary>
	/// [Delegate] Increase resolution level (from list of supported resolutions) and save to persistent memory.
	/// </summary>
	public void IncreaseResolution()
	{
		resolutionIndex = SetResolution(resolutionIndex + 1);
	}

	/// <summary>
	/// [Delegate] Decrease resolution level (from list of supported resolutions) and save to persistent memory.
	/// </summary>
	public void DecreaseResolution()
	{
		resolutionIndex = SetResolution(resolutionIndex - 1);
	}

	private int SetResolution(int index)
	{
		index = Mathf.Clamp(index, 0, supportedResolutions.Length - 1);
		resolutionLabel.text = supportedResolutions[index].x.ToString() + "x" + supportedResolutions[index].y.ToString();
		Screen.SetResolution(supportedResolutions[index].x, supportedResolutions[index].y, fullscreenMode);

		PlayerPrefs.SetInt(screenResolutionName, index);
		PlayerPrefs.Save();

		return index;
	}

	/// <summary>
	/// [Delegate] Increase fullscreen mode type (from list of supported modes) and save to persistent memory.
	/// </summary>
	public void IncreaseFullscreenMode()
	{
		switch(fullscreenMode)
		{
			case FullScreenMode.ExclusiveFullScreen:
			case FullScreenMode.FullScreenWindow:
			case FullScreenMode.MaximizedWindow:
				fullscreenMode = SetFullScreenMode(fullscreenMode + 1);
				break;

			case FullScreenMode.Windowed:
				// Do nothing
				break;
		}
	}

	/// <summary>
	/// [Delegate] Decrease fullscreen mode type (from list of supported modes) and save to persistent memory.
	/// </summary>
	public void DecreaseFullscreenMode()
	{
		switch(fullscreenMode)
		{
			case FullScreenMode.ExclusiveFullScreen:
				// Do nothing
				break;

			case FullScreenMode.FullScreenWindow:
			case FullScreenMode.MaximizedWindow:
			case FullScreenMode.Windowed:
				fullscreenMode = SetFullScreenMode(fullscreenMode - 1);
				break;
		}
	}

	private FullScreenMode SetFullScreenMode(FullScreenMode mode)
	{
		mode = (FullScreenMode) Mathf.Clamp((int) mode, 0, System.Enum.GetValues(typeof(FullScreenMode)).Length);
		fullscreenLabel.text = mode.ToString();
		Screen.fullScreenMode = mode;

		PlayerPrefs.SetInt(fullscreenModeName, (int)mode);
		PlayerPrefs.Save();

		return mode;
	}

	/// <summary>
	/// [Delegate] Increase graphics quality level (from list of supported quality levels) and save to persistent memory.
	/// </summary>
	public void IncreaseGraphicsQuality()
	{
		qualityLevel = SetGraphicsQualityLevel(qualityLevel + 1);
	}

	/// <summary>
	/// [Delegate] Decrease graphics quality level (from list of supported quality levels) and save to persistent memory.
	/// </summary>
	public void DecreaseGraphicsQuality()
	{
		qualityLevel = SetGraphicsQualityLevel(qualityLevel - 1);
	}

	private int SetGraphicsQualityLevel(int index)
	{
		index = Mathf.Clamp(index, 0, QualitySettings.names.Length - 1);
		graphicsQualityLabel.text = QualitySettings.names[index];
		QualitySettings.SetQualityLevel(index, true);

		PlayerPrefs.SetInt(graphicsQualityName, index);
		PlayerPrefs.Save();

		return index;
	}

	/// <summary>
	/// Initialize UI to stored video settings.
	/// </summary>
	public void InitVideoSettings()
	{
		// Create PlayerPrefs if they don't exist
		if(!PlayerPrefs.HasKey(graphicsQualityName))
		{
			PlayerPrefs.SetInt(graphicsQualityName, 4);
			PlayerPrefs.SetInt(fullscreenModeName, (int)FullScreenMode.Windowed);
			PlayerPrefs.SetInt(screenResolutionName, 3);
			PlayerPrefs.Save();
		}

		// Init graphics quality
		qualityLevel = PlayerPrefs.GetInt(graphicsQualityName, 0);
		qualityLevel = Mathf.Clamp(qualityLevel, 0, QualitySettings.names.Length - 1);
		graphicsQualityLabel.text = QualitySettings.names[qualityLevel];
		QualitySettings.SetQualityLevel(qualityLevel, true);

		// Init resolution and fullscreen modes
		fullscreenMode = (FullScreenMode) PlayerPrefs.GetInt(fullscreenModeName, (int)FullScreenMode.Windowed);
		resolutionIndex = PlayerPrefs.GetInt(screenResolutionName, 3);
		resolutionIndex = Mathf.Clamp(resolutionIndex, 0, supportedResolutions.Length - 1);

		resolutionLabel.text = supportedResolutions[resolutionIndex].x.ToString() + "x" + supportedResolutions[resolutionIndex].y.ToString();
		fullscreenLabel.text = fullscreenMode.ToString();
		Screen.SetResolution(supportedResolutions[resolutionIndex].x, supportedResolutions[resolutionIndex].y, fullscreenMode);
	}

	/// <summary>
	/// Reset video settings to defaults.
	/// </summary>
	public void ResetSettingsToDefaults()
	{
		qualityLevel = SetGraphicsQualityLevel(4); // Very High
		fullscreenMode = SetFullScreenMode(FullScreenMode.Windowed);
		resolutionIndex = SetResolution(3); // 720p
	}

	private void Start()
	{
		InitVideoSettings();
	}
}
