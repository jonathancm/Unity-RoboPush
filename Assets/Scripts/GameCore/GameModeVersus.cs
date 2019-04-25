using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameModeVersus : GameMode
{
    void Start()
    {
		Health[] players = FindObjectsOfType<Health>();
		if(players == null)
			return;

		foreach(var player in players)
		{
			player.onDeath += TriggerVictoryCondition;
		}
	}

	void TriggerVictoryCondition()
	{
		GameAppManager gameAppManager = FindObjectOfType<GameAppManager>();
		if(!gameAppManager)
			return;

		gameAppManager.EndGame();
	}
}
