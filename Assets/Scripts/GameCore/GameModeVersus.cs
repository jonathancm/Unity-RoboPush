using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameModeVersus : GameMode
{
	private void Start()
    {
		Damageable[] players = FindObjectsOfType<Damageable>();
		if(players == null)
			return;

		foreach(var player in players)
		{
			player.onDeath += TriggerVictoryCondition;
		}
	}

	private void TriggerVictoryCondition()
	{
		GameAppManager gameAppManager = FindObjectOfType<GameAppManager>();
		if(!gameAppManager)
			return;

		gameAppManager.EndGame();
	}
}
