using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class HealthDisplay : MonoBehaviour
{
	// Configurable Parameters
	[Header("Setup")]
	[SerializeField] Health playerHealth = null;
	[SerializeField] GameObject healthBarLevel = null;
	[SerializeField] TextMeshProUGUI healthText = null;

	[Header("Layout")]
	[SerializeField] float framePadding = 10.0f;

	[Header("Colors")]
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

	private void Start()
	{
		SetHealthBarLevelColor();
		SetHealthBarFrameWidth();

		// Setup event delegates
		if(playerHealth)
		{
			playerHealth.onHealthThreshold += SetHealthBarLevelColor;
			playerHealth.onHealthThreshold += SetHealthBarFrameWidth;
		}
	}

	void Update()
    {
		SetHealthBarLevelWidth();
		SetHealthTextValue();
	}

	void SetHealthBarLevelWidth()
	{
		if(!playerHealth)
			return;

		float healthPercentage = playerHealth.GetCurrentHealth() / playerHealth.GetBaseMaxHealth();
		float healthWidth = healthPercentage * healthBarStartSize.x;
		healthBarLevelTransform.sizeDelta = new Vector2(healthWidth, healthBarStartSize.y);
	}

	void SetHealthTextValue()
	{
		if(!playerHealth || !healthText)
			return;

		healthText.text = (int)playerHealth.GetCurrentHealth() + "/" + (int)playerHealth.GetCurrentMaxHealth();
	}

	void SetHealthBarFrameWidth()
	{
		if(!playerHealth)
			return;

		float maxHealthPercentage = playerHealth.GetCurrentMaxHealth() / playerHealth.GetBaseMaxHealth();
		float frameWidth = (maxHealthPercentage * healthBarStartSize.x) + 2.0f * framePadding;
		healthBarFrameTransform.sizeDelta = new Vector2(frameWidth, healthBarFrameStartSize.y);
	}

	void SetHealthBarLevelColor()
	{
		if(!playerHealth)
			return;

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
