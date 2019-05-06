using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PunchingGloveEmission : MonoBehaviour
{
	// Configurable Parameters
	[SerializeField] [ColorUsage(false,true)]
	Color emissionColor = Color.white;

	// Cached References
	Renderer _renderer;

	// State variables
	MaterialPropertyBlock _propBlock;

	void Awake()
	{
		_propBlock = new MaterialPropertyBlock();
		_renderer = GetComponent<Renderer>();
		StopColorEmission();
	}

	public void StartColorEmission()
	{
		if(!_renderer)
			return;

		_renderer.GetPropertyBlock(_propBlock);
		_propBlock.SetColor("_Emission", emissionColor);
		_renderer.SetPropertyBlock(_propBlock);
	}

	public void StopColorEmission()
	{
		if(!_renderer)
			return;

		_renderer.GetPropertyBlock(_propBlock);
		_propBlock.SetColor("_Emission", Color.black);
		_renderer.SetPropertyBlock(_propBlock);
	}
}
