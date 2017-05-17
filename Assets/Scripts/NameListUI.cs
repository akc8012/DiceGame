using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Sfs2X.Entities;   // User

public class NameListUI : MonoBehaviour
{
	[SerializeField] Button[] nameButtons;

	NameListInfo nameListInfo;
	List<string> joinedUsers;

	void Awake()
	{
		joinedUsers = new List<string>();
	}

	void Start()
	{
		SetNameButtons();

		nameButtons[0].onClick.AddListener(() => NameButtonClicked(0));
		nameButtons[1].onClick.AddListener(() => NameButtonClicked(1));
		nameButtons[2].onClick.AddListener(() => NameButtonClicked(2));
		nameButtons[3].onClick.AddListener(() => NameButtonClicked(3));
		nameButtons[4].onClick.AddListener(() => NameButtonClicked(4));
	}

	public void UpdateJoinedMembersList(List<User> users)
	{
		joinedUsers.Clear();

		foreach (User user in users)
		{
			if (user != Connection.instance.Sfs.MySelf)
				joinedUsers.Add(user.Name);
		}

		SetNameButtons();
	}

	void SetNameButtons()
	{
		nameListInfo = new NameListInfo();
		List<string> names = nameListInfo.GetNames;
		
		for (int i = 0; i < names.Count; i++)
		{
			bool joined = UserHasJoinedRoom(names[i]);
			string text = names[i] + (joined ? " (joined)" : "");

			nameButtons[i].GetComponentInChildren<Text>().text = text;
			nameButtons[i].interactable = !joined;
		}

		for (int i = names.Count; i < nameButtons.Length; i++)
		{
			nameButtons[i].gameObject.SetActive(false);
		}
	}

	bool UserHasJoinedRoom(string user)
	{
		for (int i = 0; i < joinedUsers.Count; i++)
		{
			if (string.Compare(user, joinedUsers[i]) == 0)
				return true;
		}

		return false;
	}

	public void NameButtonClicked(int ndx)
	{
		string username = nameButtons[ndx].GetComponentInChildren<Text>().text;
		Connection.instance.Login(username);
	}
}
