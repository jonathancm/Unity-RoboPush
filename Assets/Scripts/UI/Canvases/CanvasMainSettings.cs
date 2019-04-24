using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasMainSettings : MonoBehaviour
{
	[SerializeField] Button defaultButton = null;

	private void Awake()
	{
		if(defaultButton)
			defaultButton.Select();
	}
}
