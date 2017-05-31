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
	bool resetTurnText = true;

	void Start()
	{
		die = GameObject.Find("Die").GetComponent<Die>();
	}

	void Update()
	{
		if (IsMyTurn && die.CanRoll && (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space)))
			SendRoll();

		if (die.CanRoll && !resetTurnText)
		{
			if (Connection.instance.Sfs.MySelf.PlayerId == whoseTurn)
				turnText.text = "It's your turn!";
			else
				turnText.text = "Player " + whoseTurn + "'s turn";

			resetTurnText = true;
		}
	}

	public void SetStartingTurn(int playerId)
	{
		print("set starting turn player: " + playerId);
		print(playerId);

		if (playerId == 1)
		{
			whoseTurn = 1;
			turnText.text = "It's your turn!";
		}
		else
			turnText.text = "Player 1's turn";

		playerNumText.text = Connection.instance.Sfs.MySelf.Name + ": player " + playerId;
	}

	public void UpdateTurn(int newTurn)
	{
		whoseTurn = newTurn;
	}

	public void SendRoll()
	{
		SFSObject obj = new SFSObject();
		int randRoll = die.GetRandomRoll();
		randRoll = 3;	// DELET THIS
		//print(randRoll);
		obj.PutInt("roll", randRoll);

		SetChipStuff(randRoll);

		Connection.instance.Sfs.Send(new ExtensionRequest("sendRoll", obj, Connection.instance.Sfs.LastJoinedRoom));
	}

	public void GetRoll(int roll)
	{
		print("got roll: " + roll);
		turnText.text = "Rolling...";
		resetTurnText = false;
		die.RollTheDie(roll);
	}

	void SetChipStuff(int roll)
	{
		int playerId = Connection.instance.Sfs.MySelf.PlayerId - 1;
		int amountOfChips = chipDrawBoxes[playerId].GetAmountOfChips;

		if (amountOfChips <= 0) return;

		chipDrawBoxes[playerId].SetChipsToDraw(amountOfChips - roll);

		if (roll > amountOfChips)
			roll = amountOfChips;

		amountOfChips = chipDrawBoxes[playerId+1].GetAmountOfChips;
		chipDrawBoxes[playerId+1].SetChipsToDraw(amountOfChips + roll);
	}
}
