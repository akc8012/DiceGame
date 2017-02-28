using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadedDieInfo
{
	bool isLoaded = false;
	public bool IsLoaded { get { return isLoaded; } }
	int loadedNum = 0;
	public int LoadedNum { get { return loadedNum; } }
	bool isPositive = true;
	public bool IsPositive { get { return isPositive; } }

	public LoadedDieInfo()
	{
		try
		{
			LoadTextFile();
		}
		catch (Exception e)
		{
			Debug.Log("The file could not be read:");
			Debug.Log(e.Message);
		}
	}

	void LoadTextFile()
	{
		// Open the text file using a stream reader.
		using (StreamReader sr = new StreamReader("loaded.dice"))
		{
			int counter = 0; string line;
			while ((line = sr.ReadLine()) != null)
			{
				if (line.Contains("loaded"))
				{
					isLoaded = GetNumFromString(line) > 0;
					if (!isLoaded) return;
				}

				if (line.Contains("num"))
					loadedNum = GetNumFromString(line);

				if (line.Contains("positive"))
					isPositive = GetNumFromString(line) > 0;

				counter++;
			}
		}

		Debug.Log("loaded: " + isLoaded);
		Debug.Log("loaded num: " + loadedNum);
		Debug.Log("is positive: " + isPositive);
	}

	int GetNumFromString(string text)
	{
		return Convert.ToInt32(text[text.Length-1]) - 48;
	}
}
