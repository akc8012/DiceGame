using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ConnectUI : MonoBehaviour
{
	[SerializeField] GameObject nameListUI;

	[SerializeField] InputField hostInput;
	[SerializeField] InputField portInput;
	//[SerializeField] InputField nameInput;
	[SerializeField] Button button;

	//public string username { get { return nameInput.text; } }

	public void Awake()
	{
		
	}
	
	public void Init(string defaultHost, int defaultPort)
	{
		hostInput.text = defaultHost;
		portInput.text = defaultPort.ToString();
		button.interactable = true;
		button.onClick.AddListener(OnButtonClick);
	}

	public void OnButtonClick()
	{
		if (Connection.instance.Connect(hostInput.text, portInput.text))
		{
			nameListUI.SetActive(true);
			gameObject.SetActive(false);
		}
	}

	public void SetButtonInteractable(bool enable)
	{
		button.interactable = enable;
	}
}
