using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Locker : MonoBehaviour
{
	void Awake()
	{
		Resources.Load<ServerSettings>("PhotonServerSettings").UseCloud("83abbecb-d18d-4f34-9b4f-2f024e5a5993", 0);
	}

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
