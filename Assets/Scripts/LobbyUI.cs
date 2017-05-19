using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sfs2X.Entities;	// User

public class LobbyUI : MonoBehaviour
{
	[SerializeField] Button joinButton;
	[SerializeField] Text userList;

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
	}
}
