using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


[RequireComponent(typeof(Selectable))]
public class SelectableHighlightFix : MonoBehaviour, IPointerEnterHandler, IDeselectHandler
{
	/// <summary>
	/// [Delegate] Set current object to selected.
	/// </summary>
	public void OnPointerEnter(PointerEventData eventData)
	{
		if(!EventSystem.current.alreadySelecting)
			EventSystem.current.SetSelectedGameObject(this.gameObject);
	}

	/// <summary>
	/// [Delegate] Set current object to unselected.
	/// </summary>
	public void OnDeselect(BaseEventData eventData)
	{
		this.GetComponent<Selectable>().OnPointerExit(null);
	}
}
