using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Die : MonoBehaviour
{
	[SerializeField] Sprite[] spriteSheet;
	SpriteRenderer spriteRenderer;

	bool isRolling = false;
	public bool CanRoll { get { return !isRolling; } }

	int[] roll1 = new int[20] { 0, 1, 2, 3, 4, 5, 0, 1, 2, 3, 4, 5, 0, 1, 2, 3, 4, 2, 1, 0 };
	int[] roll2 = new int[20] { 0, 1, 2, 3, 4, 5, 0, 1, 2, 3, 4, 5, 0, 1, 2, 3, 4, 2, 1, 1 };
	int[] roll3 = new int[20] { 0, 1, 2, 3, 4, 5, 0, 1, 2, 3, 4, 5, 0, 1, 2, 3, 4, 2, 1, 2 };
	int[] roll4 = new int[20] { 0, 1, 2, 3, 4, 5, 0, 1, 2, 3, 4, 5, 0, 1, 2, 3, 4, 2, 1, 3 };
	int[] roll5 = new int[20] { 0, 1, 2, 3, 4, 5, 0, 1, 2, 3, 4, 5, 0, 1, 2, 3, 4, 2, 1, 4 };
	int[] roll6 = new int[20] { 0, 1, 2, 3, 4, 5, 0, 1, 2, 3, 4, 5, 0, 1, 2, 3, 4, 2, 1, 5 };

	int[][] rolls;

	void Start()
	{
		spriteRenderer = GetComponent<SpriteRenderer>();
		rolls = new int[][] { roll1, roll2, roll3, roll4, roll5, roll6 };
	}

	void Update()
	{
		/*InputNumbers();

		if (Input.GetKeyDown(KeyCode.Space))
		{
			int randRoll = Random.Range(0, 6);
			print((randRoll+1) + "!");
			StartCoroutine(RollRoutine(rolls[randRoll]));
		}*/
	}

	public void RollTheDie(int num)
	{
		print((num + 1) + "!");
		StartCoroutine(RollRoutine(rolls[num]));
	}

	void InputNumbers()
	{
		if (Input.GetKeyDown(KeyCode.Alpha1) && !isRolling)
			StartCoroutine(RollRoutine(roll1));

		if (Input.GetKeyDown(KeyCode.Alpha2) && !isRolling)
			StartCoroutine(RollRoutine(roll2));

		if (Input.GetKeyDown(KeyCode.Alpha3) && !isRolling)
			StartCoroutine(RollRoutine(roll3));

		if (Input.GetKeyDown(KeyCode.Alpha4) && !isRolling)
			StartCoroutine(RollRoutine(roll4));

		if (Input.GetKeyDown(KeyCode.Alpha5) && !isRolling)
			StartCoroutine(RollRoutine(roll5));

		if (Input.GetKeyDown(KeyCode.Alpha6) && !isRolling)
			StartCoroutine(RollRoutine(roll6));
	}

	IEnumerator RollRoutine(int[] roll)
	{
		isRolling = true;

		const float incSpeed = 0.05f;
		float speed = 0.05f;

		const int framesBeforeInc = 12;
		int frames = 0;

		while (frames < 20)
		{
			spriteRenderer.sprite = spriteSheet[roll[frames]];
			speed += (frames <= framesBeforeInc) ? 0 : incSpeed;
			frames++;
			
			yield return new WaitForSeconds(speed);
		}

		isRolling = false;
	}

}
