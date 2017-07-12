using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChipMovement : MonoBehaviour
{
	const float distFromCamera = 20;
	Vector3 lastTargetPos;
	Vector2 mousePos;

	void Start()
	{
		
	}

	void Update()
	{
		Vector3 targetPos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,
			Input.mousePosition.y, Camera.main.nearClipPlane + distFromCamera));

		Vector3 deltaMovement = targetPos - lastTargetPos;
		deltaMovement *= 20;
		transform.rotation = Quaternion.Euler(deltaMovement.y, -deltaMovement.x, 0);

		transform.position = targetPos;
		lastTargetPos = targetPos;
	}
}
