using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Sfs2X.Entities;	// User

public class LobbyUI : MonoBehaviour
{
	[SerializeField] Button joinButton;
	[SerializeField] Text userList;
	[SerializeField] Text waitingList;

	NameListInfo nameListInfo;

	void Start()
	{
		joinButton.onClick.AddListener(JoinButtonClicked);
	}

	void JoinButtonClicked()
	{
		Connection.instance.HandleGameRoomJoin();
	}

	public void PopulateUserList(List<User> users)
	{
		List<string> userNames = new List<string>();

		foreach (User user in users)
		{
			bool isSelf = (user == Connection.instance.Sfs.MySelf);
			userNames.Add(user.Name + (isSelf ? " (You)" : ""));
		}

		userList.text = "";
		for (int i = 0; i < userNames.Count; i++)
		{
			userList.text += userNames[i] + "\n";
		}

		PopulateWaitingList(userNames);
	}

	void PopulateWaitingList(List<string> userNames)
	{
		nameListInfo = new NameListInfo();
		List<string> names = nameListInfo.GetNames;

		waitingList.text = "";
		for (int i = 0; i < names.Count; i++)
		{
			if (!InJoinedList(userNames, names[i]) && 
			(names[i] != Connection.instance.Sfs.MySelf.Name))
				waitingList.text += names[i] + "\n";
		}
	}

	bool InJoinedList(List<string> userNames, string user)
	{
		if (user.Contains("(You)")) return true;

		for (int i = 0; i < userNames.Count; i++)
		{
			if (string.Compare(user, userNames[i]) == 0)
				return true;
		}

		return false;
	}
}
