using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NameListUI : MonoBehaviour
{
	[SerializeField] Button[] nameButtons;

	NameListInfo nameListInfo;

	void Start()
	{
		SetNameButtons();

		nameButtons[0].onClick.AddListener(() => NameButtonClicked(0));
		nameButtons[1].onClick.AddListener(() => NameButtonClicked(1));
		nameButtons[2].onClick.AddListener(() => NameButtonClicked(2));
		nameButtons[3].onClick.AddListener(() => NameButtonClicked(3));
		nameButtons[4].onClick.AddListener(() => NameButtonClicked(4));
	}

	public void SetNameButtons()
	{
		nameListInfo = new NameListInfo();
		List<string> names = nameListInfo.GetNames;
		
		for (int i = 0; i < names.Count; i++)
		{
			nameButtons[i].GetComponentInChildren<Text>().text = names[i];
		}

		for (int i = names.Count; i < nameButtons.Length; i++)
		{
			nameButtons[i].gameObject.SetActive(false);
		}
	}

	public void NameButtonClicked(int ndx)
	{
		string username = nameButtons[ndx].GetComponentInChildren<Text>().text;
		Connection.instance.Login(username);
	}
}
