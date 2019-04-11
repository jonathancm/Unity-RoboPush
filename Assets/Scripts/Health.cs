using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
	[SerializeField] float baseMaxHealth = 20.0f;
	[SerializeField] float healthRegenPerSecond = 1.0f;

	// State Variables
	float currentMaxHealth = 1.0f;
	float currentHealth = 1.0f;

	private void Awake()
	{
		currentMaxHealth = baseMaxHealth;
		currentHealth = currentMaxHealth;
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
	}

	void KillCharacter()
	{
		currentHealth = 0.0f;
		healthRegenPerSecond = 0.0f;
	}

	public float GetBaseMaxHealth() { return baseMaxHealth; }
	public float GetCurrentMaxHealth() { return currentMaxHealth; }
	public float GetCurrentHealth() { return currentHealth; }
}
