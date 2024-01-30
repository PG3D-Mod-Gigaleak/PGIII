using UnityEngine;

public class HealthItem : MonoBehaviour
{
	private Player_move_c test;

	private GameObject player;

	private bool isKilled;

	public AudioClip HealthItemUp;

	private PhotonView photonView;

	private void Start()
	{
		photonView = PhotonView.Get(this);
		Recorder.Send(MatchRecorder.EventType.HealthSpawned, null, transform.position.x + " " + transform.position.y + " " + transform.position.z);
		if (PlayerPrefs.GetInt("MultyPlayer") == 1)
		{
			if (GameObject.FindGameObjectWithTag("WeaponManager").GetComponent<WeaponManager>() != null && GameObject.FindGameObjectWithTag("WeaponManager").GetComponent<WeaponManager>().myGun != null)
			{
				test = GameObject.FindGameObjectWithTag("WeaponManager").GetComponent<WeaponManager>().myGun.GetComponent<Player_move_c>();
			}
			if (GameObject.FindGameObjectWithTag("WeaponManager").GetComponent<WeaponManager>() != null)
			{
				player = GameObject.FindGameObjectWithTag("WeaponManager").GetComponent<WeaponManager>().myPlayer;
			}
		}
		else
		{
			GameObject gameObject = GameObject.FindGameObjectWithTag("PlayerGun");
			if ((bool)gameObject)
			{
				test = gameObject.GetComponent<Player_move_c>();
			}
			player = GameObject.FindGameObjectWithTag("WeaponManager").GetComponent<WeaponManager>().myPlayer;
		}
	}

	[RPC]
	private void delBonus(NetworkViewID id, NetworkViewID idPlayer)
	{
		GameObject[] array = GameObject.FindGameObjectsWithTag("Bonus");
		GameObject[] array2 = array;
		foreach (GameObject gameObject in array2)
		{
			if (!id.Equals(gameObject.GetComponent<NetworkView>().viewID))
			{
				continue;
			}
			GameObject[] array3 = GameObject.FindGameObjectsWithTag("Player");
			GameObject[] array4 = array3;
			foreach (GameObject gameObject2 in array4)
			{
				if (idPlayer.Equals(gameObject2.GetComponent<NetworkView>().viewID) && gameObject2 != null)
				{
					gameObject2.transform.Find("GameObject").GetComponent<AudioSource>().PlayOneShot(gameObject.GetComponent<HealthItem>().HealthItemUp);
				}
			}
			Object.Destroy(gameObject, 0.3f);
			break;
		}
	}

	[RPC]
	private void delBonusPhoton(int id, int idPlayer)
	{
		GameObject[] array = GameObject.FindGameObjectsWithTag("Bonus");
		GameObject[] array2 = array;
		foreach (GameObject gameObject in array2)
		{
			if (id != gameObject.GetComponent<PhotonView>().viewID)
			{
				continue;
			}
			GameObject[] array3 = GameObject.FindGameObjectsWithTag("Player");
			GameObject[] array4 = array3;
			foreach (GameObject gameObject2 in array4)
			{
				if (idPlayer == gameObject2.GetComponent<PhotonView>().viewID && gameObject2 != null)
				{
					gameObject2.transform.Find("GameObject").GetComponent<AudioSource>().PlayOneShot(gameObject.GetComponent<HealthItem>().HealthItemUp);
				}
			}
			Object.Destroy(gameObject, 0.3f);
			break;
		}
	}

	private void Update()
	{
		if (isKilled)
		{
			return;
		}
		if (test == null || player == null)
		{
			if (PlayerPrefs.GetInt("MultyPlayer") == 1)
			{
				if (GameObject.FindGameObjectWithTag("WeaponManager").GetComponent<WeaponManager>() != null && GameObject.FindGameObjectWithTag("WeaponManager").GetComponent<WeaponManager>().myGun != null)
				{
					test = GameObject.FindGameObjectWithTag("WeaponManager").GetComponent<WeaponManager>().myGun.GetComponent<Player_move_c>();
				}
				if (GameObject.FindGameObjectWithTag("WeaponManager").GetComponent<WeaponManager>() != null)
				{
					player = GameObject.FindGameObjectWithTag("WeaponManager").GetComponent<WeaponManager>().myPlayer;
				}
			}
			else
			{
				GameObject gameObject = GameObject.FindGameObjectWithTag("PlayerGun");
				if ((bool)gameObject)
				{
					test = gameObject.GetComponent<Player_move_c>();
				}
				player = GameObject.FindGameObjectWithTag("Player");
			}
		}
		if (test == null || player == null || !(Vector3.Distance(base.transform.position, player.transform.position) < 2f))
		{
			return;
		}
		test.CurHealth += 1f;
		if (PlayerPrefsX.GetBool(PlayerPrefsX.SndSetting, true))
		{
			test.gameObject.GetComponent<AudioSource>().PlayOneShot(HealthItemUp);
		}
		isKilled = true;
		if (PlayerPrefs.GetInt("MultyPlayer") == 1)
		{
			if (PlayerPrefs.GetString("TypeConnect").Equals("local"))
			{
				base.GetComponent<NetworkView>().RPC("delBonus", RPCMode.All, GetComponent<NetworkView>().viewID, player.GetComponent<NetworkView>().viewID);
			}
			else
			{
				photonView.RPC("delBonusPhoton", PhotonTargets.All, GetComponent<PhotonView>().viewID, player.GetComponent<PhotonView>().viewID);
			}
		}
		else
		{
			Recorder.Send(MatchRecorder.EventType.HealthDestroyed, null, transform.position.x + " " + transform.position.y + " " + transform.position.z);
			Object.Destroy(base.gameObject);
		}
		if (test.CurHealth > test.MaxHealth)
		{
			test.CurHealth = test.MaxHealth;
			GlobalGameController.Score += 100;
		}
	}
}
