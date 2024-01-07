using UnityEngine;

public class ElixirSprite : MonoBehaviour
{
	private void Start()
	{
		bool flag = PlayerPrefs.GetInt("MultyPlayer", 0) == 0;
		base.gameObject.SetActive(flag);
		if (flag)
		{
		}
	}

	private void Update()
	{
		base.gameObject.SetActive(Defs.NumberOfElixirs > 0);
	}
}
