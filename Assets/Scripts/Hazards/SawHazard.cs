﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class SawHazard : GameTimeObject
{
	// Configurable Parameters
	[Header("Setup")]
	[SerializeField] GameObject sawMesh = null;
	[SerializeField] BoxCollider hazardTrigger = null;

	[Header("Damage")]
	[SerializeField] float damageAmount = 0.7f;
	[SerializeField] float cuttingForce = 30.0f;

	[Header("Animation")]
	[SerializeField] float cosmeticTurnsPerSecond = 1.0f;

	[Header("Particle Effects")]
	[SerializeField] ParticleSystem sparks = null;

	[Header("Sound Effects")]
	[SerializeField] float sawHitVolume = 0.5f;

	// Cached References
	AudioSource audioSource = null;
	DamageDealer damageDealer = null;


	private void Awake()
	{
		audioSource = GetComponent<AudioSource>();
		damageDealer = GetComponent<DamageDealer>();
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
			damageDealer.DealDamage(other.gameObject, damageAmount, false);
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

	/// <summary>
	/// Pause game object activity.
	/// </summary>
	public override void OnPause()
	{
		// Disable Update() and FixedUpdate()
		this.enabled = false;

		// Disable trigger volume
		if(hazardTrigger)
			hazardTrigger.enabled = false;

		// Pause Particles
		if(sparks && sparks.isPlaying)
			sparks.Pause();

		// Pause Audio
		if(audioSource && audioSource.isPlaying)
			audioSource.Pause();
	}

	/// <summary>
	/// Un-pause game object activity.
	/// </summary>
	public override void OnResume()
	{
		// Enable Update()
		this.enabled = true;

		// Enable trigger volume
		if(hazardTrigger)
			hazardTrigger.enabled = true;

		// Resume Particles
		if(sparks && sparks.isPaused)
			sparks.Play();

		// Resume Audio
		if(audioSource)
			audioSource.UnPause();
	}

	/// <summary>
	/// Prepare game object for game end.
	/// </summary>
	public override void OnGameOver()
	{
		// Disable Update() and FixedUpdate()
		this.enabled = false;

		// Disable trigger volume
		if(hazardTrigger)
			hazardTrigger.enabled = false;
	}
}
