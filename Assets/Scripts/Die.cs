using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Die : MonoBehaviour
{
	[SerializeField] Sprite[] spriteSheet;
	SpriteRenderer spriteRenderer;
	LoadedDieInfo loadedDieInfo;

	bool isRolling = false;
	public bool CanRoll { get { return !isRolling; } }

	public delegate void RollAction(int roll);
	public event RollAction DoneRoll;

	int[] roll0 = new int[20] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };	// shouldn't roll this
	int[] roll1 = new int[20] { 0, 1, 2, 3, 4, 5, 0, 1, 2, 3, 4, 5, 0, 1, 2, 3, 4, 2, 1, 1 };
	int[] roll2 = new int[20] { 0, 1, 2, 3, 4, 5, 0, 1, 2, 3, 4, 5, 0, 1, 2, 3, 4, 2, 1, 2 };
	int[] roll3 = new int[20] { 0, 1, 2, 3, 4, 5, 0, 1, 2, 3, 4, 5, 0, 1, 2, 3, 4, 2, 1, 3 };
	int[] roll4 = new int[20] { 0, 1, 2, 3, 4, 5, 0, 1, 2, 3, 4, 5, 0, 1, 2, 3, 4, 2, 1, 4 };
	int[] roll5 = new int[20] { 0, 1, 2, 3, 4, 5, 0, 1, 2, 3, 4, 5, 0, 1, 2, 3, 4, 2, 1, 5 };
	int[] roll6 = new int[20] { 0, 1, 2, 3, 4, 5, 0, 1, 2, 3, 4, 5, 0, 1, 2, 3, 4, 2, 1, 6 };

	int[][] rolls;

	void Start()
	{
		spriteRenderer = GetComponent<SpriteRenderer>();
		rolls = new int[][] { roll0, roll1, roll2, roll3, roll4, roll5, roll6 };

		loadedDieInfo = new LoadedDieInfo();

		transform.position = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 10));
		transform.position += Vector3.one * 1.25f;
	}

	public int GetRandomRoll()
	{
		int roll;
		// add one to max due to exclusive
		roll = UnityEngine.Random.Range(loadedDieInfo.LoadedMin(), loadedDieInfo.LoadedMax() + 1);

		return roll;
	}

	public void RollTheDie(int num)
	{
		print((num) + "!");
		StartCoroutine(RollRoutine(num));
	}

	void InputNumbers()
	{
		if (Input.GetKeyDown(KeyCode.Alpha1) && !isRolling)
			StartCoroutine(RollRoutine(1));

		if (Input.GetKeyDown(KeyCode.Alpha2) && !isRolling)
			StartCoroutine(RollRoutine(2));

		if (Input.GetKeyDown(KeyCode.Alpha3) && !isRolling)
			StartCoroutine(RollRoutine(3));

		if (Input.GetKeyDown(KeyCode.Alpha4) && !isRolling)
			StartCoroutine(RollRoutine(4));

		if (Input.GetKeyDown(KeyCode.Alpha5) && !isRolling)
			StartCoroutine(RollRoutine(5));

		if (Input.GetKeyDown(KeyCode.Alpha6) && !isRolling)
			StartCoroutine(RollRoutine(6));
	}

	IEnumerator RollRoutine(int ndx)
	{
		isRolling = true;

		const float incSpeed = 0.05f;
		float speed = 0.05f;

		const int framesBeforeInc = 12;
		int frames = 0;

		while (frames < 20)
		{
			spriteRenderer.sprite = spriteSheet[rolls[ndx][frames]];
			speed += (frames <= framesBeforeInc) ? 0 : incSpeed;
			frames++;
			
			yield return new WaitForSeconds(speed);
		}

		isRolling = false;

		if (DoneRoll != null)
			DoneRoll(ndx);
	}

}
