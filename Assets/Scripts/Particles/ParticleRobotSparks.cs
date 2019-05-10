using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleRobotSparks : MonoBehaviour
{
	// Configurable Parameters
	[SerializeField] float period = 1.0f;

	// Cached references
	ParticleSystem robotSparks = null;

	// State Variable
	float nextActionTime = 0.0f;

	private void Awake()
	{
		robotSparks = GetComponent<ParticleSystem>();
	}

	private void Update()
	{
		if(Time.time > nextActionTime)
		{
			nextActionTime = Time.time + period;
			PlaySparks();
		}
	}

	private void PlaySparks()
	{
		if(!robotSparks)
			return;

		robotSparks.Play();
	}
}
