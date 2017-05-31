using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChipDrawBox : MonoBehaviour
{
	[SerializeField] GameObject chip;

	[SerializeField] int startingChips = 0;

	const int amountOfCols = 8;
	const float offset = 0.75f;

	void Start()
	{
		SetChipsToDraw(startingChips);
	}

	public void SetChipsToDraw(int amount)
	{
		foreach (Transform child in transform)
		{
			Destroy(child.gameObject);
		}

		int xOffset = 0;
		int yOffset = 0;
		for (int i = 0; i < amount; i++)
		{
			xOffset++;
			if (i % amountOfCols == 0)
			{
				yOffset++;
				xOffset = 0;
			}
				
			GameObject chipObj = Instantiate(chip, transform.position + (Vector3.right * offset * xOffset) + 
				(Vector3.down * offset * yOffset), Quaternion.identity);

			chipObj.transform.parent = transform;
		}
	}
}
