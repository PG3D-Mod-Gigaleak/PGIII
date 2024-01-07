using UnityEngine;

public class RestartOnTap : MonoBehaviour
{
	private void Start()
	{
	}

	private void Update()
	{
		if (Input2.touchCount > 0)
		{
			Application.LoadLevel("Level2");
		}
	}
}
