using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ConnectUI : MonoBehaviour
{
	//GameLogic gameLogic;

	public InputField hostInput;
	public InputField portInput;
	public InputField nameInput;
	public Button button;

	public void Awake()
	{
		//gameLogic = GameObject.Find("GameLogic").GetComponent<GameLogic>();
	}
	
	public void Init(string defaultHost, int defaultPort)
	{
		hostInput.text = defaultHost;
		portInput.text = defaultPort.ToString();
		button.interactable = true;
	}

	public void OnButtonClick()
	{
		Connection.instance.Connect();
	}

	public void OnMathButtonClick()
	{
		/*if (n1Input.text != "" && n2Input.text != "")
			gameLogic.SendNumbersToServer(Convert.ToInt32(n1Input.text), Convert.ToInt32(n2Input.text));
		else
			print("some fields blank");*/
	}
}
