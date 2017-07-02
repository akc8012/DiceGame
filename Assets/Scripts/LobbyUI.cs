using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sfs2X.Entities;	// User

public class LobbyUI : MonoBehaviour
{
	[SerializeField] Button readyButton;
	[SerializeField] Text userName;
	[SerializeField] Text userList;
	[SerializeField] Text waitingForText;
	
	List<string> readyList;

	void Awake()
	{
		readyButton.onClick.AddListener(ReadyButtonClicked);
		//readyButton.interactable = false;
		readyList = new List<string>();
	}

	void Start()
	{
		userName.text = Connection.instance.Sfs.MySelf.Name;
		PopulateUserList(Connection.instance.Sfs.LastJoinedRoom.UserList);
	}

	void ReadyButtonClicked()
	{
		Connection.instance.HandleGameRoomJoin();
	}

	void SetWaitingForText(int num)
	{
		if (num != 0)
			waitingForText.text = "Waiting for " + num + " more players...";
		else
			waitingForText.text = "Ready to play!";
	}

	public void PopulateUserList(List<User> users)
	{
		userList.text = "";

		for (int i = 0; i < readyList.Count; i++)
		{
			userList.text += readyList[i] + " (Ready)";
			userList.text += "\n";
		}

		for (int i = 0; i < users.Count; i++)
		{
			if (users[i] == Connection.instance.Sfs.MySelf)
				continue;

			userList.text += users[i].Name;
			userList.text += "\n";
		}

		int neededUsers = Connection.instance.MaxPlayers - (users.Count + readyList.Count);
		SetWaitingForText(neededUsers);
		if (neededUsers <= 0)
			readyButton.interactable = true;
	}

	public void AddToReadyList(string name)
	{
		readyList.Add(name);
	}
}
