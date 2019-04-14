using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthDisplay : MonoBehaviour
{
	// Configurable Parameters
	[SerializeField] Health playerHealth = null;
	[SerializeField] GameObject healthBarLevel = null;
	[SerializeField] Color healthColorHigh = Color.green;
	[SerializeField] Color healthColorMid = Color.yellow;
	[SerializeField] Color healthColorLow = Color.red;

	// Cached References
	RectTransform healthBarFrameTransform = null;
	RectTransform healthBarLevelTransform = null;
	Image healthBarLevelImage = null;

	// State variables
	Vector2 healthBarFrameStartSize;
	Vector2 healthBarStartSize;

	private void Awake()
	{
		healthBarFrameTransform = GetComponent<RectTransform>();
		healthBarFrameStartSize = healthBarFrameTransform.rect.size;

		healthBarLevelTransform = healthBarLevel.GetComponent<RectTransform>();
		healthBarLevelImage = healthBarLevel.GetComponent<Image>();
		healthBarStartSize = healthBarLevelTransform.rect.size;
	}

	void Update()
    {
		if(!playerHealth)
			return;

		UpdateHealthBarLevelWidth();
		UpdateHealthBarFrameWidth();
		UpdateHealthBarLevelColor();

	}

	void UpdateHealthBarLevelWidth()
	{
		float healthWidth = playerHealth.GetCurrentHealth() / playerHealth.GetBaseMaxHealth() * healthBarStartSize.x;
		healthBarLevelTransform.sizeDelta = new Vector2(healthWidth, healthBarStartSize.y);
	}

	void UpdateHealthBarFrameWidth()
	{
		float frameWidth = playerHealth.GetCurrentMaxHealth() / playerHealth.GetBaseMaxHealth() * healthBarFrameStartSize.x;
		healthBarFrameTransform.sizeDelta = new Vector2(frameWidth, healthBarFrameStartSize.y);
	}

	void UpdateHealthBarLevelColor()
	{
		switch(playerHealth.GetHealthLevel())
		{
			case Health.HealthLevel.High:
				healthBarLevelImage.color = healthColorHigh;
				break;
			case Health.HealthLevel.Mid:
				healthBarLevelImage.color = healthColorMid;
				break;
			default:
				healthBarLevelImage.color = healthColorLow;
				break;
		}
	}
}
