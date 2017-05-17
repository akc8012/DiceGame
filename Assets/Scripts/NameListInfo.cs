using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NameListInfo
{
	List<string> names;
	public List<string> GetNames { get { return names; } }

	public NameListInfo()
	{
		names = new List<string>();

		try
		{
			LoadTextFile();
		}
		catch (Exception e)
		{
			Debug.LogError("The file could not be read: " + e.Message);
		}
	}

	void LoadTextFile()
	{
		// Open the text file using a stream reader.
		using (StreamReader sr = new StreamReader("names.list"))
		{
			int counter = 0; string line;
			while ((line = sr.ReadLine()) != null)
			{
				names.Add(line);
				counter++;
			}
		}

		Debug.Log("loaded name list");
	}
}
