using UnityEngine;
using System.Collections;

public class Loader : MonoBehaviour
{
	[SerializeField] GameObject connection;

    void Awake()
	{
		if (Connection.instance == null)
			Instantiate(connection);
	}
}
