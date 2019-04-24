using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class AudioMixerController : MonoBehaviour
{
	public class AudioVolumes
	{
		public float masterVolume = 0.0f;
		public float musicVolume = 0.0f;
		public float ambientVolume = 0.0f;
		public float announcerVolume = 0.0f;
		public float sfxVolume = 0.0f;
	}

	// Configurable Parameters
	[Header("Audio Mixer")]
	[SerializeField] AudioMixer audioMixer = null;
	[SerializeField] string masterVolumeName = "masterVolume";
	[SerializeField] string musicVolumeName = "musicVolume";
	[SerializeField] string ambientVolumeName = "ambientVolume";
	[SerializeField] string announcerVolumeName = "announcerVolume";
	[SerializeField] string sfxVolumeName = "sfxVolume";

	[Header("UI Sliders")]
	[SerializeField] Slider masterVolumeSlider = null;
	[SerializeField] Slider musicVolumeSlider = null;
	[SerializeField] Slider ambientVolumeSlider = null;
	[SerializeField] Slider announcerVolumeSlider = null;
	[SerializeField] Slider sfxVolumeSlider = null;

	// State Variables
	AudioVolumes audioSliderVolumes = new AudioVolumes();

	public void SetMasterVolume(float volume)
	{
		audioSliderVolumes.masterVolume = volume;
		SetAudioMixerFloat(masterVolumeName, volume);
		PlayerPrefs.SetFloat(masterVolumeName, audioSliderVolumes.masterVolume);
		PlayerPrefs.Save();
	}

	public void SetMusicVolume(float volume)
	{
		audioSliderVolumes.musicVolume = volume;
		SetAudioMixerFloat(musicVolumeName, volume);
		PlayerPrefs.SetFloat(musicVolumeName, audioSliderVolumes.musicVolume);
		PlayerPrefs.Save();
	}

	public void SetAmbientVolume(float volume)
	{
		audioSliderVolumes.ambientVolume = volume;
		SetAudioMixerFloat(ambientVolumeName, volume);
		PlayerPrefs.SetFloat(ambientVolumeName, audioSliderVolumes.ambientVolume);
		PlayerPrefs.Save();
	}

	public void SetAnnouncerVolume(float volume)
	{
		audioSliderVolumes.announcerVolume = volume;
		SetAudioMixerFloat(announcerVolumeName, volume);
		PlayerPrefs.SetFloat(announcerVolumeName, audioSliderVolumes.announcerVolume);
		PlayerPrefs.Save();
	}

	public void SetSFXVolume(float volume)
	{
		audioSliderVolumes.sfxVolume = volume;
		SetAudioMixerFloat(sfxVolumeName, volume);
		PlayerPrefs.SetFloat(sfxVolumeName, audioSliderVolumes.sfxVolume);
		PlayerPrefs.Save();
	}

	private void SetAudioMixerFloat(string name, float value)
	{
		if(audioMixer)
			audioMixer.SetFloat(name, value);
	}

	public void InitMixerSliders()
	{
		if(!PlayerPrefs.HasKey(masterVolumeName))
		{
			PlayerPrefs.SetFloat(masterVolumeName, 0.0f);
			PlayerPrefs.SetFloat(musicVolumeName, 0.0f);
			PlayerPrefs.SetFloat(ambientVolumeName, 0.0f);
			PlayerPrefs.SetFloat(announcerVolumeName, 0.0f);
			PlayerPrefs.SetFloat(sfxVolumeName, 0.0f);
			PlayerPrefs.Save();
		}

		masterVolumeSlider.value = PlayerPrefs.GetFloat(masterVolumeName, 0.0f);
		musicVolumeSlider.value = PlayerPrefs.GetFloat(musicVolumeName, 0.0f);
		ambientVolumeSlider.value = PlayerPrefs.GetFloat(ambientVolumeName, 0.0f);
		announcerVolumeSlider.value = PlayerPrefs.GetFloat(announcerVolumeName, 0.0f);
		sfxVolumeSlider.value = PlayerPrefs.GetFloat(sfxVolumeName, 0.0f);
	}

	public void ApplyDefaultMixerSettings()
	{
		masterVolumeSlider.value = 0.0f;
		musicVolumeSlider.value = 0.0f;
		ambientVolumeSlider.value = 0.0f;
		announcerVolumeSlider.value = 0.0f;
		sfxVolumeSlider.value = 0.0f;
	}

	private void Start()
	{
		InitMixerSliders();
	}

	// TODO: Move this to UI Volume display
	private float UISliderScaleToAudioMixerScale(float volume)
	{
		// Assuming slider range(0,1) and mixer range (-80,0)
		volume = (volume * 80.0f) - 80.0f;
		return volume;
	}

	private float AudioMixerScaleToUISliderScale(float volume)
	{
		// Assuming slider range(0,1) and mixer range(-80,0)
		volume = (volume + 80.0f) / 80.0f;
		return volume;
	}
}
