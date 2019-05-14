using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingFloor : GameTimeObject
{
	// Configurable Parameters
	[Header("Animation")]
	[SerializeField] bool invertDirection = false;
	[SerializeField] float rotationSpeed = 30.0f;

	// Cached References
	Rigidbody rigidBody = null;

	// State variables
	bool isGamePaused = false;

	private void Awake()
	{
		rigidBody = GetComponent<Rigidbody>();
	}

	// Update is called once per frame
	void Update()
    {
		Vector3 rotation;

		if(isGamePaused)
			return;

		if(invertDirection)
			rotation = -transform.up * rotationSpeed * Time.deltaTime;
		else
			rotation = transform.up * rotationSpeed * Time.deltaTime;
		Quaternion baseRotation = rigidBody.transform.rotation;
		Quaternion deltaRotation = Quaternion.Euler(rotation);
		rigidBody.MoveRotation(deltaRotation * baseRotation);
	}

	/// <summary>
	/// Pause game object activity.
	/// </summary>
	public override void OnPause()
	{
		isGamePaused = true;
	}

	/// <summary>
	/// Un-pause game object activity.
	/// </summary>
	public override void OnResume()
	{
		isGamePaused = false;
	}

	/// <summary>
	/// Prepare game object for game end.
	/// </summary>
	public override void OnGameOver()
	{
		isGamePaused = true;
	}
}
