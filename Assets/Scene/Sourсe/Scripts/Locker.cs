using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Locker : MonoBehaviour
{
	void Start()
	{
		Application.targetFrameRate = 30;
		DontDestroyOnLoad(gameObject);
	}
	
	void Update()
	{
		if (Input.GetKeyDown(KeyCode.F1))
		{
			Screen.lockCursor = !Screen.lockCursor;
		}
		if (Input.GetKeyDown(KeyCode.F3))
		{
			Time.timeScale -= 0.25f;
		}
		if (Input.GetKeyDown(KeyCode.F4))
		{
			Time.timeScale += 0.25f;
		}
	}
}
