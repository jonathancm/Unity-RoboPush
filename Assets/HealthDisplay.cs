using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthDisplay : MonoBehaviour
{
	// Configurable Parameters
	[SerializeField] Health playerHealth = null;

	// Cached References
	RectTransform imageTransform = null;
	
	// State variables
	Vector2 initialImageSize;

	private void Awake()
	{
		imageTransform = GetComponent<RectTransform>();
		initialImageSize = imageTransform.rect.size;
	}

	void Update()
    {
		if(!playerHealth)
			return;

		UpdateHealthBarLength();
		UpdateHealthBarColor();
    }

	void UpdateHealthBarLength()
	{
		float healthWidth = playerHealth.GetCurrentHealth() / playerHealth.GetCurrentMaxHealth() * initialImageSize.x;
		imageTransform.sizeDelta = new Vector2(healthWidth, initialImageSize.y);
	}

	void UpdateHealthBarColor()
	{

	}
}
