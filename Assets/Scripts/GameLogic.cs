using UnityEngine;
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

public class GameLogic : MonoBehaviour
{
	[SerializeField] Text turnText;
	[SerializeField] Text playerNumText;
	[SerializeField] ChipDrawBox[] chipDrawBoxes;

	Die die;

	int playerId = -1;
	int? secondPlayerId;
	int whoseTurn = -1;

	public bool IsMyTurn
	{
		get
		{
			if (secondPlayerId != null)
				return playerId == whoseTurn || secondPlayerId == whoseTurn;

			return playerId == whoseTurn;
		}
	}


	bool sendChipMove = false;

	void Start()
	{
		die = GameObject.Find("Die").GetComponent<Die>();
		die.DoneRoll += FinishRoll;
		playerId = Connection.instance.Sfs.MySelf.PlayerId;

		if (Application.isEditor)
		{
			secondPlayerId = playerId + 1;
			print("SET SECONDARY ID: " + secondPlayerId);
		}

		Connection.instance.StartupGame();
	}

	void Update()
	{
		if (!Connection.instance.GameStarted)
			return;

		if (IsMyTurn && die.CanRoll && (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space)))
			SendRoll();
	}

	public void SetStartingTurn()
	{
		print("set starting turn player: " + playerId);
		whoseTurn = Connection.instance.MaxPlayers;

		if (IsMyTurn)
			turnText.text = "It's your turn!";
		else
			turnText.text = "Player " + whoseTurn + "'s turn";

		playerNumText.text = Connection.instance.Sfs.MySelf.Name + ": player " + playerId;
	}

	public void RecieveRoll(int roll, int newTurn)
	{
		print("recieved roll: " + roll);
		turnText.text = "Rolling...";
		die.RollTheDie(roll);

		whoseTurn = newTurn;
	}

	void SendRoll()
	{
		SFSObject rollObj = new SFSObject();
		int randRoll = die.GetRandomRoll();
		rollObj.PutInt("roll", randRoll);
		rollObj.PutInt("secondPlayerId", (secondPlayerId != null) ? (int)secondPlayerId : -1);

		Connection.instance.Sfs.Send(new ExtensionRequest("sendRoll", rollObj, Connection.instance.Sfs.LastJoinedRoom));
		sendChipMove = true;
	}

	void FinishRoll(int roll)
	{
		if (IsMyTurn)
			turnText.text = "It's your turn!";
		else
			turnText.text = "Player " + whoseTurn + "'s turn";

		if (sendChipMove)
			SendChipMove(roll);	
	}

	void SendChipMove(int roll)
	{
		SFSObject chipObj = new SFSObject();
		chipObj.PutInt("amount", roll);

		int last = playerId - 1;
		int self = last + 1;

		chipObj.PutInt("from", last);
		chipObj.PutInt("to", self);

		Connection.instance.Sfs.Send(new ExtensionRequest("sendChipMove", chipObj, Connection.instance.Sfs.LastJoinedRoom));
		sendChipMove = false;
	}

	public void SetAllBoxesBasedOnData(int[] chipData)
	{
		for (int i = 0; i < chipData.Length; i++)
		{
			chipDrawBoxes[i].SetChipsToDraw(chipData[i]);
		}
	}
}
