using System.Runtime.InteropServices;
using UnityEngine;

public class keychainPlugin
{
	public static int getKCValue(string id)
	{
		if (id == Defs.Coins)
		{
			return 9999;
		}
		return PlayerPrefs.GetInt(id + "_KC");
	}

	public static bool createKCValue(int val, string id)
	{
		if (PlayerPrefs.HasKey(id))
		{
			return false;
		}

		PlayerPrefs.SetInt(id + "_KC", val);
		return true;
	}

	public static bool updateKCValue(int val, string id)
	{
		PlayerPrefs.SetInt(id + "_KC", val);
		return true;
	}
}