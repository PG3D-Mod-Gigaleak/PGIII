using UnityEngine;

public class InGameGUI : MonoBehaviour
{
	public delegate float GetFloatVAlue();

	public delegate string GetString();

	public GetFloatVAlue health;

	public GetFloatVAlue armor;

	public GetString killsToMaxKills;

	public GetString timeLeft;

	public GameObject[] hearts = new GameObject[0];

	public GameObject[] armorShields = new GameObject[0];

	public GameObject elixir;

	public GameObject scoreLabel;

	public GameObject enemiesLabel;

	public GameObject timeLabel;

	public GameObject killsLabel;

	private void Start()
	{
	}

	private void Update()
	{
		for (int i = 0; i < Player_move_c.MaxPlayerHealth; i++)
		{
			hearts[i].SetActive((float)i < health());
		}
		for (int j = 0; j < Player_move_c.MaxPlayerHealth; j++)
		{
			armorShields[j].SetActive((float)j < armor());
		}
	}
}
