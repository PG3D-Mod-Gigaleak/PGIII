using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyPlayer : MonoBehaviour
{
	public Transform moveC;

	public Animation playerAnimation;

	public string nickName;

	public Material skin;

	public Camera cam;

	public GameObject body;

	private Animation currentWeapon;

	private Vector3 lastPosition;

	private void Start()
	{
		lastPosition = transform.position;
	}

	private void Update()
	{
		if (Vector3.Distance(new Vector3(lastPosition.x, 0, lastPosition.z), new Vector3(transform.position.x, 0, transform.position.z)) > 0.01f)
		{
			playerAnimation.CrossFade("Walk");

			if (currentWeapon != null && !currentWeapon.isPlaying)
			{
				currentWeapon.Play("Walk");
			}
		}
		else
		{
			playerAnimation.CrossFade("Idle");

			if (currentWeapon != null && !currentWeapon.isPlaying)
			{
				currentWeapon.Play("Idle");
			}
		}

		lastPosition = transform.position;
	}

	public void ChangeWeapon(string weaponName)
	{
		if (currentWeapon != null)
		{
			Destroy(currentWeapon.GetComponentInParent<WeaponSounds>().gameObject);
		}

		currentWeapon = Instantiate(Resources.Load<GameObject>("weapons/" + weaponName), moveC).GetComponent<WeaponSounds>().animationObject.GetComponent<Animation>();
		currentWeapon.GetComponentInParent<WeaponSounds>().transform.localPosition = new Vector3(0, -1.7f, 0);

		foreach (Renderer renderer in currentWeapon.GetComponentsInChildren<Renderer>())
		{
			if (renderer.sharedMaterial.name.StartsWith("player"))
			{
				renderer.sharedMaterial = skin;
			}
		}
	}

	public void Shoot()
	{
		if (currentWeapon != null)
		{
			currentWeapon.Play("Shoot");

			WeaponSounds weaponSounds = currentWeapon.GetComponentInParent<WeaponSounds>();

			if (!weaponSounds.isMelee)
			{
				Instantiate(Resources.Load<GameObject>("Bullet"), weaponSounds.transform.Find("BulletSpawnPoint").position, Quaternion.LookRotation(-weaponSounds.transform.Find("BulletSpawnPoint").transform.right)).GetComponent<Bullet>().lifeS = 500f;
				currentWeapon.GetComponentInParent<FlashFire>().fire();
			}

			GetComponent<AudioSource>().PlayOneShot(weaponSounds.shoot);
		}
	}

	public void Reload()
	{
		if (currentWeapon != null)
		{
			currentWeapon.Play("Reload");
			GetComponent<AudioSource>().PlayOneShot(currentWeapon.GetComponentInParent<WeaponSounds>().shoot);
		}
	}
}
