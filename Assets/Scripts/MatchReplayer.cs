using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class MatchReplayer : MonoBehaviour
{
	public long recordingID;

	private bool loadedIn;

	private BinaryReader dataReader, infoReader, eventReader;

	private Dictionary<string, DummyPlayer> playerList = new Dictionary<string, DummyPlayer>();

	private List<Event> eventList = new List<Event>();

	private float eventTime;

	private class Event
	{
		public MatchRecorder.EventType eventType;

		public string nick, param;

		public float time;
	}

	void Start()
	{
		dataReader = new BinaryReader(File.Open(Application.persistentDataPath + "/Recordings/Recording_" + recordingID + ".data", FileMode.Open));
		infoReader = new BinaryReader(File.Open(Application.persistentDataPath + "/Recordings/Recording_" + recordingID + ".info", FileMode.Open));
		eventReader = new BinaryReader(File.Open(Application.persistentDataPath + "/Recordings/Recording_" + recordingID + ".events", FileMode.Open));

		DontDestroyOnLoad(gameObject);

		Application.LoadLevel(infoReader.ReadString());
	}

	void Update()
	{
		if (loadedIn)
		{
			ProcessFrame();
			
			if (eventList.Count > 0)
			{
				eventTime += Time.deltaTime;

				if (eventTime >= eventList[0].time)
				{
					ProcessEvent();
				}
			}
		}
	}

	void OnLevelWasLoaded()
	{
		ProcessInfo();
		ProcessEvents();

		GameObject.Find("GameController").SetActive(false);
		GameObject.Find("RenderAllInSceneObj").SetActive(false);
		GameObject.Find("GameObjects").SetActive(false);
		
		loadedIn = true;
	}

	private void ProcessFrame()
	{
		while (true)
		{
			string name = dataReader.ReadString();

			if (name == "ENDFRAME")
			{
				break;
			}
			else if (name == "END")
			{
				loadedIn = false;
				break;
			}

			DummyPlayer player = LocatePlayer(name);
			
			player.transform.position = new Vector3(dataReader.ReadSingle(), dataReader.ReadSingle(), dataReader.ReadSingle());
			player.transform.rotation = new Quaternion(dataReader.ReadSingle(), dataReader.ReadSingle(), dataReader.ReadSingle(), dataReader.ReadSingle());
			
			player.moveC.transform.rotation = new Quaternion(dataReader.ReadSingle(), dataReader.ReadSingle(), dataReader.ReadSingle(), dataReader.ReadSingle());
		}
	}

	private void ProcessInfo()
	{
		while (true)
		{
			string name = infoReader.ReadString();

			if (name == "END")
			{
				break;
			}

			int skinIndex = infoReader.ReadInt32();

			CreatePlayer(name, Resources.Load<Texture2D>("multiplayer skins/multi_skin_" + skinIndex));
		}
	}

	private void ProcessEvents()
	{
		while (true)
		{
			string eventName = eventReader.ReadString();

			if (eventName == "END")
			{
				break;
			}

			float time = eventReader.ReadSingle();

			string nick = eventReader.ReadString();
			string param = eventReader.ReadString();

			eventList.Add(new Event { eventType = (MatchRecorder.EventType)System.Enum.Parse(typeof(MatchRecorder.EventType), eventName), time = time, nick = nick, param = param });
		}
	}

	private void ProcessEvent()
	{
		Event @event = eventList[0];

		switch (@event.eventType)
		{
			case MatchRecorder.EventType.Shot:
				OnShot(@event.nick);
				break;

			case MatchRecorder.EventType.Reload:
				OnReload(@event.nick);
				break;

			case MatchRecorder.EventType.WeaponSwitched:
				OnSwitched(@event.nick, @event.param);
				break;
		}

		eventList.RemoveAt(0);
	}

	private DummyPlayer CreatePlayer(string name, Texture2D skin)
	{
		DummyPlayer player = Instantiate(Resources.Load<GameObject>("DummyPlayer")).GetComponent<DummyPlayer>();

		Material skinMat = new Material(Shader.Find("Mobile/Diffuse"))
        {
            mainTexture = skin
        };
		
		foreach (Renderer renderer in player.GetComponentsInChildren<Renderer>())
		{
            renderer.sharedMaterial = skinMat;
		}

		player.nickName = name;
		player.skin = skinMat;

		playerList.Add(player.nickName, player);

		return player;
	}

	private void OnShot(string nick)
	{
		LocatePlayer(nick).Shoot();
	}

	private void OnReload(string nick)
	{
		LocatePlayer(nick).Reload();
	}

	private void OnSwitched(string nick, string weapon)
	{
		LocatePlayer(nick).ChangeWeapon(weapon);
	}

	private DummyPlayer LocatePlayer(string name)
	{
		return playerList[name];
	}
}
