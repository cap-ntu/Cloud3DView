using UnityEngine;
using System.Collections;

public class OnEnterShow : MonoBehaviour {
	public GameObject target;
	
	void OnEnable () {
		if (target != null) {
			target.SetActive(true);
		}
	}
}
