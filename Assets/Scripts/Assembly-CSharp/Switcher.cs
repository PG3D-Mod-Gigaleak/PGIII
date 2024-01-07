using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Switcher : MonoBehaviour
{
	[CompilerGenerated]
	private sealed class _003CStart_003Ec__AnonStorey22
	{
		internal int launchCount;

		internal float hoursForReview;

		internal string appId;
	}

	[CompilerGenerated]
	private sealed class _003CStart_003Ec__AnonStorey21
	{
		internal Action<string> handler;

		internal _003CStart_003Ec__AnonStorey22 _003C_003Ef__ref_002434;

		internal void _003C_003Em__1F(string but)
		{
			EtceteraManager.alertButtonClickedEvent -= handler;
			EtceteraBinding.askForReview(_003C_003Ef__ref_002434.launchCount, _003C_003Ef__ref_002434.hoursForReview, "Review", "Please review our app.", _003C_003Ef__ref_002434.appId);
		}
	}

	public static string LoadingInResourcesPath = "LevelLoadings";

	public static string[] loadingNames = new string[11]
	{
		"Loading_Training", "loading_Cementery", "Loading_Maze", "Loading_City", "Loading_Hospital", "Loading_Jail", "Loading_Gluk", "Loading_Arena", "Loading_Area52", "Loading_Slender",
		"Loading_Hell"
	};

	public bool NoWait;

	public bool isGameOver;

	public GameObject coinsShopPrefab;

	private Texture fonToDraw;

	public GameObject skinsManagerPrefab;

	public GameObject weaponManagerPrefab;

	public GameObject flurryPrefab;

	private void Start()
	{
		Debug.Log("0 GlobalGameController.currentLevel " + GlobalGameController.currentLevel);
		AudioListener.volume = (PlayerPrefsX.GetBool(PlayerPrefsX.SndSetting, true) ? 1 : 0);
		if (GlobalGameController._currentIndexInMapping >= GlobalGameController.NumOfLevels - 1 && GlobalGameController.currentLevel != 101 && !isGameOver)
		{
			GlobalGameController.reGenerateLevelMapping();
			GlobalGameController.setLevelToFirstInMapping();
			GlobalGameController.AllLevelsCompleted++;
		}
		if (GlobalGameController.currentLevel != 101)
		{
			GlobalGameController.numOfCompletedLevels++;
			GlobalGameController.totalNumOfCompletedLevels++;
		}
		int levelsToGetCoins = GlobalGameController.levelsToGetCoins;
		int coinsBase = GlobalGameController.coinsBase;
		int coinsBaseAdding = GlobalGameController.coinsBaseAdding;
		if (GlobalGameController.numOfCompletedLevels >= levelsToGetCoins)
		{
			int num = coinsBase + coinsBaseAdding * Mathf.Max(GlobalGameController.totalNumOfCompletedLevels / levelsToGetCoins - 1, 0);
			keychainPlugin.updateKCValue(keychainPlugin.getKCValue(Defs.Coins) + num, Defs.Coins);
			keychainPlugin.createKCValue(num, Defs.EarnedCoins);
			keychainPlugin.updateKCValue(num, Defs.EarnedCoins);
			Debug.Log(string.Empty + num + " coins earned");
			GlobalGameController.numOfCompletedLevels = 0;
		}
		Debug.Log("1 GlobalGameController.currentLevel " + GlobalGameController.currentLevel);
		if (isGameOver)
		{
			fonToDraw = Resources.Load("dead") as Texture;
			return;
		}
		if (!NoWait)
		{
			if (GlobalGameController.currentLevel == GlobalGameController.levelMapping[0])
			{
				fonToDraw = Resources.Load(Path.Combine(LoadingInResourcesPath, (GlobalGameController.AllLevelsCompleted != 0) ? "NextLoopFon" : loadingNames[GlobalGameController.levelMapping[0] + 1])) as Texture;
			}
			else if (GlobalGameController.currentLevel == -1)
			{
				fonToDraw = Resources.Load("main_loading") as Texture;
			}
			else if (GlobalGameController.currentLevel == 101)
			{
				fonToDraw = Resources.Load(Path.Combine(LoadingInResourcesPath, loadingNames[0])) as Texture;
			}
			else
			{
				fonToDraw = Resources.Load(Path.Combine(LoadingInResourcesPath, loadingNames[GlobalGameController.levelMapping[GlobalGameController._currentIndexInMapping] + 1])) as Texture;
			}
		}
		else
		{
			fonToDraw = Resources.Load(Path.Combine(LoadingInResourcesPath, loadingNames[(GlobalGameController.currentLevel == GlobalGameController.levelMapping[0]) ? (GlobalGameController.levelMapping[0] + 1) : 0])) as Texture;
		}
		Debug.Log("2 GlobalGameController.currentLevel " + GlobalGameController.currentLevel);
		if (NoWait)
		{
			LoadMenu();
		}
		else
		{
			Invoke("LoadMenu", 2.5f);
		}
		if (!GameObject.FindGameObjectWithTag("SkinsManager") && (bool)skinsManagerPrefab)
		{
			UnityEngine.Object.Instantiate(skinsManagerPrefab, Vector3.zero, Quaternion.identity);
		}
		if (!GameObject.FindGameObjectWithTag("WeaponManager") && (bool)weaponManagerPrefab)
		{
			UnityEngine.Object.Instantiate(weaponManagerPrefab, Vector3.zero, Quaternion.identity);
		}
		if (!GameObject.FindGameObjectWithTag("Flurry") && (bool)flurryPrefab)
		{
			UnityEngine.Object.Instantiate(flurryPrefab, Vector3.zero, Quaternion.identity);
		}
		if ((bool)GameObject.FindGameObjectWithTag("CoinsShop") || !coinsShopPrefab)
		{
			return;
		}
		_003CStart_003Ec__AnonStorey22 _003CStart_003Ec__AnonStorey = new _003CStart_003Ec__AnonStorey22();
		string ubiquityIdentityToken = iCloudBinding.getUbiquityIdentityToken();
		Debug.Log("ubiquityIdentityToken: " + ubiquityIdentityToken);
		Storager.Initialize(ubiquityIdentityToken == null);
		if (keychainPlugin.createKCValue(0, Defs.initValsInKeychain))
		{
			keychainPlugin.createKCValue(0, Defs.SwordSett);
			keychainPlugin.createKCValue(0, Defs.MinerWeaponSett);
			keychainPlugin.createKCValue(0, Defs.InAppBoughtSett);
			keychainPlugin.createKCValue(0, Defs.CombatRifleSett);
			keychainPlugin.createKCValue(0, Defs.GoldenEagleSett);
			keychainPlugin.createKCValue(0, Defs.MagicBowSett);
			keychainPlugin.createKCValue(0, Defs.SPASSett);
			keychainPlugin.createKCValue(0, Defs.GoldenAxeSett);
			foreach (KeyValuePair<string, string> value in InAppData.inAppData.Values)
			{
				keychainPlugin.createKCValue(0, value.Value);
			}
			if (PlayerPrefs.GetInt(Defs.SwordSett, 0) > 0)
			{
				Storager.setInt(Defs.SwordSett, PlayerPrefs.GetInt(Defs.SwordSett, 0));
			}
			if (PlayerPrefs.GetInt(Defs.MinerWeaponSett, 0) > 0)
			{
				Storager.setInt(Defs.MinerWeaponSett, PlayerPrefs.GetInt(Defs.MinerWeaponSett, 0));
			}
			if (PlayerPrefs.GetInt(Defs.CombatRifleSett, 0) > 0)
			{
				Storager.setInt(Defs.CombatRifleSett, PlayerPrefs.GetInt(Defs.CombatRifleSett, 0));
			}
			if (PlayerPrefs.GetInt(Defs.GoldenEagleSett, 0) > 0)
			{
				Storager.setInt(Defs.GoldenEagleSett, PlayerPrefs.GetInt(Defs.GoldenEagleSett, 0));
			}
			if (PlayerPrefs.GetInt(Defs.MagicBowSett, 0) > 0)
			{
				Storager.setInt(Defs.MagicBowSett, PlayerPrefs.GetInt(Defs.MagicBowSett, 0));
			}
			if (PlayerPrefs.GetInt(Defs.SPASSett, 0) > 0)
			{
				Storager.setInt(Defs.SPASSett, PlayerPrefs.GetInt(Defs.SPASSett, 0));
			}
			if (PlayerPrefs.GetInt(Defs.GoldenAxeSett, 0) > 0)
			{
				Storager.setInt(Defs.GoldenAxeSett, PlayerPrefs.GetInt(Defs.GoldenAxeSett, 0));
			}
			foreach (KeyValuePair<string, string> value2 in InAppData.inAppData.Values)
			{
				if (PlayerPrefs.GetInt(value2.Value, 0) > 0)
				{
					Storager.setInt(value2.Value, PlayerPrefs.GetInt(value2.Value, 0));
				}
			}
		}
		if (keychainPlugin.createKCValue(0, Defs.initValsInKeychain2))
		{
			keychainPlugin.createKCValue(0, Defs.ChainsawS);
			keychainPlugin.createKCValue(0, Defs.FAMASS);
			keychainPlugin.createKCValue(0, Defs.GlockSett);
		}
		if (keychainPlugin.createKCValue(0, Defs.initValsInKeychain3))
		{
			keychainPlugin.createKCValue(0, Defs.ScytheSN);
			keychainPlugin.createKCValue(0, Defs.ShovelSN);
		}
		if (keychainPlugin.createKCValue(0, Defs.initValsInKeychain4))
		{
			keychainPlugin.createKCValue(0, Defs.Sword_2_SN);
			keychainPlugin.createKCValue(0, Defs.HammerSN);
		}
		if (keychainPlugin.createKCValue(0, Defs.initValsInKeychain5))
		{
			keychainPlugin.createKCValue(0, Defs.StaffSN);
		}
		_003CStart_003Ec__AnonStorey.hoursForReview = 2f;
		_003CStart_003Ec__AnonStorey.launchCount = 5;
		_003CStart_003Ec__AnonStorey.appId = "640111933";
		if (ubiquityIdentityToken == null)
		{
			_003CStart_003Ec__AnonStorey21 _003CStart_003Ec__AnonStorey2 = new _003CStart_003Ec__AnonStorey21();
			_003CStart_003Ec__AnonStorey2._003C_003Ef__ref_002434 = _003CStart_003Ec__AnonStorey;
			_003CStart_003Ec__AnonStorey2.handler = null;
			_003CStart_003Ec__AnonStorey2.handler = _003CStart_003Ec__AnonStorey2._003C_003Em__1F;
			EtceteraManager.alertButtonClickedEvent += _003CStart_003Ec__AnonStorey2.handler;
			EtceteraBinding.showAlertWithTitleMessageAndButtons(string.Empty, "You need to login iCloud and enable Documents & Data section to save and restore game data.", new string[1] { "Ok" });
		}
		else
		{
			EtceteraBinding.askForReview(_003CStart_003Ec__AnonStorey.launchCount, _003CStart_003Ec__AnonStorey.hoursForReview, "Review", "Please review our app.", _003CStart_003Ec__AnonStorey.appId);
		}
		string @string = P31Prefs.getString(Defs.FirstLaunch);
		if (keychainPlugin.createKCValue(15, Defs.Coins))
		{
		}
		if (Storager.getInt(Defs.SwordSett) > 0)
		{
			Storager.setInt(Defs.SwordSett, Storager.getInt(Defs.SwordSett));
		}
		if (Storager.getInt(Defs.MinerWeaponSett) > 0)
		{
			Storager.setInt(Defs.MinerWeaponSett, Storager.getInt(Defs.MinerWeaponSett));
		}
		if (Storager.getInt(Defs.CombatRifleSett) > 0)
		{
			Storager.setInt(Defs.CombatRifleSett, Storager.getInt(Defs.CombatRifleSett));
		}
		if (Storager.getInt(Defs.GoldenEagleSett) > 0)
		{
			Storager.setInt(Defs.GoldenEagleSett, Storager.getInt(Defs.GoldenEagleSett));
		}
		if (Storager.getInt(Defs.MagicBowSett) > 0)
		{
			Storager.setInt(Defs.MagicBowSett, Storager.getInt(Defs.MagicBowSett));
		}
		if (Storager.getInt(Defs.SPASSett) > 0)
		{
			Storager.setInt(Defs.SPASSett, Storager.getInt(Defs.SPASSett));
		}
		if (Storager.getInt(Defs.GoldenAxeSett) > 0)
		{
			Storager.setInt(Defs.GoldenAxeSett, Storager.getInt(Defs.GoldenAxeSett));
		}
		foreach (KeyValuePair<string, string> value3 in InAppData.inAppData.Values)
		{
			if (Storager.getInt(value3.Value) > 0)
			{
				Storager.setInt(value3.Value, Storager.getInt(value3.Value));
			}
		}
		UnityEngine.Object.Instantiate(coinsShopPrefab);
	}

	private void Method()
	{
	}

	private void OnGUI()
	{
		int depth = GUI.depth;
		if (isGameOver)
		{
			GUI.depth = 4;
		}
		Rect position = new Rect(((float)Screen.width - 2048f * (float)Screen.height / 1154f) / 2f, 0f, 2048f * (float)Screen.height / 1154f, Screen.height);
		GUI.DrawTexture(position, fonToDraw, ScaleMode.StretchToFill);
	}

	private void LoadMenu()
	{
		string text;
		switch (GlobalGameController.currentLevel)
		{
		case -1:
			text = "Restart";
			break;
		case 0:
			text = "Cementery";
			break;
		case 1:
			text = "Maze";
			break;
		case 2:
			text = "City";
			break;
		case 3:
			text = "Hospital";
			break;
		case 4:
			text = "Jail";
			break;
		case 5:
			text = "Gluk";
			break;
		case 6:
			text = "Arena";
			break;
		case 7:
			text = "Area52";
			break;
		case 101:
			text = "Training";
			break;
		case 8:
			text = "Slender";
			break;
		case 9:
			text = "Castle";
			break;
		default:
			text = "Restart";
			break;
		}
		if (GlobalGameController.currentLevel == -1)
		{
			int @int = PlayerPrefs.GetInt(Defs.TrainingComplSett, 0);
			GlobalGameController.currentLevel = ((@int != 1) ? 101 : GlobalGameController.levelMapping[0]);
		}
		else if (GlobalGameController.currentLevel == 101)
		{
			GlobalGameController.currentLevel = GlobalGameController.levelMapping[0];
		}
		else
		{
			PlayerPrefs.SetInt(Defs.TrainingComplSett, 1);
			PlayerPrefs.Save();
			GlobalGameController.incrementLevel();
		}
		Debug.Log("3 GlobalGameController.currentLevel " + GlobalGameController.currentLevel);
		Application.LoadLevel(text);
	}
}
