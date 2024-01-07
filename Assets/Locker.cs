using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Locker : MonoBehaviour
{
	void Start()
	{
		keychainPlugin.updateKCValue(999999, Defs.Coins);
		Debug.LogError(keychainPlugin.getKCValue(Defs.Coins));
		DontDestroyOnLoad(gameObject);
	}
	
	void Update()
	{
		if (Input.GetKeyDown(KeyCode.F1))
		{
			Screen.lockCursor = !Screen.lockCursor;
		}
	}
}
