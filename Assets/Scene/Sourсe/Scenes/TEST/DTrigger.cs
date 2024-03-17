using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DTrigger : MonoBehaviour {
	public Animation toTrigger;
	void OnTriggerEnter(Collider collision) {
		if (collision.gameObject.GetComponentInParent<SkinName>() || collision.gameObject.GetComponentInParent<DummyPlayer>()) {
			toTrigger.Play();
		}
	}
}
