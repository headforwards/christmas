﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventNames  {
		public const string UpdateScore = "UpdateScore";
		public const string PlayerFound = "PlayerFound";
		public const string PlayerLost = "PlayerLost";
		public const string PlayerWaving = "PlayerWaving";
		public const string PlayerReady = "PlayerReady";

		public const string DebugMessage = "DebugMessage";

		public const string GameStateChanged = "GameStateChanged";

		public const string StartPresents = "StartPresents";
		public const string StopPresents = "StopPresents";
}

public class GameStates {
	public const string WaitingForPlayers = "WaitingForPlayers";
	public const string PlayerJoined = "PlayerJoined";

	public const string InProgress = "InProgress";

	public const string GameFinished = "GameFinished";
}
