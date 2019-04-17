using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
	public enum HealthLevel
	{
		High = 0,
		Mid = 1,
		Low = 2
	};

	[Header("Health Stats")]
	[Min(1.0f)] [SerializeField] float baseMaxHealth = 20.0f;
	[SerializeField] float healthRegenPerSecond = 1.0f;
	[Range(1.0f, 10.0f)] [SerializeField] float healthThresholdMid = 14.0f;
	[Range(11.0f, 19.0f)] [SerializeField] float healthThresholdLow = 7.0f;

	[Header("Damage visuals")]
	[SerializeField] GameObject particlesThresholdMid = null;
	[SerializeField] GameObject particlesThresholdLow = null;

	// Cached References
	MovingCenterMass movingCenterMass = null;

	// State Variables
	float currentMaxHealth = 1.0f;
	float currentHealth = 1.0f;
	HealthLevel healthLevel = HealthLevel.High;

	// Delegates & Events
	public delegate void OnHealthThresholdAction();
	public event OnHealthThresholdAction onHealthThreshold;

	private void Awake()
	{
		healthLevel = HealthLevel.High;
		currentMaxHealth = baseMaxHealth;
		currentHealth = currentMaxHealth;

		movingCenterMass = GetComponent<MovingCenterMass>();
	}

	private void Update()
	{
		HealthRegen();
	}

	private void HealthRegen()
	{
		if(currentHealth > currentMaxHealth)
			currentHealth = currentMaxHealth;

		if(healthRegenPerSecond > 0.0f && currentHealth < currentMaxHealth)
			currentHealth += (healthRegenPerSecond * Time.deltaTime);
	}

	public void TakeDamage(float amount)
	{
		currentHealth -= amount;
		if(currentHealth <= 0.0f)
		{
			KillCharacter();
		}
		else if(currentHealth < healthThresholdLow)
		{
			healthLevel = HealthLevel.Low;
			currentMaxHealth = healthThresholdLow;

			if(particlesThresholdMid)
				particlesThresholdLow.SetActive(true);

			if(onHealthThreshold != null)
				onHealthThreshold();
		}
		else if(currentHealth < healthThresholdMid)
		{
			healthLevel = HealthLevel.Mid;
			currentMaxHealth = healthThresholdMid;

			if(particlesThresholdMid)
				particlesThresholdMid.SetActive(true);

			if(onHealthThreshold != null)
				onHealthThreshold();
		}

		if(movingCenterMass)
			movingCenterMass.KickBackCenterOfMass();
	}

	void KillCharacter()
	{
		currentHealth = 0.0f;
		healthRegenPerSecond = 0.0f;
	}

	public float GetBaseMaxHealth() { return baseMaxHealth; }
	public float GetHealthThresholdMid() { return healthThresholdMid; }
	public float GetHealthThresholdLow() { return healthThresholdLow; }
	public float GetCurrentMaxHealth() { return currentMaxHealth; }
	public float GetCurrentHealth() { return currentHealth; }
	public HealthLevel GetHealthLevel() { return healthLevel; }
}
