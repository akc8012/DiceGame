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
	public Text turnText;
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
				turnText.text = "Other user's turn";

			resetTurnText = true;
		}
	}

	public void SetStartingTurn(int playerId)
	{
		if (playerId == 1)
		{
			whoseTurn = 1;
			turnText.text = "It's your turn!";
		}
		else
			turnText.text = "Other user's turn";
	}

	public void UpdateTurn(bool toggleTurn, int lastTurn = -1)
	{
		if (toggleTurn && lastTurn != -1)
			whoseTurn = (lastTurn == 1) ? 2 : 1;
	}

	public void SendRoll()
	{
		SFSObject obj = new SFSObject();
		int randRoll = UnityEngine.Random.Range(0, 6);
		obj.PutInt("roll", randRoll);

		Connection.instance.Sfs.Send(new ExtensionRequest("sendRoll", obj, Connection.instance.Sfs.LastJoinedRoom));
	}

	public void GetRoll(int roll)
	{
		print("got roll: " + roll);
		turnText.text = "Rolling...";
		resetTurnText = false;
		die.RollTheDie(roll);
	}
}
