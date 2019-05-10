using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CanvasLoadingScreen : MonoBehaviour
{
	// Configurable Parameters
	[SerializeField] Image loadingBarForeground = null;
	[SerializeField] TextMeshProUGUI textLoadingPercent = null;

	// Singleton interface
	public static CanvasLoadingScreen instance = null;

	// Constants
	private const float MIN_TIME_TO_SHOW = 2.0f;

	// Cached references
	Animator animator = null;

	// State Variables
	AsyncOperation currentLoadingOperation;
	bool isLoading;
	float timeElapsed = 0.0f;
	bool didTriggerFadeOutAnimation;

	private void Awake()
	{
		SetupSingleton();

		animator = GetComponent<Animator>();

		// Enable and Hide
		GetComponent<Canvas>().enabled = true;
		Hide();
	}

	private void SetupSingleton()
	{
		if(instance == null)
		{
			instance = this;
			DontDestroyOnLoad(gameObject);
		}
		else if(instance != this)
		{
			gameObject.SetActive(false);
			Destroy(gameObject);
		}
	}

	private void Update()
	{
		if(!isLoading)
			return;
		
		// Get the progress and update the UI. Goes from 0 (start) to 1 (end)
		SetProgress(currentLoadingOperation.progress);
		if(currentLoadingOperation.isDone && !didTriggerFadeOutAnimation)
		{
			animator.SetTrigger("Hide");
			didTriggerFadeOutAnimation = true;
		}
		else
		{
			timeElapsed += Time.deltaTime;
			if(timeElapsed >= MIN_TIME_TO_SHOW)
				currentLoadingOperation.allowSceneActivation = true;
		}
	}

	private void SetProgress(float progress)
	{
		if(loadingBarForeground)
			loadingBarForeground.fillAmount = progress;

		if(textLoadingPercent)
			textLoadingPercent.text = Mathf.CeilToInt(progress * 100).ToString() + "%";
	}

	/// <summary>
	/// Enable and show canvas. Start monitoring ongoing loading operation.
	/// </summary>
	/// <param name="loadingOperation">Ongoing loading operation.</param>
	public void Show(AsyncOperation loadingOperation)
	{
		gameObject.SetActive(true);
		
		// Stop the loading operation from finishing, even if it technically did
		currentLoadingOperation = loadingOperation;
		currentLoadingOperation.allowSceneActivation = false;
		
		// Reset state
		SetProgress(0f);
		timeElapsed = 0f;

		// Start animation
		animator.SetTrigger("Show");
		didTriggerFadeOutAnimation = false;

		isLoading = true;
	}

	/// <summary>
	/// Disable and hide canvas.
	/// </summary>
	public void Hide()
	{
		gameObject.SetActive(false);
		currentLoadingOperation = null;
		isLoading = false;
	}
}
