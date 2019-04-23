using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSettings : MonoBehaviour
{
	public enum GraphicsQuality
	{
		Lowest,
		Low,
		Medium,
		High,
		VeryHigh,
		Ultra
	};

	[Header("Video")]
	public Vector2Int resolution = new Vector2Int(1280, 720);
	public GraphicsQuality graphicsQuality = GraphicsQuality.VeryHigh;

	[Header("Audio")]
	[Range(0,1)] public float masterVolume = 1.0f;
	[Range(0,1)] public float musicVolume = 1.0f;
	[Range(0,1)] public float ambientVolume = 1.0f;
	[Range(0,1)] public float announcerVolume = 1.0f;
	[Range(0,1)] public float sfxVolume = 1.0f;
}
