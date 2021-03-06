﻿using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using Sfs2X;
using Sfs2X.Logging;
using Sfs2X.Util;
using Sfs2X.Core;
using Sfs2X.Entities;
using Sfs2X.Requests;
using Sfs2X.Entities.Data;

public class Connection : MonoBehaviour
{
	string defaultHost = "127.0.0.1";   // Default host

	#if !UNITY_WEBGL
	int defaultTcpPort = 9933;          // Default TCP port
	#else
	int defaultWsPort = 8888;           // Default WebSocket port
	#endif 
	
	const string ZONE = "DiceExtension";
	const string EXTENSION_ID = "dice";
	const string EXTENSION_CLASS = "sfs2x.extensions.games.dice.DiceExtension";

	SmartFox sfs;
	public SmartFox Sfs { get { return sfs; } }
	public bool SfsReady { get { return sfs != null; } }
	ConnectUI connectUI;
	LobbyUI lobbyUI;
	GameLogic gameLogic;

	const int totalMaxPlayers = 5;		// keep hardcoded to the REAL max
	int? maxPlayers = null;
	public int MaxPlayers { get { return maxPlayers != null ? (int)maxPlayers : -1; } }

	bool gameStarted = false;
	public bool GameStarted { get { return gameStarted; } }

	public static Connection instance = null;

	void Awake()
	{
		if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy(gameObject);

		DontDestroyOnLoad(gameObject);
		Application.runInBackground = true;
		SceneManager.sceneLoaded += (scene, loadingMode) => { SceneLoaded(); };
	}

	void SceneLoaded()
	{
		int scene = SceneManager.GetActiveScene().buildIndex;

		if (!SfsReady && scene != 0) SceneManager.LoadScene(0);
		else
		{
			if (SfsReady)
				AddSfsListeners();

			FindRefs();

			print("loaded scene " + scene);
		}
	}

	void FindRefs()
	{
		if (connectUI == null)
		{
			GameObject con = GameObject.Find("ConnectUI");
			if (con != null)
			{
				connectUI = con.GetComponent<ConnectUI>();
				#if !UNITY_WEBGL
				connectUI.Init(defaultHost, defaultTcpPort);
				#else
				connectUI.Init(defaultHost, defaultWsPort);
				#endif
			}
		}

		if (gameLogic == null)
		{
			GameObject game = GameObject.Find("GameLogic");
			if (game != null)
				gameLogic = game.GetComponent<GameLogic>();
		}

		if (lobbyUI == null)
		{
			GameObject lobby = GameObject.Find("LobbyUI");
			if (lobby != null)
				lobbyUI = lobby.GetComponent<LobbyUI>();
		}
	}

	void Update()
	{
		// As Unity is not thread safe, we process the queued up callbacks on every frame
		if (SfsReady)
			sfs.ProcessEvents();
	}

	void OnApplicationQuit()
	{
		if (SfsReady)
		{
			sfs.Disconnect();
			print("Disconnected");
		}
	}

	#region Helper methods
	public bool Connect(string hostInput, string portInput, int maxPlayers)
	{
		if (!SfsReady || !sfs.IsConnected) // CONNECT
		{
			#if UNITY_WEBPLAYER
			// Socket policy prefetch can be done if the client-server communication is not encrypted only (read link provided in the note above)
			if (!Security.PrefetchSocketPolicy(hostInput.text, Convert.ToInt32(portInput.text), 500)) {
				Debug.LogError("Security Exception. Policy file loading failed!");
			}
			#endif

			print("Now connecting...");

			// Initialize SFS2X client and add listeners
			// WebGL build uses a different constructor
			#if !UNITY_WEBGL
			sfs = new SmartFox();
			#else
			sfs = new SmartFox(UseWebSocket.WS);
			#endif

			// Set ThreadSafeMode explicitly, or Windows Store builds will get a wrong default value (false)
			sfs.ThreadSafeMode = true;

			AddSfsListeners();

			// Set connection parameters
			ConfigData cfg = new ConfigData();
			cfg.Host = hostInput;
			cfg.Port = Convert.ToInt32(portInput);
			cfg.Zone = ZONE;

			if (this.maxPlayers == null)
				this.maxPlayers = maxPlayers;

			// Connect to SFS2X
			sfs.Connect(cfg);

			return true;
		}
		else // DISCONNECT
		{
			// Disconnect from SFS2X
			sfs.Disconnect();

			return false;
		}
	}

	void AddSfsListeners()
	{
		sfs.AddEventListener(SFSEvent.CONNECTION, OnConnection);
		sfs.AddEventListener(SFSEvent.CONNECTION_LOST, OnConnectionLost);
		sfs.AddEventListener(SFSEvent.LOGIN, OnLogin);
		sfs.AddEventListener(SFSEvent.LOGIN_ERROR, OnLoginError);
		sfs.AddEventListener(SFSEvent.ROOM_JOIN, OnRoomJoin);
		sfs.AddEventListener(SFSEvent.ROOM_JOIN_ERROR, OnRoomJoinError);
		sfs.AddEventListener(SFSEvent.USER_ENTER_ROOM, OnUserEnterRoom);
		sfs.AddEventListener(SFSEvent.USER_EXIT_ROOM, OnUserExitRoom);
		sfs.AddEventListener(SFSEvent.PUBLIC_MESSAGE, OnPublicMessage);
		sfs.AddEventListener(SFSEvent.EXTENSION_RESPONSE, OnExtensionResponse);

		sfs.AddLogListener(LogLevel.INFO, OnInfoMessage);
		sfs.AddLogListener(LogLevel.WARN, OnWarnMessage);
		sfs.AddLogListener(LogLevel.ERROR, OnErrorMessage);

		print("added event listeners");
	}

	public void Reset()
	{
		// Remove SFS2X listeners
		sfs.RemoveAllEventListeners();
	}

	public void HandleGameRoomJoin()
	{
		// create room, otherwise: join existing

		if (sfs.RoomList.Count <= 0) return;

		int roomId = -1;

		for (int i = 0; i < sfs.RoomList.Count; i++)
		{
			if (!sfs.RoomList[i].IsGame || sfs.RoomList[i].IsHidden || sfs.RoomList[i].IsPasswordProtected)
				continue;

			roomId = sfs.RoomList[i].Id;
			break;
		}

		sfs.Send(new PublicMessageRequest("playerReady"));
		if (roomId == -1) CreateGameRoom();
		else sfs.Send(new JoinRoomRequest(roomId));
	}

	void CreateGameRoom()
	{
		RoomSettings settings = new RoomSettings(sfs.MySelf.Name + "'s game");
		settings.GroupId = "games";
		settings.IsGame = true;
		settings.MaxUsers = totalMaxPlayers;
		settings.MaxSpectators = 0;
		settings.Extension = new RoomExtension(EXTENSION_ID, EXTENSION_CLASS);

		// Request Game Room creation to server
		sfs.Send(new CreateRoomRequest(settings, true, sfs.LastJoinedRoom));
	}

	public void StartupGame()
	{
		gameLogic.SetStartingTurn();

		SFSObject numOfPlayers = new SFSObject();
		numOfPlayers.PutInt("num", MaxPlayers);

		// Tell extension that this client is ready to play
		sfs.Send(new ExtensionRequest("ready", numOfPlayers, sfs.LastJoinedRoom));
	}
	#endregion

	#region SmartFoxServer successful event listeners
	void OnConnection(BaseEvent evt)
	{
		if ((bool)evt.Params["success"])
		{
			print("Connection established successfully");
			print("SFS2X API version: " + sfs.Version);
			print("Connection mode is: " + sfs.ConnectionMode);

			connectUI.EnableLogin();
		}
		else
		{
			print("Connection failed; is the server running at all?");

			// Remove SFS2X listeners and re-enable interface
			Reset();
		}
	}

	public void Login(string username)
	{
		sfs.Send(new LoginRequest(username));   // we need to be a user in order to talk to the server...
		connectUI.SetLoginButtonInteractable(false);
	}

	void OnLogin(BaseEvent evt)
	{
		User user = (User)evt.Params["user"];

		// Show system message
		print("Logged in as: '" + user.Name + "'");

		// Join first Room in Zone (The Lobby)
		if (sfs.RoomList.Count > 0)
			sfs.Send(new JoinRoomRequest(sfs.RoomList[0].Name));
	}

	void OnRoomJoin(BaseEvent evt)
	{
		Room room = (Room)evt.Params["room"];

		// Show system message
		print("You joined room '" + room.Name + "'");

		Reset();
		SceneManager.LoadScene(room.IsGame ? 2 : 1);
	}

	void OnUserEnterRoom(BaseEvent evt)
	{
		User user = (User)evt.Params["user"];

		// Show system message
		print("User " + user.Name + " entered the room");

		if (lobbyUI) lobbyUI.PopulateUserList(sfs.LastJoinedRoom.UserList);
	}

	void OnUserExitRoom(BaseEvent evt)
	{
		User user = (User)evt.Params["user"];

		if (user != sfs.MySelf)
		{
			// Show system message
			print("User " + user.Name + " left the room");
		}

		if (lobbyUI) lobbyUI.PopulateUserList(sfs.LastJoinedRoom.UserList);
	}

	private void OnPublicMessage(BaseEvent evt)
	{
		User sender = (User)evt.Params["sender"];
		string message = (string)evt.Params["message"];

		if (message == "playerReady" && !sender.IsItMe)
		{
			if (lobbyUI) lobbyUI.AddToReadyList(sender.Name);
		}
	}

	// Handle responses from server side Extension.
	public void OnExtensionResponse(BaseEvent evt)
	{
		string cmd = (string)evt.Params["cmd"];
		SFSObject dataObject = (SFSObject)evt.Params["params"];

		if (cmd == "recieveRoll")
		{
			print("recieved roll!");
			gameLogic.RecieveRoll(dataObject.GetInt("roll"), dataObject.GetInt("turn"));
		}

		if (cmd == "recieveChipList")
		{
			print("recieved chip list");
			gameLogic.SetAllBoxesBasedOnData(dataObject.GetIntArray("chipList"));
		}

		if (cmd == "start")
		{
			gameStarted = true;	// make sure other stuff can't happen before we start
		}
	}
	#endregion

	#region SmartFoxServer unsuccessful event listeners
	void OnConnectionLost(BaseEvent evt)
	{
		print("Connection was lost; reason is: " + (string)evt.Params["reason"]);

		// Remove SFS2X listeners and re-enable interface
		Reset();
	}

	void OnLoginError(BaseEvent evt)
	{
		// Disconnect
		sfs.Disconnect();

		// Remove SFS2X listeners and re-enable interface
		Reset();

		// Show error message
		print("Login failed: " + (string)evt.Params["errorMessage"]);
	}

	void OnRoomJoinError(BaseEvent evt)
	{
		// Show error message
		print("Room join failed: " + (string)evt.Params["errorMessage"]);
	}
	#endregion

	#region SmartFoxServer log event listeners
	public void OnInfoMessage(BaseEvent evt)
	{
		string message = (string)evt.Params["message"];
		ShowLogMessage("INFO", message);
	}

	public void OnWarnMessage(BaseEvent evt)
	{
		string message = (string)evt.Params["message"];
		ShowLogMessage("WARN", message);
	}

	public void OnErrorMessage(BaseEvent evt)
	{
		string message = (string)evt.Params["message"];
		ShowLogMessage("ERROR", message);
	}

	void ShowLogMessage(string level, string message)
	{
		message = "[SFS > " + level + "] " + message;
		print(message);
	}
	#endregion
}
