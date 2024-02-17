using UnityEngine;

public class EnemiesLabel : MonoBehaviour
{
	private UILabel _label;

	private ZombieCreator _zombieCreator;

	private void Start()
	{
		bool flag = PlayerPrefs.GetInt("MultyPlayer", 0) == 0;
		base.gameObject.SetActive(flag);
		if (flag)
		{
			_label = GetComponent<UILabel>();
			_zombieCreator = GameObject.FindGameObjectWithTag("GameController").GetComponent<ZombieCreator>();
		}
	}

	private void Update()
	{
		_label.text = "Enemies\n" + (GlobalGameController.EnemiesToKill - _zombieCreator.NumOfDeadZombies);
	}
}
