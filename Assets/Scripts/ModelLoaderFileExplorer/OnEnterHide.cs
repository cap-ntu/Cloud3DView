using UnityEngine;
using System.Collections;

public class OnEnterHide : MonoBehaviour {
	public GameObject target;

	void OnEnable () {
		if (target != null) {
			target.SetActive(false);
		}
	}
}
