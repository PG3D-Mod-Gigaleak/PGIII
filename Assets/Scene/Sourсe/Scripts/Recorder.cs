using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recorder : MonoBehaviour
{
	public bool recordAsFirstPerson;
	private bool recording;

	private MatchRecorder matchRecorder = new MatchRecorder();

	public static Recorder instance;

	void Start()
	{
		instance = this;
		DontDestroyOnLoad(this);
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.F2) && !recording)
		{
			recording = true;

			matchRecorder.StartRecording();
			matchRecorder.WriteFrame();
		}
		else if (recording)
		{
			if (Input.GetKeyDown(KeyCode.F2))
			{
				recording = false;
				matchRecorder.StopRecording();
			}
			else
			{
				matchRecorder.curFrame++;
				matchRecorder.WriteFrame();
			}
		}
	}

	public static void Send(MatchRecorder.EventType eventType, SkinName sender = null, string param = "")
	{
		if (instance.recording)
		{
			instance.matchRecorder.WriteEvent(eventType, sender, param);
		}
	}
}
