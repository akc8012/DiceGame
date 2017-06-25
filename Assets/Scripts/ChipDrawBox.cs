using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChipDrawBox : MonoBehaviour
{
	[SerializeField] int startingChips = 0;
	[SerializeField] Text numText;

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

		if (numText) numText.text = amountOfChips.ToString();
	}

	public void ClearChips()
	{
		for (int i = 0; i < chips.Length; i++)
			chips[i].SetActive(false);

		if (numText) numText.text = "0";
	}
}
