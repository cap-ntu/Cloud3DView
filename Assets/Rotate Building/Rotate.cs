using UnityEngine;
using System.Collections;

public class Rotate : MonoBehaviour {

	public Vector3 speed;

	private Transform _trans;

	// Use this for initialization
	void Start () {
		_trans = transform;
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 angles = _trans.localEulerAngles;
		angles += speed * Time.deltaTime;
		_trans.localEulerAngles = angles;
	}
}
