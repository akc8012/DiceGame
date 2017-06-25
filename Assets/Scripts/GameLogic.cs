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

	int whoseTurn = -1;
	public bool IsMyTurn { get { return Connection.instance.Sfs.MySelf.PlayerId == whoseTurn; } }
	bool sendChipMove = false;

	void Start()
	{
		die = GameObject.Find("Die").GetComponent<Die>();
		die.DoneRoll += FinishRoll;
	}

	void Update()
	{
		if (!Connection.instance.GameStarted)
			return;

		if (IsMyTurn && die.CanRoll && (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space)))
			SendRoll();
	}

	public void SetStartingTurn(int playerId)
	{
		print("set starting turn player: " + playerId);
		print(playerId);

		if (playerId == Connection.instance.MaxPlayers)
		{
			whoseTurn = Connection.instance.MaxPlayers;
			turnText.text = "It's your turn!";
		}
		else
			turnText.text = "Player " + Connection.instance.MaxPlayers + "'s turn";

		playerNumText.text = Connection.instance.Sfs.MySelf.Name + ": player " + playerId;
	}

	public void UpdateTurn(int newTurn)
	{
		whoseTurn = newTurn;
	}

	void SendRoll()
	{
		SFSObject rollObj = new SFSObject();
		int randRoll = die.GetRandomRoll();
		rollObj.PutInt("roll", randRoll);

		Connection.instance.Sfs.Send(new ExtensionRequest("sendRoll", rollObj, Connection.instance.Sfs.LastJoinedRoom));
		sendChipMove = true;
	}

	public void RecieveRoll(int roll)
	{
		print("recieved roll: " + roll);
		turnText.text = "Rolling...";
		die.RollTheDie(roll);
	}

	void FinishRoll(int roll)
	{
		if (Connection.instance.Sfs.MySelf.PlayerId == whoseTurn)
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

		int self = Connection.instance.Sfs.MySelf.PlayerId - 1;
		int next = self + 1;

		//if (next > Connection.instance.MaxPlayers)
		//	next = 0;

		chipObj.PutInt("from", self);
		chipObj.PutInt("to", next);

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
