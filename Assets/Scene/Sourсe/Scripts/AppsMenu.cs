using UnityEngine;

public class AppsMenu : MonoBehaviour {
	public Texture fon;

	public Texture pixlgun3d;

	public Texture man;

	public Texture androidFon;

	public GUIStyle shooter;

	public GUIStyle skinsmaker;

	private void Update()
	{
		if (Application.platform == RuntimePlatform.Android && Input.GetKey(KeyCode.Escape))
		{
			Application.Quit();
		}
	}

	private void Start()
	{
		PhotonNetwork.PhotonServerSettings.UseCloud("538670b4-9abd-42d8-9cbb-ec6f00c7f8c5", 0);
#if UNITY_EDITOR
        Invoke("LoadLoading", 1f/30f);
#else
        Invoke("LoadLoading", 0.1f);
#endif
    }

    private void LoadLoading()
	{
		GlobalGameController.currentLevel = -1;
        Application.LoadLevel("Loading");
    }

    private void OnGUI()
	{
		Rect position = new Rect(((float)Screen.width - 2048f * (float)Screen.height / 1154f) / 2f, 0f, 2048f * (float)Screen.height / 1154f, Screen.height);
		GUI.DrawTexture(position, androidFon, ScaleMode.StretchToFill);
	}
}
