using UnityEngine;

public class KillsLabel : MonoBehaviour
{
	private UILabel _label;

	private InGameGUI _inGameGUI;

	private void Start()
	{
		base.gameObject.SetActive(PlayerPrefs.GetInt("MultyPlayer", 0) == 1 && PlayerPrefs.GetInt("COOP", 0) == 0);
		_label = GetComponent<UILabel>();
		_inGameGUI = GameObject.FindGameObjectWithTag("InGameGUI").GetComponent<InGameGUI>();
	}

	private void Update()
	{
		if ((bool)_inGameGUI && (bool)_label)
		{
			_label.text = _inGameGUI.killsToMaxKills();
		}
	}
}
