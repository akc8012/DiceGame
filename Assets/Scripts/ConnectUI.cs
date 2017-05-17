using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ConnectUI : MonoBehaviour
{
	//GameLogic gameLogic;

	[SerializeField] InputField hostInput;
	[SerializeField] InputField portInput;
	[SerializeField] InputField nameInput;
	[SerializeField] Button button;

	public string username { get { return nameInput.text; } }

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
		Connection.instance.Connect(hostInput.text, portInput.text, nameInput.text);
	}

	public void SetButtonInteractable(bool enable)
	{
		button.interactable = enable;
	}
}
