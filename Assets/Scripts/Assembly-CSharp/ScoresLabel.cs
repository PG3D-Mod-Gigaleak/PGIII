using UnityEngine;

public class ScoresLabel : MonoBehaviour
{
	private UILabel _label;

	private void Start()
	{
		base.gameObject.SetActive(PlayerPrefs.GetInt("MultyPlayer", 0) == 0 || PlayerPrefs.GetInt("COOP", 0) == 1);
		_label = GetComponent<UILabel>();
	}

	private void Update()
	{
		_label.text = "Score\n" + GlobalGameController.Score;
	}
}
