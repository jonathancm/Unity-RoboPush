using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameTimeObject : MonoBehaviour
{
	public abstract void OnPause();
	public abstract void OnResume();
	public abstract void OnGameOver();
}
