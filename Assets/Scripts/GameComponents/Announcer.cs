using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Announcer : MonoBehaviour
{
	[Header("Sound Effects")]
	[SerializeField] AudioClip[] readyClips = null;
	[SerializeField] float startDelay = 1.0f;
	[Range(0.0f, 1.0f)] [SerializeField] float readyVolume = 1.0f;

	// Cached References
	AudioSource audioSource = null;

	private void Awake()
	{
		audioSource = GetComponent<AudioSource>();
	}

	private void Start()
	{
		StartCoroutine(AnnounceFightStart());	
	}

	IEnumerator AnnounceFightStart()
	{
		yield return new WaitForSeconds(startDelay);
		if(audioSource && readyClips.Length > 0)
		{
			int index = Random.Range(0, readyClips.Length);
			audioSource.volume = readyVolume;
			audioSource.clip = readyClips[index];
			audioSource.Play();
		}
	}
}
