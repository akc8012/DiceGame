using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChipDrawBox : MonoBehaviour
{
	[SerializeField] int startingChips = 0;

	GameObject[] chips;
	int amountOfChips;
	public int GetAmountOfChips { get { return amountOfChips; } }

	void Start()
	{
		int children = transform.childCount;

		chips = new GameObject[children];
		for (int i = 0; i < children; ++i)
			chips[i] = transform.GetChild(i).gameObject;

		SetChipsToDraw(startingChips);
	}

	public void SetChipsToDraw(int amount)
	{
		amountOfChips = amount;

		ClearChips();

		for (int i = 0; i < amount; i++)
			chips[i].SetActive(true);
	}

	public void ClearChips()
	{
		for (int i = 0; i < chips.Length; i++)
			chips[i].SetActive(false);
	}
}
