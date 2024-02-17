using UnityEngine;

public class coinsPlashka : MonoBehaviour
{
	public static coinsPlashka thisScript;

	public static bool hideButtonCoins;

	private float kfSize = (float)Screen.height / 768f;

	public Texture txFonCoins;

	public GUIStyle stButCoins;

	public GUIStyle stLabelCoins;

	public Rect rectButCoins;

	public Rect rectLabelCoins;

	private int tekKolCoins;

	public static Rect symmetricRect
	{
		get
		{
			Rect result = new Rect(thisScript.rectLabelCoins.x, thisScript.rectButCoins.y, thisScript.rectButCoins.width, thisScript.rectButCoins.height);
			result.x = (float)Screen.width - result.x - result.width;
			return result;
		}
	}

	private void Awake()
	{
		thisScript = base.gameObject.GetComponent<coinsPlashka>();
		hidePlashka();
		rectButCoins = new Rect(33f * kfSize, 22f * kfSize, (float)stButCoins.normal.background.width * kfSize, (float)stButCoins.normal.background.height * kfSize);
		rectLabelCoins = new Rect(rectButCoins.x + 48f * kfSize, rectButCoins.y, 85f * kfSize, rectButCoins.height - 5f * kfSize);
		stLabelCoins.fontSize = Mathf.RoundToInt(21f * kfSize);
	}

	public static void showPlashka()
	{
		thisScript.enabled = true;
	}

	public static void hidePlashka()
	{
		thisScript.enabled = false;
	}

	private void OnGUI()
	{
		GUI.depth = -3;
		tekKolCoins = keychainPlugin.getKCValue(Defs.Coins);
		if (!hideButtonCoins)
		{
			if (GUI.Button(rectButCoins, string.Empty, stButCoins))
			{
				coinsShop.showCoinsShop();
			}
		}
		else
		{
			GUI.DrawTexture(rectButCoins, txFonCoins);
		}
		GUI.Label(rectLabelCoins, string.Empty + tekKolCoins, stLabelCoins);
	}
}
