using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ConnectUI : MonoBehaviour
{
	[SerializeField] GameObject tempConnectUI;
	[SerializeField] GameObject enterNameUI;

	[SerializeField] InputField hostInput;
	[SerializeField] InputField portInput;
	[SerializeField] InputField playersInput;
	[SerializeField] InputField nameInput;
	[SerializeField] Button connectButton;
	[SerializeField] Button loginButton;

	public void Awake()
	{
		playersInput.text = "5";
	}
	
	public void Init(string defaultHost, int defaultPort)
	{
		hostInput.text = defaultHost;
		portInput.text = defaultPort.ToString();

		connectButton.interactable = true;
		connectButton.onClick.AddListener(OnConnectButtonClick);

		loginButton.interactable = false;
		loginButton.onClick.AddListener(OnLoginButtonClicked);
	}

	void OnConnectButtonClick()
	{
		Connection.instance.Connect(hostInput.text, portInput.text, Convert.ToInt32(playersInput.text));
	}

	public void EnableLogin()
	{
		enterNameUI.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
		tempConnectUI.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -1500);

		loginButton.interactable = true;
	}

	void OnLoginButtonClicked()
	{
		Connection.instance.Login(nameInput.text);
	}

	public void SetLoginButtonInteractable(bool enable)
	{
		loginButton.interactable = enable;
	}
}
