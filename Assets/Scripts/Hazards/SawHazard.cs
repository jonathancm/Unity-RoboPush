﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class SawHazard : MonoBehaviour
{
	// Configurable Parameters
	[SerializeField] GameObject sawMesh = null;
	[SerializeField] float cosmeticTurnsPerSecond = 1.0f;
	[SerializeField] float cuttingForce = 30.0f;

	[Header("Particle Effects")]
	[SerializeField] ParticleSystem sparks = null;

	[Header("Sound Effects")]
	[SerializeField] float sawHitVolume = 0.5f;

	// Cached References
	AudioSource audioSource = null;
	DamageDealer damageDealer = null;

	// State variables
	bool isPaused = false;

	private void Awake()
	{
		audioSource = GetComponent<AudioSource>();
		damageDealer = GetComponent<DamageDealer>();
	}

	private void Start()
	{
		//AssignGameModeDelegates();
	}

	void Update()
	{
		if(sawMesh)
			sawMesh.transform.Rotate(cosmeticTurnsPerSecond * 360.0f * Time.deltaTime, 0.0f, 0.0f);
	}

	private void OnTriggerStay(Collider other)
	{
		Rigidbody otherBody = other.attachedRigidbody;
		if(!otherBody)
			return;

		otherBody.AddForceAtPosition(cuttingForce * transform.forward, transform.position, ForceMode.Impulse);
	}

	private void OnTriggerEnter(Collider other)
	{
		PlayParticleEffect();
		PlaySawHitSound();

		if(damageDealer)
			damageDealer.DealDamage(other.gameObject);
	}

	private void PlayParticleEffect()
	{
		if(sparks)
			sparks.Play();
	}

	private void PlaySawHitSound()
	{
		if(!audioSource)
			return;

		if(audioSource.isPlaying)
			return;

		audioSource.volume = sawHitVolume;
		audioSource.Play();
	}

	private void AssignGameModeDelegates()
	{
		GameModeLogic gameModeLogic = FindObjectOfType<GameModeLogic>();
		if(gameModeLogic)
		{
			gameModeLogic.onPause += OnPause;
			gameModeLogic.onResume += OnResume;
		}
	}

	private void OnPause()
	{
		isPaused = true;
	}

	private void OnResume()
	{
		isPaused = false;
	}
}
