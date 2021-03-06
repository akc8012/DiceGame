﻿using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadedDieInfo
{
	bool isLoaded = false;
	int min, max;
	const int rollMin = 1;
	const int rollMax = 6;

	const string minError = "Loaded die minimum number should not be less than 1.";
	const string maxError = "Loaded die maximum number should not be greater than 6.";

	public LoadedDieInfo()
	{
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

				if (line.Contains("range"))
				{
					int maxFromLine = Convert.ToInt32(line[line.Length-1]) - 48;
					int minFromLine = Convert.ToInt32(line[line.Length-3]) - 48;

					if (maxFromLine > rollMax) throw new Exception(maxError);
					if (minFromLine < rollMin) throw new Exception(minError);

					max = maxFromLine;
					min = minFromLine;
				}

				counter++;
			}
		}

		Debug.Log("loaded: " + isLoaded);
		Debug.Log("min: " + min + ", max: " + max);
	}

	int GetNumFromString(string text)
	{
		return Convert.ToInt32(text[text.Length-1]) - 48;
	}

	public int LoadedMin()
	{
		if (!isLoaded) return rollMin;
		return min;
	}

	public int LoadedMax()
	{
		if (!isLoaded) return rollMax;
		return max;
	}
}
