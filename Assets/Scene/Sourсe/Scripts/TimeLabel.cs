using UnityEngine;

public class TimeLabel : MonoBehaviour
{
	private UILabel _label;

	private InGameGUI _inGameGUI;

	private void Start()
	{
		base.gameObject.SetActive(PlayerPrefs.GetInt("COOP", 0) == 1);
		_label = GetComponent<UILabel>();
		_inGameGUI = GameObject.FindGameObjectWithTag("InGameGUI").GetComponent<InGameGUI>();
	}

	private void Update()
	{
		if ((bool)_inGameGUI && (bool)_label)
		{
			_label.text = _inGameGUI.timeLeft();
		}
	}
}
