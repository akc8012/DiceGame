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

	void SendRoll()
	{
		SFSObject rollObj = new SFSObject();
		int randRoll = die.GetRandomRoll();
		rollObj.PutInt("roll", randRoll);

		SetChipStuff(randRoll);
		int[] chipData = GetChipData();
		SFSObject chipObj = new SFSObject();
		chipObj.PutIntArray("data", chipData);

		Connection.instance.Sfs.Send(new ExtensionRequest("sendRoll", rollObj, Connection.instance.Sfs.LastJoinedRoom));
		Connection.instance.Sfs.Send(new ExtensionRequest("sendChipData", chipObj, Connection.instance.Sfs.LastJoinedRoom));
	}

	public void RecieveRoll(int roll)
	{
		print("recieved roll: " + roll);
		turnText.text = "Rolling...";
		die.RollTheDie(roll);
	}

	void FinishRoll()
	{
		if (Connection.instance.Sfs.MySelf.PlayerId == whoseTurn)
			turnText.text = "It's your turn!";
		else
			turnText.text = "Player " + whoseTurn + "'s turn";
	}

	void SetChipStuff(int roll)
	{
		int playerId = Connection.instance.Sfs.MySelf.PlayerId - 1;
		int amountOfChips = chipDrawBoxes[playerId].GetAmountOfChips;

		if (amountOfChips <= 0) return;

		chipDrawBoxes[playerId].SetChipsToDraw(amountOfChips-roll < 0 ? 0 : amountOfChips-roll);

		if (roll > amountOfChips)
			roll = amountOfChips;

		amountOfChips = chipDrawBoxes[playerId+1].GetAmountOfChips;
		chipDrawBoxes[playerId+1].SetChipsToDraw(amountOfChips + roll);
	}

	int[] GetChipData()
	{
		int[] chipData = new int[chipDrawBoxes.Length];

		for (int i = 0; i < chipDrawBoxes.Length; i++)
		{
			chipData[i] = chipDrawBoxes[i].GetAmountOfChips;
		}

		return chipData;
	}

	public void SetAllBoxesBasedOnData(int[] chipData)
	{
		for (int i = 0; i < chipData.Length; i++)
		{
			chipDrawBoxes[i].SetChipsToDraw(chipData[i]);
		}
	}
}
