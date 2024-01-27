using System.Runtime.InteropServices;
using UnityEngine;

public class keychainPlugin
{
	public static int getKCValue(string id)
	{
		return PlayerPrefs.GetInt(id);
	}

	public static bool createKCValue(int val, string id)
	{
		PlayerPrefs.SetInt(id, val);
		return true;
	}

	public static bool createKCValue(string val, string id)
	{
		PlayerPrefs.SetString(id, val);
		return true;
	}

	public static bool updateKCValue(int val, string id)
	{
		PlayerPrefs.SetInt(id, val);
		return true;
	}

	public static bool updateKCValue(string val, string id)
	{
		PlayerPrefs.SetString(id, val);
		return true;
	}

	public static void deleteKCValue(string id)
	{
		PlayerPrefs.SetInt(id, 0);
	}
}