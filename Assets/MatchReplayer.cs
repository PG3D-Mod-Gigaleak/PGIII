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
		}
	}

	void OnLevelWasLoaded()
	{
		ProcessInfo();
		
		loadedIn = true;
	}

	private void ProcessFrame()
	{
		while (true)
		{
			string name = dataReader.ReadString();

			Debug.LogError(name);

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

			Debug.LogError(name);

			if (name == "END")
			{
				break;
			}

			int skinIndex = infoReader.ReadInt32();

			CreatePlayer(name, Resources.Load<Texture2D>("multiplayer skins/multi_skin_" + skinIndex));
		}

		Debug.LogError("bagh");
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
		player.skin = skin;

		playerList.Add(player.nickName, player);

		return player;
	}

	private DummyPlayer LocatePlayer(string name)
	{
		return playerList[name];
	}
}
