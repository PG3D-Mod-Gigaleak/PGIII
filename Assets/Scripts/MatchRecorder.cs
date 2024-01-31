using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MatchRecorder
{
	public void StartRecording()
	{
		if (!Directory.Exists(Application.persistentDataPath))
		{
			Directory.CreateDirectory(Application.persistentDataPath);
		}

		if (!Directory.Exists(Application.persistentDataPath + "/Recordings"))
		{
			Directory.CreateDirectory(Application.persistentDataPath + "/Recordings");
		}

		long ticks = DateTime.Now.Ticks;

		recordingStart = Time.time;

		infoWriter = new BinaryWriter(File.Open(Application.persistentDataPath + "/Recordings/Recording_" + ticks + ".info", FileMode.OpenOrCreate));
		dataWriter = new BinaryWriter(File.Open(Application.persistentDataPath + "/Recordings/Recording_" + ticks + ".data", FileMode.OpenOrCreate));
		eventWriter = new BinaryWriter(File.Open(Application.persistentDataPath + "/Recordings/Recording_" + ticks + ".events", FileMode.OpenOrCreate));

		WriteBaseInfo();
	}

	private Dictionary<string, SkinName> cachedPlayers = new Dictionary<string, SkinName>();

	private float recordingStart;

	private void WriteBaseInfo()
	{
		infoWriter.Write(Application.loadedLevelName);
		
		foreach (SkinName player in GameObject.FindObjectsOfType<SkinName>())
		{
			if (!cachedPlayers.ContainsKey(player.NickName))
			{
				WritePlayerInfo(player);
			}
		}
	}

	private void WritePlayerInfo(SkinName player)
	{
		infoWriter.Write(player.NickName);
		infoWriter.Write(int.Parse(player.GetComponentInChildren<Player_move_c>()._skin.name.Replace("multi_skin_", "")));

		cachedPlayers.Add(player.NickName, player);
	}

	public void WriteFrame()
	{
		foreach (SkinName player in GameObject.FindObjectsOfType<SkinName>())
		{
			if (!cachedPlayers.ContainsKey(player.NickName))
			{
				WritePlayerInfo(player);
			}
			else
			{
				dataWriter.Write(player.NickName);

				dataWriter.Write(player.transform.position.x);
				dataWriter.Write(player.transform.position.y);
				dataWriter.Write(player.transform.position.z);

				dataWriter.Write(player.transform.rotation.x);
				dataWriter.Write(player.transform.rotation.y);
				dataWriter.Write(player.transform.rotation.z);
				dataWriter.Write(player.transform.rotation.w);

				Transform weapon = player.transform.GetComponentInChildren<Player_move_c>().transform;

				dataWriter.Write(weapon.transform.rotation.x);
				dataWriter.Write(weapon.transform.rotation.y);
				dataWriter.Write(weapon.transform.rotation.z);
				dataWriter.Write(weapon.transform.rotation.w);
			}
		}

		dataWriter.Write("ENDFRAME");
	}

	public void WriteEvent(EventType eventType, SkinName sender = null, string param = "")
	{
		eventWriter.Write(eventType.ToString());
		eventWriter.Write(Time.time - recordingStart);

		eventWriter.Write(sender == null ? "NULL" : sender.NickName);
		eventWriter.Write(string.IsNullOrEmpty(param) ? "NULL" : param);
	}

	public void StopRecording()
	{
		dataWriter.Write("END");
		infoWriter.Write("END");
		eventWriter.Write("END");

		infoWriter.Close();
		dataWriter.Close();
		eventWriter.Close();
	}

	private BinaryWriter infoWriter, dataWriter, eventWriter;

	public enum EventType
	{
		Shot, Reload, AmmoSpawned, AmmoDestroyed, HealthSpawned, HealthDestroyed, PlayerJoined, PlayerLeft, WeaponSwitched
	};
}
