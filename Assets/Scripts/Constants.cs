using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventNames  {
		public static string UpdateScore = "UpdateScore";
		public static string PlayerFound = "PlayerFound";
		public static string PlayerLost = "PlayerLost";
		public static string PlayerWaving = "PlayerWaving";
		public static string PlayerRaisedRightHand = "PlayerRaisedRightHand";

		public static string DebugMessage = "DebugMessage";
}

public class GameStates {
	public static string WaitingForPlayers = "WaitingForPlayers";
	public static string PlayerDetected = "PlayerDetected";
	public static string PlayerJoined = "PlayerJoined";

	public static string InProgress = "InProgress";

	public static string GameFinished = "GameFinished";

	
}
